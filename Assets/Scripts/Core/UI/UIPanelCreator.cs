using UnityEngine;

namespace Core.UI
{
    public class UIPanelCreator : MonoBehaviour
    {
        public T CreateUIPanel<T>(T prefab, Transform container) where T : ScreenView
        {
            return Instantiate(prefab, container);
        }
    }
}