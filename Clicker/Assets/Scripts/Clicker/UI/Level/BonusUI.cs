using Clicker.Level.Bonuses;
using Reactive;

namespace Clicker.UI.Level
{
    public class BonusUI : ClickUI
    {
        public struct Ctx
        {
            public Bonus type;
            public ReactiveTrigger<Bonus> onClickBonus;
        }
        
        private Ctx _ctx;

        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;
        }

        protected override void OnClick()
        {
            _ctx.onClickBonus.Notify(_ctx.type);
        }
    }
}
