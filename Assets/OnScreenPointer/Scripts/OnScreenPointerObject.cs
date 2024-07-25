using UnityEngine;
using UnityEngine.UI;

namespace OnScreenPointerPlugin
{
    public class OnScreenPointerObject : MonoBehaviour
    {
        [SerializeField] private OnScreenPointerController _pointerController;
        [SerializeField] private bool _moveInCircle = false;
        [Range(0f, 1f)]
        [SerializeField] private float _circleSizeNormalized = 0.5f;
        [SerializeField] private Sprite _inScreenSprite;
        [SerializeField] private Sprite _outScreenSprite;
        [SerializeField] private Image _uiImagePrefab;

        private Vector2 _lockalOffset;
        private Image _uiImage;
        private bool _isPointerInScreen = false;
        private int _screenSizeX;
        private int _screenSizeY;
        private Vector2 _screenMidPoint;
        private Camera _localCamera;

        private void Awake()
        {
            _uiImage = Instantiate(_uiImagePrefab);
            _uiImage.raycastTarget = false;

            _uiImage.rectTransform.SetParent(_pointerController.uiContainerOfPointers);

            _screenSizeX = _pointerController.playerCamera.pixelWidth;
            _screenSizeY = _pointerController.playerCamera.pixelHeight;
            _screenMidPoint = new Vector2((int)_screenSizeX / 2, (int)_screenSizeY / 2);
            _localCamera = _pointerController.playerCamera;
        }
        
        private void Update()
        {
            var screenPos = MyScreenPosition(transform);

            _isPointerInScreen = IsPointerInScreen(screenPos);

            if (_isPointerInScreen)
            {
                _uiImage.sprite = _inScreenSprite;
                _uiImage.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _uiImage.sprite = _outScreenSprite;
                Vector2 screenPosCentered = (Vector2)screenPos - _screenMidPoint;

                if (screenPos.z < 0)
                {
                    screenPosCentered = screenPosCentered * -1;
                }

                float angle = Mathf.Atan2(screenPosCentered.y, screenPosCentered.x);
                screenPosCentered = PositionPointerObjectOffScreen(angle);
                screenPos = screenPosCentered + _screenMidPoint;

                _uiImage.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            }

            screenPos = ClampToOffsetBounds(screenPos);
            _uiImage.transform.position = screenPos;
        }

        private Vector2 PositionPointerObjectOffScreen(float angle)
        {
            if (_moveInCircle)
            {
                float smallerDimOfScreen = 0;
                smallerDimOfScreen = Mathf.Min(_screenSizeX, _screenSizeY);
                smallerDimOfScreen = smallerDimOfScreen * 0.5f * _circleSizeNormalized;
                float x = Mathf.Cos(angle) * smallerDimOfScreen;
                float y = Mathf.Sin(angle) * smallerDimOfScreen;
                return new Vector2(x, y);
            }
            else
            {
                float largerDimOfScreen = 0;
                largerDimOfScreen = Mathf.Max(_screenSizeX, _screenSizeY);
                largerDimOfScreen = largerDimOfScreen * 2;
                float x = Mathf.Cos(angle) * largerDimOfScreen;
                float y = Mathf.Sin(angle) * largerDimOfScreen;
                return new Vector2(x, y);
            }
        }

        private Vector2 ClampToOffsetBounds(Vector2 screenPos)
        {
            int x = (int)Mathf.Clamp(screenPos.x, _lockalOffset.x * _screenSizeX, _screenSizeX - _lockalOffset.x * _screenSizeX);
            int y = (int)Mathf.Clamp(screenPos.y, _lockalOffset.y * _screenSizeY, _screenSizeY - _lockalOffset.y * _screenSizeY);

            return new Vector2(x, y);
        }

        private Vector3 MyScreenPosition(Transform transform)
        {
            var screenpos = _pointerController.playerCamera.WorldToScreenPoint(transform.position);
            return screenpos;
        }

        private bool IsPointerInScreen(Vector3 screenPosition)
        {
            bool isTargetVisible = screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < _localCamera.pixelWidth && screenPosition.y > 0 && screenPosition.y < _localCamera.pixelHeight;
            return isTargetVisible;
        }

    }
}