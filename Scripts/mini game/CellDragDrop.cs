using UnityEngine;
using UnityEngine.EventSystems;

namespace mini_game
{
    public class CellDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        private RectTransform _rectTransform;
        private Vector2 _startPosition;
        private Transform _defaultGrid;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _defaultGrid = GameObject.Find("DefaultGrid").GetComponent<Transform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = _rectTransform.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                var dropZone = eventData.pointerCurrentRaycast.gameObject.GetComponent<GridDropZone>();

                if (dropZone != null)
                {
                    transform.SetParent(dropZone.transform);
                }
                else
                {
                    _rectTransform.anchoredPosition = _startPosition;
                }
            }
            else
            {
                _rectTransform.anchoredPosition = _startPosition;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            // not used
        }
    }
}