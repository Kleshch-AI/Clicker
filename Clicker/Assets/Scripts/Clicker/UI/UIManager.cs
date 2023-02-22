using System.Threading.Tasks;
using Clicker.Channels;
using Clicker.UI.Level;
using Clicker.UI.MainMenu;
using Clicker.UI.Popups;
using Clicker.UI.Popups.LevelInfo;
using Configuration;
using Reactive;
using UniRx;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public struct Ctx
        {
            public LevelsConfig levelsConfig;
            public UIChannel uiChannel;
            public LevelChannel levelChannel;
            public SettingsChannel settingsChannel;
        }

        [SerializeField] private MainMenuUI mainMenu;
        [SerializeField] private LevelUI levelUI;
        [SerializeField] private SettingsUI settingsUI;
        [SerializeField] private LevelInfoUI levelInfoUI;
        [SerializeField] private LevelResultsUI levelResultsUI;

        private Ctx _ctx;

        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;
            
            mainMenu.SetCtx(new MainMenuUI.Ctx
            {
                levelsConfig = _ctx.levelsConfig,

                onShowLevelInfoUI = _ctx.uiChannel.onShowLevelInfoUI,
                onClickStartLevel = _ctx.uiChannel.onClickStartLevel,
                onShowSettingsUI = _ctx.uiChannel.onShowSettingsUI,

                onStartLevel = _ctx.levelChannel.onStartLevel,
                onLevelEnd = _ctx.levelChannel.onLevelEnd,
            });

            ctx.uiChannel.onShowSettingsUI.Subscribe(ShowSettings).AddTo(this);
            ctx.uiChannel.onShowLevelInfoUI.Subscribe(ShowLevelInfo).AddTo(this);
            ctx.uiChannel.onShowLevelUI.Subscribe(ShowLevel).AddTo(this);
            ctx.uiChannel.onShowLevelResults.Subscribe((isWin, ret) => ret.Value = ShowLevelResults(isWin)).AddTo(this);
        }

        private void ShowLevel(LevelInfo info)
        {
            var levelUICtx = new LevelUI.Ctx
            {
                clicks = info.Clicks,
                seconds = info.Seconds,
                bg = info.Bg,

                onStartLevel = _ctx.levelChannel.onStartLevel,
                onShowSettingsUI = _ctx.uiChannel.onShowSettingsUI,
                secondsPassed = _ctx.levelChannel.secondsPassed,
                targetClicks = _ctx.levelChannel.targetClicks,
                onSpawnTarget = _ctx.levelChannel.onSpawnTarget,
                onLevelEnd = _ctx.levelChannel.onLevelEnd,
                onShowLevelResults = _ctx.uiChannel.onShowLevelResults,
                targetCtx = new TargetUI.Ctx
                {
                    onTargetClick = _ctx.levelChannel.onTargetClick,
                    onChangeTargetSize = _ctx.levelChannel.onChangeTargetSize,
                },
                bonusesCtx = new BonusesUI.Ctx
                {
                    onBonusSetActive = _ctx.levelChannel.onBonusSetActive,
                    onClickBonus = _ctx.levelChannel.onClickBonus,
                    onSpawnBonus = _ctx.levelChannel.onSpawnBonus,
                    onHideBonus = _ctx.levelChannel.onHideBonus,
                    onLevelEnd = _ctx.levelChannel.onLevelEnd,
                }
            };

            var level = Instantiate(levelUI, transform);
            level.SetCtx(levelUICtx);
        }

        private void ShowSettings()
        {
            var settings = Instantiate(settingsUI, transform);
            settings.SetCtx(new SettingsUI.Ctx
            {
                settingsChannel = _ctx.settingsChannel,
            });
            settings.Show();
        }

        private void ShowLevelInfo(LevelInfoUI.Ctx ctx)
        {
            var levelInfo = Instantiate(levelInfoUI, transform);
            levelInfo.SetCtx(ctx);
            levelInfo.Show();
        }

        private async Task ShowLevelResults(bool isWin)
        {
            var levelResults = Instantiate(levelResultsUI, transform);
            levelResults.Show(isWin);
            await Task.Delay(2000);
            levelResults.Hide();
        }
    }
}