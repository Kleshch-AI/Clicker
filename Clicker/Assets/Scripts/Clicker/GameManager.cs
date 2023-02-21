using System;
using System.Collections.Generic;
using Clicker.Level;
using Clicker.Level.Bonuses;
using Clicker.UI;
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
            var levelInfo = new LevelInfo
            {
                clicks = 20,
                seconds = 20,
                bonuses = new List<BonusSpawnInfo>
                {
                    new BonusSpawnInfo { type = Bonus.Size, chance = 0.2f, seconds = 5 },
                    new BonusSpawnInfo { type = Bonus.Tap, chance = 0.2f, seconds = 5 },
                    new BonusSpawnInfo { type = Bonus.Stop, chance = 0.2f, seconds = 5 }
                }
            };

            ReactiveTrigger<Vector2> onSpawnTarget = new ReactiveTrigger<Vector2>();
            ReactiveProperty<int> secondsPassed = new ReactiveProperty<int>();
            ReactiveProperty<int> targetClicks = new ReactiveProperty<int>();
            ReactiveTrigger onTargetClick = new ReactiveTrigger();
            ReactiveTrigger onStartLevel = new ReactiveTrigger();
            ReactiveTrigger<bool> onLevelEnd = new ReactiveTrigger<bool>();
            ReactiveTrigger<Bonus, Vector2> onSpawnBonus = new ReactiveTrigger<Bonus, Vector2>();
            ReactiveTrigger<Bonus> onClickBonus = new ReactiveTrigger<Bonus>();
            ReactiveTrigger<float> onChangeTargetSize = new ReactiveTrigger<float>();
            ReactiveTrigger<Bonus> onHideBonus = new ReactiveTrigger<Bonus>();
            ReactiveTrigger<int> onSetClickWeight = new ReactiveTrigger<int>();
            ReactiveTrigger<Bonus, bool> onBonusSetActive = new ReactiveTrigger<Bonus, bool>();
            ReactiveTrigger<bool> onLockTargetMove = new ReactiveTrigger<bool>();

            var clickerLevelPm = new ClickerLevelPm(new ClickerLevelPm.Ctx
            {
                levelInfo = levelInfo,

                onSpawnTarget = onSpawnTarget,
                secondsPassed = secondsPassed,
                targetClicks = targetClicks,

                onSetClickWeight = onSetClickWeight,
                onLockTargetMove = onLockTargetMove,
                onTargetClick = onTargetClick,
                onStartLevel = onStartLevel,
                onLevelEnd = onLevelEnd,
            });
            _disposables.Add(clickerLevelPm);

            var bonusesPm = new BonusesPm(new BonusesPm.Ctx
            {
                bonuses = levelInfo.bonuses,

                onSpawnBonus = onSpawnBonus,
                onChangeTargetSize = onChangeTargetSize,
                onSetClickWeight = onSetClickWeight,
                onLockTargetMove = onLockTargetMove,
                onBonusSetActive = onBonusSetActive,

                secondsPassed = secondsPassed,
                onClickBonus = onClickBonus,
                onHideBonus = onHideBonus,
            });
            _disposables.Add(bonusesPm);

            uiManager.SetCtx(new UIManager.Ctx
            {
                levelUICtx = new LevelUI.Ctx
                {
                    levelInfo = levelInfo,

                    onStartLevel = onStartLevel,
                    secondsPassed = secondsPassed,
                    targetClicks = targetClicks,
                    onSpawnTarget = onSpawnTarget,
                    onLevelEnd = onLevelEnd,
                    targetCtx = new TargetUI.Ctx
                    {
                        onTargetClick = onTargetClick,
                        onChangeTargetSize = onChangeTargetSize,
                    },
                    bonusesCtx = new BonusesUI.Ctx
                    {
                        onBonusSetActive = onBonusSetActive,
                        onClickBonus = onClickBonus,
                        onSpawnBonus = onSpawnBonus,
                        onHideBonus = onHideBonus,
                        onLevelEnd = onLevelEnd,
                    }
                }
            });
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}