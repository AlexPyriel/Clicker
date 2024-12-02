using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Application.Views
{
    public class ApplicationView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _clickerButton;
        [SerializeField] private Button _factsButton;
        [Space]
        [Header("Tabs")]
        [SerializeField] private GameObject _clickerTab;
        [SerializeField] private GameObject _factsTab;
        
        public IObservable<Unit> ClickerButtonClick => _clickerButton.OnClickAsObservable();
        public IObservable<Unit> FactsButtonClick => _factsButton.OnClickAsObservable();
        
        public void SelectClickerTab()
        {
            _clickerTab.SetActive(true);
            _factsTab.SetActive(false);
        }

        public void SelectFactsTab()
        {
            _clickerTab.SetActive(false);
            _factsTab.SetActive(true);
        }
    }
}