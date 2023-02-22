using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.UI.Popups
{
    public class BasePopupUI : MonoBehaviour
    {
        [SerializeField] private bool hasCloseButton;
        [SerializeField] [ShowIf("$hasCloseButton")] private Button close;
        [SerializeField] private RectTransform mainPanel;

        private Vector2 _pos;
        private Vector2 _outOfScreenPos;

        private void Awake()
        {
            _pos = mainPanel.anchoredPosition;
            var screenHeight = (transform as RectTransform).rect.height;
            _outOfScreenPos = new Vector2(_pos.x,
                -mainPanel.rect.height * (1 - mainPanel.pivot.y) - screenHeight * mainPanel.anchorMin.y);
        }

        private void Start()
        {
            if (hasCloseButton)
                close.onClick.AddListener(Close);
        }

        protected async void Close()
        {
            await AnimateHide();
            Destroy(gameObject);
        }

        protected async Task AnimateShow()
        {
            mainPanel.anchoredPosition = _outOfScreenPos;
            DOTween.Sequence()
                .Append(mainPanel.DOAnchorPosY(_pos.y + 40, .15f))
                .Append(mainPanel.DOAnchorPosY(_pos.y - 30, .1f))
                .Append(mainPanel.DOAnchorPosY(_pos.y, .1f));
            await Task.Delay(350);
        }

        protected async Task AnimateHide()
        {
            mainPanel.DOAnchorPos(_outOfScreenPos, .2f);
            await Task.Delay(200);
        }
    }
}