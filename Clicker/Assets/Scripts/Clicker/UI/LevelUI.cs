using System.Threading.Tasks;
using Clicker.Level;
using Reactive;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.UI
{
    public class LevelUI : MonoBehaviour
    {
        public struct Ctx
        {
            public LevelInfo levelInfo;

            public ReactiveTrigger onStartLevel;
            public ReactiveProperty<int> secondsPassed;
            public ReactiveProperty<int> targetClicks;
            public TargetUI.Ctx targetCtx;
            public IReadOnlyReactiveTrigger<Vector2> onSpawnTarget;
            public IReadOnlyReactiveTrigger<bool> onLevelEnd;
            public BonusesUI.Ctx bonusesCtx;
        }

        [SerializeField] private Button start;
        [SerializeField] private Slider clicksStatSlider;
        [SerializeField] private TextMeshProUGUI clicksStat;
        [SerializeField] private TextMeshProUGUI clicksInfo;
        [SerializeField] private TextMeshProUGUI timerStat;
        [SerializeField] private TextMeshProUGUI resultText;
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
            start.gameObject.SetActive(true);
            resultText.gameObject.SetActive(false);

            _ctx.targetClicks.Subscribe(UpdateClicks).AddTo(this);
            _ctx.secondsPassed.Subscribe(UpdateTimer).AddTo(this);
            _ctx.onSpawnTarget.Subscribe(SpawnTarget).AddTo(this);
            _ctx.onLevelEnd.Subscribe(OnLevelEnd).AddTo(this);

            start.onClick.AddListener(OnLevelStart);
        }

        private void OnLevelStart()
        {
            target.gameObject.SetActive(true);
            start.gameObject.SetActive(false);
            resultText.gameObject.SetActive(false);
            clicksStatSlider.maxValue = _ctx.levelInfo.clicks;
            clicksInfo.text = _ctx.levelInfo.clicks.ToString();
            timerStat.text = _ctx.levelInfo.seconds.ToString();
            _ctx.onStartLevel.Notify();
        }

        private void UpdateClicks(int clicks)
        {
            clicksStat.text = $"{clicks}/{_ctx.levelInfo.clicks}";
            clicksStatSlider.value = clicks;
        }

        private void UpdateTimer(int secondsPassed)
        {
            timerStat.text = $"{_ctx.levelInfo.seconds - secondsPassed}";
        }

        private void SpawnTarget(Vector2 factor)
        {
            target.Respawn(SpawnHelper.CalculateSpawnCoordinates(gameField.rect.size, factor, targetRt.rect.size));
        }

        private async void OnLevelEnd(bool isWin)
        {
            target.gameObject.SetActive(false);
            resultText.text = isWin ? "You win!" : "You lose!";
            resultText.gameObject.SetActive(true);
            await Task.Delay(1500);
            resultText.gameObject.SetActive(false);
            start.gameObject.SetActive(true);
        }
    }
}