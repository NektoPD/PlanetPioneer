using UnityEngine;

namespace OnScreenPointerPlugin
{
    public class OnScreenPointerController : MonoBehaviour
    {
        [SerializeField] private RectTransform _uiContainerOfPointers;
        [SerializeField] OnScreenPointerObject _onScreenPointerObject;
        
        public RectTransform uiContainerOfPointers =>_uiContainerOfPointers;
    }
}