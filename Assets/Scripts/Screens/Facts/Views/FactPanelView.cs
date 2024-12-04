using System;
using Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Facts.Views
{
    public class FactPanelView : ScreenView
    {
        [SerializeField] private TMP_Text _breedName;
        [SerializeField] private Button _breedButton;

        public IObservable<Unit> BreedButtonClick => _breedButton.OnClickAsObservable();

        public void Initialize(ReactiveProperty<string> breedName)
        {
            _breedName.text = breedName.Value;
        }
    }
}