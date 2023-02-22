using Clicker.UI.Popups.LevelInfo;
using Configuration;
using Reactive;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        public struct Ctx
        {
            public LevelsConfig levelsConfig;

            public ReactiveTrigger<LevelInfoUI.Ctx> onShowLevelInfoUI;
            public ReactiveTrigger<int> onClickStartLevel;
            public ReactiveTrigger onShowSettingsUI;
            public ReactiveTrigger onStartLevel;
            public ReactiveTrigger<bool> onLevelEnd;
        }

        [SerializeField] private Transform levelsGroup;
        [SerializeField] private LevelItemUI levelItem;
        [SerializeField] private Button settings;
        
        private Ctx _ctx;
        private ReactiveTrigger<int> _onClickLevelItem;

        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;

            _onClickLevelItem = new ReactiveTrigger<int>();

            SetupLevels();
            
            settings.onClick.AddListener(() => _ctx.onShowSettingsUI.Notify());

            _onClickLevelItem.Subscribe(ShowLevelInfo).AddTo(this);
            _ctx.onStartLevel.Subscribe(Hide).AddTo(this);
            _ctx.onLevelEnd.Subscribe(_ => Show()).AddTo(this);
        }

        private void SetupLevels()
        {
            for (var i = 0; i <= _ctx.levelsConfig.MaxLevelId; i++)
            {
                var levelInfo = _ctx.levelsConfig.GetById(i);
                if (levelInfo == null)
                    continue;

                var id = i;
                var newLevelUI = Instantiate(levelItem, levelsGroup);
                newLevelUI.SetCtx(new LevelItemUI.Ctx
                {
                    id = id,
                    title = levelInfo.Title,
                    rating = Random.Range(0, 100),
                    bg = levelInfo.Bg,

                    onClickLevelItem = _onClickLevelItem
                });
            }
        }

        private void ShowLevelInfo(int id)
        {
            var levelInfo = _ctx.levelsConfig.GetById(id);
            if (levelInfo == null)
                return;

            var levelInfoUICtx = new LevelInfoUI.Ctx
            {
                id = id,
                title = levelInfo.Title,
                rating = Random.Range(0, 100),

                onClickStartLevel = _ctx.onClickStartLevel,
            };
            _ctx.onShowLevelInfoUI.Notify(levelInfoUICtx);
        }
        
        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}