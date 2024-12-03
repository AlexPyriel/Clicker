using System;
using System.Collections.Generic;
using Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Screens.Application.Views
{
    public class ApplicationView : ScreenView
    {
        [Inject] private readonly ViewTransitionAnimator _transitionAnimator;
        
        [Header("Buttons")]
        [SerializeField] private Button _clickerButton;
        [SerializeField] private Button _factsButton;

        [Space]
        [Header("Tabs")]
        [SerializeField] private ScreenView _clickerTab;
        [SerializeField] private ScreenView _factsTab;
        [SerializeField] private List<ScreenView> _tabs;

        public IObservable<Unit> ClickerButtonClick => _clickerButton.OnClickAsObservable();
        public IObservable<Unit> FactsButtonClick => _factsButton.OnClickAsObservable();

        public async void SelectClickerTab()
        {
            _clickerButton.interactable = false;
            _factsButton.interactable = true;
            
            _transitionAnimator.Animate(() => SwitchTab(_factsTab, _clickerTab));
        }

        public async void SelectFactsTab()
        {
            _clickerButton.interactable = true;
            _factsButton.interactable = false;
            
            _transitionAnimator.Animate(() => SwitchTab(_clickerTab,_factsTab));
        }

        private async void SwitchTab(ScreenView fromTab, ScreenView targetTab)
        {
            await fromTab.Hide();
            await targetTab.Show(); 
        }
    }
}