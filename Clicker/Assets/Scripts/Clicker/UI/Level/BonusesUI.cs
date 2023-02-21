using Clicker.Level;
using Clicker.Level.Bonuses;
using Reactive;
using UniRx;
using UnityEngine;

namespace Clicker.UI.Level
{
    public class BonusesUI : MonoBehaviour
    {
        public struct Ctx
        {
            public Vector2 gameFieldSize { get; private set; }

            public ReactiveTrigger<Bonus> onClickBonus;

            public IReadOnlyReactiveTrigger<Bonus, Vector2> onSpawnBonus;
            public IReadOnlyReactiveTrigger<Bonus> onHideBonus;
            public IReadOnlyReactiveTrigger<bool> onLevelEnd;
            public IReadOnlyReactiveTrigger<Bonus, bool> onBonusSetActive;

            public void SetGameFieldSize(Vector2 value)
            {
                gameFieldSize = value;
            }
        }

        [SerializeField] private BonusUI bonusSize;
        [SerializeField] private BonusUI bonusStop;
        [SerializeField] private BonusUI bonusTap;
        
        [SerializeField] private GameObject bonusSizeIcon;
        [SerializeField] private GameObject bonusStopIcon;
        [SerializeField] private GameObject bonusTapIcon;

        private Ctx _ctx;

        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;

            bonusSize.SetCtx(new BonusUI.Ctx
            {
                type = Bonus.Size,
                onClickBonus = _ctx.onClickBonus,
            });

            bonusTap.SetCtx(new BonusUI.Ctx
            {
                type = Bonus.Tap,
                onClickBonus = _ctx.onClickBonus,
            });
            
            bonusStop.SetCtx(new BonusUI.Ctx
            {
                type = Bonus.Stop,
                onClickBonus = _ctx.onClickBonus,
            });

            ResetView();

            _ctx.onBonusSetActive.Subscribe(BonusSetActive).AddTo(this);
            _ctx.onSpawnBonus.Subscribe(SpawnBonus).AddTo(this);
            _ctx.onHideBonus.Subscribe(HideBonus).AddTo(this);
            _ctx.onLevelEnd.Subscribe(_ => ResetView()).AddTo(this);
        }

        private void SpawnBonus(Bonus type, Vector2 factor)
        {
            var bonus = GetBonusByType(type);
            var bonusRt = bonus.transform as RectTransform;
            bonus.Respawn(SpawnHelper.CalculateSpawnCoordinates(_ctx.gameFieldSize, factor, bonusRt.rect.size));
            bonus.gameObject.SetActive(true);
        }

        private void HideBonus(Bonus type)
        {
            var bonus = GetBonusByType(type);
            bonus.gameObject.SetActive(false);
        }

        private void ResetView()
        {
            bonusSize.gameObject.SetActive(false);
            bonusStop.gameObject.SetActive(false);
            bonusTap.gameObject.SetActive(false);
            bonusSizeIcon.gameObject.SetActive(false);
            bonusStopIcon.gameObject.SetActive(false);
            bonusTapIcon.gameObject.SetActive(false);
        }

        private void BonusSetActive(Bonus type, bool isActive)
        {
            switch (type)
            {
                case Bonus.Size:
                   bonusSizeIcon.SetActive(isActive);
                   break;
                case Bonus.Stop:
                    bonusStopIcon.SetActive(isActive);
                    break;
                case Bonus.Tap:
                    bonusTapIcon.SetActive(isActive);
                    break;
            }
        }

        private ClickUI GetBonusByType(Bonus type)
        {
            switch (type)
            {
                case Bonus.Size:
                    return bonusSize;
                case Bonus.Stop:
                    return bonusStop;
                case Bonus.Tap:
                    return bonusTap;
            }

            return null;
        }
    }
}