using Clicker.Level.Bonuses;
using Reactive;
using UniRx;
using UnityEngine;

namespace Clicker.Channels
{
    public class LevelChannel
    {
        public ReactiveTrigger<Vector2> onSpawnTarget = new ReactiveTrigger<Vector2>();
        public ReactiveProperty<int> secondsPassed = new ReactiveProperty<int>();
        public ReactiveProperty<int> targetClicks = new ReactiveProperty<int>();
        public ReactiveTrigger onTargetClick = new ReactiveTrigger();
        public ReactiveTrigger onStartLevel = new ReactiveTrigger();
        public ReactiveTrigger<bool> onLevelEnd = new ReactiveTrigger<bool>();
        public ReactiveTrigger<Bonus, Vector2> onSpawnBonus = new ReactiveTrigger<Bonus, Vector2>();
        public ReactiveTrigger<Bonus> onClickBonus = new ReactiveTrigger<Bonus>();
        public ReactiveTrigger<float> onChangeTargetSize = new ReactiveTrigger<float>();
        public ReactiveTrigger<Bonus> onHideBonus = new ReactiveTrigger<Bonus>();
        public ReactiveTrigger<int> onSetClickWeight = new ReactiveTrigger<int>();
        public ReactiveTrigger<Bonus, bool> onBonusSetActive = new ReactiveTrigger<Bonus, bool>();
        public ReactiveTrigger<bool> onLockTargetMove = new ReactiveTrigger<bool>();
    }
}