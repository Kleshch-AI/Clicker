using Clicker.Channels;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.UI.Popups
{
    public class SettingsUI : BasePopupUI
    {
        public struct Ctx
        {
            public SettingsChannel settingsChannel;
        }

        [SerializeField] private Toggle music;
        [SerializeField] private Toggle sound;

        private Ctx _ctx;
        
        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;

            music.isOn = _ctx.settingsChannel.music.Value;
            sound.isOn = _ctx.settingsChannel.sound.Value;
            
            music.onValueChanged.AddListener(OnMusicToggle);
            sound.onValueChanged.AddListener(OnSoundToggle);
        }
        
        public async void Show()
        {
            await AnimateShow();
        }

        private void OnMusicToggle(bool isOn)
        {
            _ctx.settingsChannel.music.Value = isOn;
        }
        
        private void OnSoundToggle(bool isOn)
        {
            _ctx.settingsChannel.sound.Value = isOn;
        }
    }
}