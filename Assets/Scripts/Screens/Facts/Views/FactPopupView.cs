using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Facts.Views
{
    public class FactPopupView : ScreenView
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;

        public void Initialize(string breedName, string description)
        {
            Debug.Log("TRIGGERED");
            _nameText.text = breedName;
            _descriptionText.text = description;
        }
    }
}