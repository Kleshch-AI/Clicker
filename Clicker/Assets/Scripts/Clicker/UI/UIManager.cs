using Clicker.UI;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public struct Ctx
        {
            public LevelUI.Ctx levelUICtx;
        }
        
        [SerializeField] private LevelUI levelUI;

        public void SetCtx(Ctx ctx)
        {
            levelUI.SetCtx(ctx.levelUICtx);
        }
    }
}