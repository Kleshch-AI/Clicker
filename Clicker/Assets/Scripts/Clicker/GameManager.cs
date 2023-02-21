using System;
using System.Collections.Generic;
using Clicker.Channels;
using Clicker.Level;
using Clicker.Level.Bonuses;
using Clicker.UI;
using Clicker.UI.Level;
using Configuration;
using Reactive;
using UI;
using UniRx;
using UnityEngine;

namespace Clicker
{
    public class GameManager : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public GameManager(UIManager uiManager)
        {
            // var levelInfo = new LevelInfo
            // {
            //     clicks = 20,
            //     seconds = 20,
            //     bonuses = new List<BonusSpawnInfo>
            //     {
            //         new BonusSpawnInfo { type = Bonus.Size, chance = 0.2f, seconds = 5 },
            //         new BonusSpawnInfo { type = Bonus.Tap, chance = 0.2f, seconds = 5 },
            //         new BonusSpawnInfo { type = Bonus.Stop, chance = 0.2f, seconds = 5 }
            //     }
            // };

            var levelsConfig = Resources.Load<LevelsConfig>("LevelsConfig");
            var levelInfo = levelsConfig.GetById(1);

            var levelChannel = new LevelChannel();
            var uiChannel = new UIChannel();

            var clickerLevelPm = new ClickerLevelPm(new ClickerLevelPm.Ctx
            {
                levelInfo = levelInfo,

                onSpawnTarget = levelChannel.onSpawnTarget,
                secondsPassed = levelChannel.secondsPassed,
                targetClicks = levelChannel.targetClicks,

                onSetClickWeight = levelChannel.onSetClickWeight,
                onLockTargetMove = levelChannel.onLockTargetMove,
                onTargetClick = levelChannel.onTargetClick,
                onStartLevel = levelChannel.onStartLevel,
                onLevelEnd = levelChannel.onLevelEnd,
            });
            _disposables.Add(clickerLevelPm);

            var bonusesPm = new BonusesPm(new BonusesPm.Ctx
            {
                bonuses = levelInfo.Bonuses,

                onSpawnBonus = levelChannel.onSpawnBonus,
                onChangeTargetSize = levelChannel.onChangeTargetSize,
                onSetClickWeight = levelChannel.onSetClickWeight,
                onLockTargetMove = levelChannel.onLockTargetMove,
                onBonusSetActive = levelChannel.onBonusSetActive,

                secondsPassed = levelChannel.secondsPassed,
                onClickBonus = levelChannel.onClickBonus,
                onHideBonus = levelChannel.onHideBonus,
            });
            _disposables.Add(bonusesPm);

            uiManager.SetCtx(new UIManager.Ctx
            {
                levelUICtx = new LevelUI.Ctx
                {
                    levelInfo = levelInfo,

                    onStartLevel = levelChannel.onStartLevel,
                    onShowSettingsUI = uiChannel.onShowSettingsUI,
                    secondsPassed = levelChannel.secondsPassed,
                    targetClicks = levelChannel.targetClicks,
                    onSpawnTarget = levelChannel.onSpawnTarget,
                    onLevelEnd = levelChannel.onLevelEnd,
                    targetCtx = new TargetUI.Ctx
                    {
                        onTargetClick = levelChannel.onTargetClick,
                        onChangeTargetSize = levelChannel.onChangeTargetSize,
                    },
                    bonusesCtx = new BonusesUI.Ctx
                    {
                        onBonusSetActive = levelChannel.onBonusSetActive,
                        onClickBonus = levelChannel.onClickBonus,
                        onSpawnBonus = levelChannel.onSpawnBonus,
                        onHideBonus = levelChannel.onHideBonus,
                        onLevelEnd = levelChannel.onLevelEnd,
                    }
                },
                uiChannel = uiChannel
            });
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}