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
        private readonly GetFactsCommandFactory _getFactsCommandFactory;
        private readonly ServerRequestInvoker _serverRequestInvoker;
        private readonly UIPanelCreator _panelCreator = new();

        private CancellationTokenSource _cancellationTokenSource;
        private CompositeDisposable _disposables;


        public FactsPresenter(FactsView view, FactsModel factsModel, GetFactsCommandFactory getFactsCommandFactory, ServerRequestInvoker serverRequestInvoker)
        {
            _view = view;
            _factsModel = factsModel;
            _getFactsCommandFactory = getFactsCommandFactory;
            _serverRequestInvoker = serverRequestInvoker;
        }

        public void Initialize()
        {
            Debug.Log("Initializing Facts Presenter");
            
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
            
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _disposables?.Dispose();
            _disposables = null;
        }
        
        private void OnViewShow()
        {
            FactsRequestInvoker();
        }

        private void OnViewHide()
        {
            _serverRequestInvoker.CancelAllCommands();
        }
        
        private void FactsRequestInvoker()
        {
            var command = _getFactsCommandFactory.Create();
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
                        Debug.Log($"Breed: {breed.Name}, id: {breed.Id}");
                    })
                    .AddTo(_disposables);
            });
        }
    }
}