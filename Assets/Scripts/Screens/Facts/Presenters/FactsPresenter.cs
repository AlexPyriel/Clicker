using System;
using System.Threading;
using Core.Network;
using Core.UI;
using Screens.Facts.Models;
using Screens.Facts.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace Screens.Facts.Presenters
{
    public class FactsPresenter : IInitializable, IDisposable
    {
        private readonly FactsView _view;
        private readonly FactsModel _factsModel;
        private readonly CurrentFactModel _currentFactModel;
        private readonly GetFactsListCommandFactory _getFactsListCommandFactory;
        private readonly GetFactCommandFactory _getFactCommandFactory;
        private readonly ServerRequestInvoker _serverRequestInvoker;
        private readonly UIPanelCreator _panelCreator = new();

        private CancellationTokenSource _cancellationTokenSource;
        private CompositeDisposable _disposables;


        public FactsPresenter(
            FactsView view, 
            FactsModel factsModel, 
            CurrentFactModel currentFactModel, 
            GetFactsListCommandFactory getFactsListCommandFactory, 
            GetFactCommandFactory getFactCommandFactory, 
            ServerRequestInvoker serverRequestInvoker)
        {
            _view = view;
            _factsModel = factsModel;
            _currentFactModel = currentFactModel;
            _getFactsListCommandFactory = getFactsListCommandFactory;
            _getFactCommandFactory = getFactCommandFactory;
            _serverRequestInvoker = serverRequestInvoker;
        }

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _disposables = new CompositeDisposable();
            
            _view.ShowCompleted
                .Subscribe(_ => OnViewShow())
                .AddTo(_disposables);
            
            _view.HideCompleted
                .Subscribe(_ => OnViewHide())
                .AddTo(_disposables);
            
            _factsModel.Breeds
                .Subscribe(_ => InitializeTab(_factsModel))
                .AddTo(_disposables);
            
            _currentFactModel.PropertyChanged
                .Subscribe(_ => _view.ShowPopup(_currentFactModel))
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _disposables?.Dispose();
            _disposables = null;
        }
        
        private void OnViewShow()
        {
            FactsListRequestInvoker();
        }

        private void OnViewHide()
        {
            _serverRequestInvoker.CancelAllCommands();
        }
        
        private void FactsListRequestInvoker()
        {
            var command = _getFactsListCommandFactory.Create();
            _serverRequestInvoker.EnqueueCommand(command);
        }
        
        private void FactRequestInvoker(string id)
        {
            var command = _getFactCommandFactory.Create(id);
            _serverRequestInvoker.EnqueueCommand(command);
        }

        private void InitializeTab(FactsModel factsModel)
        {
            _view.ResetPanels();
            
            factsModel.Breeds.Value.ForEach(breed =>
            {
                FactPanelView factPanelView = _panelCreator.CreateUIPanel(_view.FactsPanelPrefab, _view.FactsPanelContainer);
                factPanelView.Initialize(breed.Name);
                
                factPanelView.BreedButtonClick
                    .Subscribe(_ =>
                    {
                        FactRequestInvoker(breed.Id.Value);
                    })
                    .AddTo(_disposables);
            });
        }
    }
}