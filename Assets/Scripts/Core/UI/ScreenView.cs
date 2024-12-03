using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public abstract class ScreenView : MonoBehaviour
    {
        public virtual async UniTask Show()
        {
            gameObject.SetActive(true);
            await UniTask.CompletedTask;
        }

        public virtual async UniTask Hide()
        {
            gameObject.SetActive(false);
            await UniTask.CompletedTask;
        }
    }
}