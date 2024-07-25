using UnityEngine;

namespace OnScreenPointerPlugin
{
    public class OnScreenPointerController : MonoBehaviour
    {
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private RectTransform _uiContainerOfPointers;

        public Camera playerCamera => _playerCamera; 
        
        public RectTransform uiContainerOfPointers =>_uiContainerOfPointers;
    }
}