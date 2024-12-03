using System;
using Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Clicker.Views
{
    public class ClickerView : ScreenView
    {
        [SerializeField] private TMP_Text _currencyText;
        [SerializeField] private TMP_Text _energyText;
        [SerializeField] private Button _collectButton;

        public IObservable<Unit> CollectButtonClick => _collectButton.OnClickAsObservable();

        public void UpdateUI(int currency, int energy)
        {
            _currencyText.text = $": {currency}";
            _energyText.text = $": {energy}";
        }
    }
}