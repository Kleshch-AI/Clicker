using UnityEngine;
using UnityEngine.EventSystems;

namespace Clicker.UI
{
    public class ClickUI : MonoBehaviour, IPointerDownHandler
    {
        protected RectTransform _rt;
        
        private void Start()
        {
            _rt = transform as RectTransform;
        }

        public void Respawn(Vector2 coord)
        {
            _rt.anchoredPosition = coord;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnClick();
        }
        
        protected virtual void OnClick()
        {
            
        }
    }
}