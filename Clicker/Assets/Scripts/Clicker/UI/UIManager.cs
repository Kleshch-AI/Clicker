using Clicker.Channels;
using Clicker.UI;
using Clicker.UI.Level;
using UniRx;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public struct Ctx
        {
            public LevelUI.Ctx levelUICtx;
            public UIChannel uiChannel;
        }
        
        [SerializeField] private LevelUI levelUI;
        [SerializeField] private SettingsUI settingsUIPrefab;

        public void SetCtx(Ctx ctx)
        {
            levelUI.SetCtx(ctx.levelUICtx);

            ctx.uiChannel.onShowSettingsUI.Subscribe(ShowSettings).AddTo(this);
        }

        private void ShowSettings()
        {
            var settings = Instantiate(settingsUIPrefab, transform);
            settings.SetCtx(new SettingsUI.Ctx());
            settings.Show();
        }
    }
}