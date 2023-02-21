using System;
using Reactive;
using UniRx;
using UnityEngine;

namespace Clicker.Level
{
    public class ClickerLevelPm : IDisposable
    {
        public struct Ctx
        {
            public LevelInfo levelInfo;

            public ReactiveTrigger<Vector2> onSpawnTarget;
            public ReactiveProperty<int> secondsPassed;
            public ReactiveProperty<int> targetClicks;
            public ReactiveTrigger<bool> onLevelEnd;

            public IReadOnlyReactiveTrigger onTargetClick;
            public IReadOnlyReactiveTrigger onStartLevel;
            public IReadOnlyReactiveTrigger<int> onSetClickWeight;
            public IReadOnlyReactiveTrigger<bool> onLockTargetMove;
        }

        private readonly Ctx _ctx;

        private readonly CompositeDisposable _disposables;
        private IDisposable _timerDisposable;
        private bool _isLevelActive;
        private int _clickWeight = 1;
        private bool _isMoveLocked = false;

        public ClickerLevelPm(Ctx ctx)
        {
            _ctx = ctx;

            _disposables = new CompositeDisposable();

            _ctx.onTargetClick.Subscribe(OnTargetClick).AddTo(_disposables);
            _ctx.onStartLevel.Subscribe(StartLevel).AddTo(_disposables);
            _ctx.onSetClickWeight.Subscribe(SetClickWeight).AddTo(_disposables);
            _ctx.onLockTargetMove.Subscribe(LockTargetMove).AddTo(_disposables);
        }

        private void StartLevel()
        {
            _ctx.secondsPassed.Value = 0;
            _ctx.targetClicks.Value = 0;

            _isLevelActive = true;

            SpawnTarget();
            _timerDisposable = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x => { TickTimer(); });
        }

        private void SpawnTarget()
        {
            _ctx.onSpawnTarget.Notify(SpawnHelper.GetRandomNormalizedPoint());
        }

        private void OnTargetClick()
        {
            if (!_isLevelActive)
                return;

            _ctx.targetClicks.Value += _clickWeight;
            if (_ctx.targetClicks.Value >= _ctx.levelInfo.clicks)
            {
                OnWin();
                return;
            }

            if (!_isMoveLocked)
                SpawnTarget();
        }

        private void TickTimer()
        {
            _ctx.secondsPassed.Value++;
            if (_ctx.secondsPassed.Value >= _ctx.levelInfo.seconds)
                OnFail();
        }

        private void OnWin()
        {
            OnLevelEnd(true);
        }

        private void OnFail()
        {
            OnLevelEnd(false);
        }

        private void OnLevelEnd(bool isWin)
        {
            _timerDisposable?.Dispose();
            _isLevelActive = false;
            _ctx.onLevelEnd.Notify(isWin);
        }

        private void SetClickWeight(int value)
        {
            _clickWeight = value;
        }

        private void LockTargetMove(bool isLock)
        {
            _isMoveLocked = isLock;
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}