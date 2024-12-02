using System;
using Screens.Application.Views;
using UniRx;
using Zenject;

namespace Screens.Application.Presenters
{
    public class ApplicationPresenter : IInitializable, IDisposable
    {
        private readonly ApplicationView _view;
        private CompositeDisposable _disposables;
        
        public ApplicationPresenter(ApplicationView view)
        {
            _view = view;
            _disposables = new CompositeDisposable();
        }

        public void Initialize()
        {
            _view.ClickerButtonClick
                .Subscribe(_ => HandleClickerButtonClick())
                .AddTo(_disposables);
            
            _view.FactsButtonClick
                .Subscribe(_ => HandleFactsButtonClick())
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
            _disposables = null;
        }

        private void HandleClickerButtonClick()
        {
            _view.SelectClickerTab();
        }

        private void HandleFactsButtonClick()
        {
            _view.SelectFactsTab();
        }
    }
}