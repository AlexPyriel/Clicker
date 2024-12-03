using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Screens.Clicker.Models;
using Screens.Clicker.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace Screens.Clicker.Presenters
{
    public class ClickerPresenter : IInitializable, IDisposable
    {
        private readonly ClickerView _view;
        private readonly ClickerConfig _config;
        private ClickerModel _clickerModel;
        private CancellationTokenSource _cancellationTokenSource;
        private CompositeDisposable _disposables;

        public ClickerPresenter(ClickerView view, ClickerConfig config)
        {
            _view = view;
            _config = config;
        }
        
        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _disposables = new CompositeDisposable();
            _clickerModel = new ClickerModel(_config);

            _view.CollectButtonClick
                .Subscribe(_ => Collect())
                .AddTo(_disposables);

            UpdateView();
            StartBackgroundLoops();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _disposables?.Dispose();
            _disposables = null;
        }
        
        private async void StartBackgroundLoops()
        {
            UniTask energyRegenTask = EnergyRegenLoop(_cancellationTokenSource.Token);
            UniTask autoCollectTask = AutoCollectLoop(_cancellationTokenSource.Token);
            
            await UniTask.WhenAny(energyRegenTask, autoCollectTask);
        }

        private void Collect()
        {
            if (_clickerModel.TrySpendEnergy(_config.EnergyCost))
            {
                _clickerModel.AddCurrency(_config.CurrencyReward);
                UpdateView();
            }
        }

        private void UpdateView()
        {
            _view.UpdateUI(_clickerModel.Currency, _clickerModel.Energy);
        }

        private async UniTask AutoCollectLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await UniTask.Delay(_config.AutoCollectDelay, cancellationToken: cancellationToken);
                    Collect();
                }
                catch (OperationCanceledException)
                {
                    Debug.Log("AutoCollect task cancelled.");
                    break;
                }
            }
        }

        private async UniTask EnergyRegenLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await UniTask.Delay(_config.EnergyRegenDelay, cancellationToken: cancellationToken);
                
                    _clickerModel.RegenEnergy(_config.EnergyReward);
                    UpdateView();
                }
                catch (OperationCanceledException)
                {
                    Debug.Log("Energy regen task cancelled.");
                    break;
                }
            }
        }
    }
}