using Screens.Clicker.Views;
using UnityEngine;

namespace Screens.Clicker
{
    [RequireComponent(typeof(AudioSource))]
    public class ClickEffects : MonoBehaviour
    {
        [Header("Audio effects")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clickSound;

        [Space] 
        [Header("Visual Effects")] 
        [SerializeField] private Sprite _rewardIcon;
        [SerializeField] private Transform _targetTransform;
        

        public void PlayClickSound()
        {
            _audioSource.PlayOneShot(_clickSound);
        }

        public void PlayAnimation()
        {
            RewardAnimationView.Instance.Play(_rewardIcon, _targetTransform.position);
        }
    }
}