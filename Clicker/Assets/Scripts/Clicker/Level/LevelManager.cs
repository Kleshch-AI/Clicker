using System;
using Clicker.Channels;
using Clicker.Level.Bonuses;
using Configuration;
using Reactive;
using UniRx;
using UnityEngine;

namespace Clicker.Level
{
    public class LevelManager : IDisposable
    {
        public struct Ctx
        {
            public LevelsConfig levelsConfig;
            
            public LevelChannel levelChannel;
            public ReactiveTrigger<int> onClickStartLevel;
            public ReactiveTrigger<int, int> onShowLevelUI;
        }

        private readonly Ctx _ctx;
        private readonly CompositeDisposable _disposables;

        public LevelManager(Ctx ctx)
        {
            _ctx = ctx;
            
            _disposables = new CompositeDisposable();

            _ctx.onClickStartLevel.Subscribe(StartLevel).AddTo(_disposables);
        }

        private void StartLevel(int id)
        {
            var levelInfo = _ctx.levelsConfig.GetById(id);
            if (levelInfo == null)
            {
                Debug.LogError($"Cannot find level with id {id}!");
                return;
            }
            
            var levelPm = new LevelPm(new LevelPm.Ctx
            {
                levelInfo = levelInfo,

                onSpawnTarget = _ctx.levelChannel.onSpawnTarget,
                secondsPassed = _ctx.levelChannel.secondsPassed,
                targetClicks = _ctx.levelChannel.targetClicks,

                onSetClickWeight = _ctx.levelChannel.onSetClickWeight,
                onLockTargetMove = _ctx.levelChannel.onLockTargetMove,
                onTargetClick = _ctx.levelChannel.onTargetClick,
                onStartLevel = _ctx.levelChannel.onStartLevel,
                onLevelEnd = _ctx.levelChannel.onLevelEnd,
            });
            _disposables.Add(levelPm);
            
            if (levelInfo.Bonuses?.Count > 0)
                StartBonuses(levelInfo);
            
            _ctx.onShowLevelUI.Notify(levelInfo.Clicks, levelInfo.Seconds); //todo await show
            _ctx.levelChannel.onStartLevel.Notify();
        }

        private void StartBonuses(LevelInfo levelInfo)
        {
            var bonusesPm = new BonusesPm(new BonusesPm.Ctx
            {
                bonuses = levelInfo.Bonuses,

                onSpawnBonus = _ctx.levelChannel.onSpawnBonus,
                onChangeTargetSize = _ctx.levelChannel.onChangeTargetSize,
                onSetClickWeight = _ctx.levelChannel.onSetClickWeight,
                onLockTargetMove = _ctx.levelChannel.onLockTargetMove,
                onBonusSetActive = _ctx.levelChannel.onBonusSetActive,

                secondsPassed = _ctx.levelChannel.secondsPassed,
                onClickBonus = _ctx.levelChannel.onClickBonus,
                onHideBonus = _ctx.levelChannel.onHideBonus,
            });
            _disposables.Add(bonusesPm);
        }
        
        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}