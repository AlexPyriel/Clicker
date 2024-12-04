using System;
using Core;
using Cysharp.Threading.Tasks;
using Screens.Facts.Models;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Facts.Views
{
    public class FactsView : ScreenView
    {
        [Header("Popup")]
        [SerializeField] private FactPopupView _factsPopup;
        [Space]
        [Header("Panels")]
        [SerializeField] private FactPanelView _factsPanelPrefab;
        [SerializeField] private Transform _factsPanelContainer;
        
        private Subject<Unit> _showCompletedSubject = new();
        private Subject<Unit> _hideCompletedSubject = new();
        
        public IObservable<Unit> ShowCompleted => _showCompletedSubject.AsObservable();
        public IObservable<Unit> HideCompleted => _hideCompletedSubject.AsObservable();
        
        public FactPanelView FactsPanelPrefab => _factsPanelPrefab;
        public Transform FactsPanelContainer => _factsPanelContainer;
        
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
        
        public void ResetPanels()
        {
            foreach (Transform child in _factsPanelContainer)
            {
                Destroy(child.gameObject);
            }
        }

        public void ShowPopup(CurrentFactModel model)
        {
            if (model.Name.Value is null) return;

            _factsPopup.Show();
            _factsPopup.Initialize( model.Name.Value, model.Description.Value );
            
            var layoutGroup = _factsPopup.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);
        }
    }
}