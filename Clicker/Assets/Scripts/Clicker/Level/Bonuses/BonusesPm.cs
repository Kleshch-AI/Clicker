using System;
using System.Collections.Generic;
using System.Linq;
using Configuration;
using Reactive;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Clicker.Level.Bonuses
{
    public class BonusesPm : IDisposable
    {
        public struct Ctx
        {
            public IReadOnlyList<BonusSpawnInfo> bonuses;

            public ReactiveTrigger<Bonus, Vector2> onSpawnBonus;
            public ReactiveTrigger<Bonus> onHideBonus;
            public ReactiveTrigger<float> onChangeTargetSize;
            public ReactiveTrigger<int> onSetClickWeight;
            public ReactiveTrigger<bool> onLockTargetMove;
            public ReactiveTrigger<Bonus, bool> onBonusSetActive;

            public IReadOnlyReactiveProperty<int> secondsPassed;
            public IReadOnlyReactiveTrigger<Bonus> onClickBonus;
        }

        private readonly Ctx _ctx;
        private readonly CompositeDisposable _disposables;
        private IDisposable _timerDisposable;

        private Dictionary<Bonus, int> timersActivated;
        private Dictionary<Bonus, int> timersSpawned;

        public BonusesPm(Ctx ctx)
        {
            _ctx = ctx;

            timersActivated = new Dictionary<Bonus, int>();
            timersSpawned = new Dictionary<Bonus, int>();
            _ctx.bonuses.ForEach(b =>
            {
                timersActivated.Add(b.Type, 0);
                timersSpawned.Add(b.Type, 0);
            });

            _disposables = new CompositeDisposable();

            _ctx.secondsPassed.SkipLatestValueOnSubscribe().Subscribe(OnLevelTimerTick).AddTo(_disposables);
            _ctx.onClickBonus.Subscribe(OnClickBonus).AddTo(_disposables);
        }

        private void OnLevelTimerTick(int secondsPassed)
        {
            foreach (var info in _ctx.bonuses)
            {
                var type = info.Type;
                if (timersSpawned[type] > 0) // если бонус заспавнен, но не активирован
                {
                    timersSpawned[type]--;
                    if (timersSpawned[type] <= 0)
                        DespawnBonus(type);
                    continue;
                }
                
                if (timersActivated[type] > 0) // если бонус активирован
                {
                    timersActivated[type]--;
                    if (timersActivated[type] <= 0)
                        OnBonusEnd(type);
                    continue;
                }

                var rand = Random.value;
                if (rand <= info.Chance)
                    SpawnBonus(type);
            }
        }

        private void SpawnBonus(Bonus type)
        {
            timersSpawned[type] = 5;
            _ctx.onSpawnBonus.Notify(type, SpawnHelper.GetRandomNormalizedPoint());
        }

        private void DespawnBonus(Bonus type)
        {
            _ctx.onHideBonus.Notify(type);
        }

        private void OnClickBonus(Bonus type)
        {
            timersSpawned[type] = 0;
            var bonuses = _ctx.bonuses;
            timersActivated[type] = bonuses.ToList().Find(b => b.Type == type).Seconds;

            _ctx.onHideBonus.Notify(type);
            _ctx.onBonusSetActive.Notify(type, true);

            switch (type)
            {
                case Bonus.Size:
                    _ctx.onChangeTargetSize.Notify(2);
                    break;
                case Bonus.Tap:
                    _ctx.onSetClickWeight.Notify(2);
                    break;
                case Bonus.Stop:
                    _ctx.onLockTargetMove.Notify(true);
                    break;
            }
        }

        private void OnBonusEnd(Bonus type)
        {
            _ctx.onBonusSetActive.Notify(type, false);

            switch (type)
            {
                case Bonus.Size:
                    _ctx.onChangeTargetSize.Notify(0.5f);
                    break;
                case Bonus.Tap:
                    _ctx.onSetClickWeight.Notify(1);
                    break;
                case Bonus.Stop:
                    _ctx.onLockTargetMove.Notify(false);
                    break;
            }
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}