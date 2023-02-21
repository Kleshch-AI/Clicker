using Reactive;
using UniRx;
using UnityEngine;

namespace Clicker.UI.Level
{
    public class TargetUI : ClickUI
    {
        public class Ctx
        {
            public ReactiveTrigger onTargetClick;
            
            public IReadOnlyReactiveTrigger<float> onChangeTargetSize;
        }
        
        private Ctx _ctx;

        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;

            _ctx.onChangeTargetSize.Subscribe(ChangeSize).AddTo(this);
        }

        protected override void OnClick()
        {
            _ctx.onTargetClick.Notify();
        }

        private void ChangeSize(float factor)
        {
            Debug.Log(factor);
            _rt.sizeDelta = new Vector2(_rt.sizeDelta.x * factor, _rt.sizeDelta.y * factor);
        }
    }
}