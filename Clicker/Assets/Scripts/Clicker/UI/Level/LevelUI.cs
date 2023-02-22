using System.Threading.Tasks;
using Clicker.Level;
using Reactive;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.UI.Level
{
    public class LevelUI : MonoBehaviour
    {
        public struct Ctx
        {
            public int clicks;
            public int seconds;
            public Sprite bg;

            public TargetUI.Ctx targetCtx;
            public BonusesUI.Ctx bonusesCtx;

            public ReactiveTrigger onShowSettingsUI;
            public ReactiveProperty<int> secondsPassed;
            public ReactiveProperty<int> targetClicks;
            public ReactiveTrigger<bool, Return<Task>> onShowLevelResults;
            public IReadOnlyReactiveTrigger onStartLevel;
            public IReadOnlyReactiveTrigger<Vector2> onSpawnTarget;
            public IReadOnlyReactiveTrigger<bool> onLevelEnd;
        }

        [SerializeField] private Image bg;
        [SerializeField] private Button settings;
        [SerializeField] private Slider clicksStatSlider;
        [SerializeField] private TextMeshProUGUI clicksStat;
        [SerializeField] private TextMeshProUGUI clicksInfo;
        [SerializeField] private TextMeshProUGUI timerStat;
        [SerializeField] private TargetUI target;
        [SerializeField] private RectTransform gameField;
        [SerializeField] private BonusesUI bonuses;

        private Ctx _ctx;
        private RectTransform targetRt;

        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;
            
            _ctx.bonusesCtx.SetGameFieldSize(gameField.rect.size);
            bonuses.SetCtx(_ctx.bonusesCtx);

            target.SetCtx(_ctx.targetCtx);

            targetRt = target.transform as RectTransform;
            target.gameObject.SetActive(false);

            bg.sprite = _ctx.bg;

            settings.onClick.AddListener(() => _ctx.onShowSettingsUI.Notify());

            _ctx.targetClicks.Subscribe(UpdateClicks).AddTo(this);
            _ctx.secondsPassed.Subscribe(UpdateTimer).AddTo(this);
            _ctx.onSpawnTarget.Subscribe(SpawnTarget).AddTo(this);
            _ctx.onLevelEnd.Subscribe(OnLevelEnd).AddTo(this);
            _ctx.onStartLevel.Subscribe(OnLevelStart).AddTo(this);
        }

        private void OnLevelStart()
        {
            target.gameObject.SetActive(true);
            clicksStatSlider.maxValue = _ctx.clicks;
            clicksInfo.text = _ctx.clicks.ToString();
            timerStat.text = _ctx.seconds.ToString();
        }

        private void UpdateClicks(int clicks)
        {
            clicksStat.text = $"{clicks}/{_ctx.clicks}";
            clicksStatSlider.value = clicks;
        }

        private void UpdateTimer(int secondsPassed)
        {
            timerStat.text = $"{_ctx.seconds - secondsPassed}";
        }

        private void SpawnTarget(Vector2 factor)
        {
            target.Respawn(SpawnHelper.CalculateSpawnCoordinates(gameField.rect.size, factor, targetRt.rect.size));
        }

        private async void OnLevelEnd(bool isWin)
        {
            target.gameObject.SetActive(false);
            var ret = new Return<Task>();
            _ctx.onShowLevelResults.Notify(isWin, ret);
            await ret.Value;
            Destroy(gameObject);
        }
    }
}