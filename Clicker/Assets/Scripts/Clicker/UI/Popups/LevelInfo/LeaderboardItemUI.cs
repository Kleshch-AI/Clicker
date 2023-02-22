using TMPro;
using UnityEngine;

namespace Clicker.UI.Popups.LevelInfo
{
    public class LeaderboardItemUI : MonoBehaviour
    {
        public struct Ctx
        {
            public string name;
            public int score;
        }

        [SerializeField] private TextMeshProUGUI nickname;
        [SerializeField] private TextMeshProUGUI score;

        public void SetCtx(Ctx ctx)
        {
            nickname.text = ctx.name;
            score.text = ctx.score.ToString();
        }
    }
}