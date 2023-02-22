using System.Collections.Generic;
using Reactive;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.UI.Popups.LevelInfo
{
    public class LevelInfoUI : BasePopupUI
    {
        public struct Ctx
        {
            public int id;
            public string title;
            public int rating;

            public ReactiveTrigger<int> onClickStartLevel;
        }

        [SerializeField] private Button start;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Slider rating;
        [SerializeField] private List<LeaderboardItemUI> players;

        private Ctx _ctx;
        
        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;

            title.text = _ctx.title;
            rating.value = _ctx.rating;
            
            start.onClick.AddListener(OnClickStart);
        }

        public async void Show()
        {
            await AnimateShow();
        }

        private void OnClickStart()
        {
            _ctx.onClickStartLevel.Notify(_ctx.id);
            Close();
        }
    }
}