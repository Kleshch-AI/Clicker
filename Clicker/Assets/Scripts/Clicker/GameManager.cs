using System;
using Clicker.Channels;
using Clicker.Level;
using Configuration;
using UI;
using UniRx;
using UnityEngine;

namespace Clicker
{
    public class GameManager : IDisposable
    {
        private readonly CompositeDisposable _disp;

        public GameManager(UIManager uiManager)
        {
            var levelsConfig = Resources.Load<LevelsConfig>("LevelsConfig");

            var levelChannel = new LevelChannel();
            var uiChannel = new UIChannel();

            _disp = new CompositeDisposable();

            var levelManager = new LevelManager(new LevelManager.Ctx
            {
                levelsConfig = levelsConfig,

                levelChannel = levelChannel,
                onClickStartLevel = uiChannel.onClickStartLevel,
                onShowLevelUI = uiChannel.onShowLevelUI,
            });
            _disp.Add(levelManager);

            uiManager.SetCtx(new UIManager.Ctx
            {
                levelsConfig = levelsConfig,
                uiChannel = uiChannel,
                levelChannel = levelChannel
            });
        }

        public void Dispose()
        {
            _disp?.Dispose();
        }
    }
}