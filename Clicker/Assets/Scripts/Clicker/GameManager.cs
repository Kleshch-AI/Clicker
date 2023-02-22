using System;
using Clicker.Channels;
using Clicker.Level;
using Clicker.Settings;
using Configuration;
using UI;
using UniRx;
using UnityEngine;

namespace Clicker
{
    public class GameManager : IDisposable
    {
        private readonly CompositeDisposable _disposables;

        public GameManager(UIManager uiManager)
        {
            var levelsConfig = Resources.Load<LevelsConfig>("LevelsConfig");

            var levelChannel = new LevelChannel();
            var uiChannel = new UIChannel();
            var settingsChannel = new SettingsChannel();

            _disposables = new CompositeDisposable();

            var levelManager = new LevelManager(new LevelManager.Ctx
            {
                levelsConfig = levelsConfig,

                levelChannel = levelChannel,
                onClickStartLevel = uiChannel.onClickStartLevel,
                onShowLevelUI = uiChannel.onShowLevelUI,
            });
            _disposables.Add(levelManager);

            var settingsPm = new SettingsPm(new SettingsPm.Ctx
            {
                settingsChannel = settingsChannel
            });
            _disposables.Add(settingsPm);

            uiManager.SetCtx(new UIManager.Ctx
            {
                levelsConfig = levelsConfig,
                uiChannel = uiChannel,
                levelChannel = levelChannel,
                settingsChannel = settingsChannel
            });
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}