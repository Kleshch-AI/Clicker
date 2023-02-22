using Reactive;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.UI.MainMenu
{
    public class LevelItemUI : MonoBehaviour
    {
        public struct Ctx
        {
            public int id;
            public string title;
            public int rating;
            public Sprite bg;

            public ReactiveTrigger<int> onClickLevelItem;
        }
        
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Slider rating;
        [SerializeField] private Image bg;
        [SerializeField] private Button press;

        private Ctx _ctx;
        
        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;
            
            title.text = ctx.title;
            rating.value = ctx.rating;
            bg.sprite = ctx.bg;
            
            press.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _ctx.onClickLevelItem.Notify(_ctx.id);
        }
    }
}