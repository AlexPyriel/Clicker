using System;
using Core;
using Screens.Application.Views;
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
        [Space]
        [Header("Loader Animation")]
        [SerializeField] private LoaderAnimation _loaderAnimation;

        public IObservable<Unit> BreedButtonClick => _breedButton.OnClickAsObservable();
        public LoaderAnimation LoaderAnimation => _loaderAnimation;

        public void Initialize(ReactiveProperty<string> breedName)
        {
            _breedName.text = breedName.Value;
        }
    }
}