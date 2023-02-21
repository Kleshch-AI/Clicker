using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.UI
{
    public class BasePopupUI : MonoBehaviour
    {
        [SerializeField] private Button close;

        private RectTransform _rt;
        private Vector2 _pos;
        private Vector2 _outOfScreenPos;

        private void Awake()
        {
            _rt = transform as RectTransform;
            _pos = _rt.anchoredPosition;
            var parentHeight = (transform.parent as RectTransform).rect.height;
            _outOfScreenPos = new Vector2(_pos.x, -_rt.rect.height * (1 - _rt.pivot.y) - parentHeight * _rt.anchorMin.y);
        }

        private void Start()
        {
            close.onClick.AddListener(Close);
        }

        protected virtual async Task AnimateShow()
        {
            _rt.anchoredPosition = _outOfScreenPos;
            DOTween.Sequence()
                .Append(_rt.DOAnchorPosY(_pos.y + 40, .15f))
                .Append(_rt.DOAnchorPosY(_pos.y - 30, .1f))
                .Append(_rt.DOAnchorPosY(_pos.y, .1f));
            await Task.Delay(350);
        }

        protected virtual async Task AnimateHide()
        {
            _rt.DOAnchorPos(_outOfScreenPos, .2f);
            await Task.Delay(200);
        }

        private async void Close()
        {
            await AnimateHide();
            Destroy(gameObject);
        }
    }
}