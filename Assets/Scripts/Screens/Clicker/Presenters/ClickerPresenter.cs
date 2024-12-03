using System;
using System.Threading;
using Core.Network;
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
        private readonly WeatherModel _weatherModel;
        private readonly GetWeatherCommandFactory _getWeatherCommandFactory;
        private readonly ServerRequestInvoker _serverRequestInvoker;
        private ClickerModel _clickerModel;
        private CancellationTokenSource _cancellationTokenSource;
        private CompositeDisposable _disposables;
        private bool _isRunning = true;

        public ClickerPresenter(ClickerView view, ClickerConfig config, WeatherModel weatherModel, GetWeatherCommandFactory getWeatherCommandFactory, ServerRequestInvoker serverRequestInvoker)
        {
            _view = view;
            _config = config;
            _weatherModel = weatherModel;
            _getWeatherCommandFactory = getWeatherCommandFactory;
            _serverRequestInvoker = serverRequestInvoker;
        }
        
        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _disposables = new CompositeDisposable();
            _clickerModel = new ClickerModel(_config);

            OnViewShow();

            _view.ShowCompleted
                .Subscribe(_ => OnViewShow())
                .AddTo(_disposables);
            
            _view.HideCompleted
                .Subscribe(_ => OnViewHide())
                .AddTo(_disposables);
                
            _view.CollectButtonClick
                .Subscribe(_ => Collect())
                .AddTo(_disposables);
                        
            _weatherModel.PropertyChanged
                .Subscribe(_ => _view.UpdateWeather(_weatherModel))
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

        private void OnViewShow()
        {
            _isRunning = true;
            WeatherRequestInvoker();
        }

        private void OnViewHide()
        {
            _isRunning = false;
            _serverRequestInvoker.CancelAllCommands();
        }
        
        private async void WeatherRequestInvoker()
        {
            while (_isRunning)
            {
                var command = _getWeatherCommandFactory.Create();
                _serverRequestInvoker.EnqueueCommand(command);
                await UniTask.Delay(TimeSpan.FromSeconds(5));
            }
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
                    Debug.Log("[AutoCollectLoop] AutoCollect task cancelled.");
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
                    Debug.Log("[EnergyRegenLoop] Energy regen task cancelled.");
                    break;
                }
            }
        }
    }
}