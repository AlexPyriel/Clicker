using System;
using Core;
using Cysharp.Threading.Tasks;
using Screens.Clicker.Models;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Clicker.Views
{
    public class ClickerView : ScreenView
    {
        [SerializeField] private Button _collectButton;
        
        [Space]
        [SerializeField] private TMP_Text _currencyText;
        [SerializeField] private TMP_Text _energyText;
        [SerializeField] private TMP_Text _weatherText;
        
        private Subject<Unit> _showCompletedSubject = new();
        private Subject<Unit> _hideCompletedSubject = new();

        public IObservable<Unit> CollectButtonClick => _collectButton.OnClickAsObservable();
        public IObservable<Unit> ShowCompleted => _showCompletedSubject.AsObservable();
        public IObservable<Unit> HideCompleted => _hideCompletedSubject.AsObservable();
        public Button CollectButton => _collectButton;

        public override async UniTask Show()
        {
            await base.Show();
            
            _showCompletedSubject.OnNext(Unit.Default);
        }

        public override async UniTask Hide()
        {
            await base.Hide();
            
            _hideCompletedSubject.OnNext(Unit.Default);
        }
        
        public void UpdateUI(int currency, int energy)
        {
            _currencyText.text = $": {currency}";
            _energyText.text = $": {energy}";
        }

        public void UpdateWeather(WeatherModel weather)
        {
            _weatherText.text = $"{weather.Name}: {weather.Temperature} {weather.TemperatureUnit}";
        }
    }
}