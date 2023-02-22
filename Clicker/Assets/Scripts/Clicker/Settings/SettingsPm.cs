using System;
using Clicker.Channels;
using UniRx;
using UnityEngine;

namespace Clicker.Settings
{
    public class SettingsPm : IDisposable
    {
        public struct Ctx
        {
            public SettingsChannel settingsChannel;
        }

        private const string MUSIC_KEY = "music";
        private const string SOUND_KEY = "sound";

        private Ctx _ctx;
        private CompositeDisposable _disposables;
        
        public SettingsPm(Ctx ctx)
        {
            _ctx = ctx;

            _disposables = new CompositeDisposable();

            _ctx.settingsChannel.music.Value = GetSettings(MUSIC_KEY);
            _ctx.settingsChannel.sound.Value = GetSettings(SOUND_KEY);

            _ctx.settingsChannel.music.Subscribe(OnMusicToggle).AddTo(_disposables);
            _ctx.settingsChannel.sound.Subscribe(OnSoundToggle).AddTo(_disposables);
        }

        private void OnMusicToggle(bool isOn)
        {
            // todo 
            SaveSetting(MUSIC_KEY, isOn);
        }
        
        private void OnSoundToggle(bool isOn)
        {
            // todo 
            SaveSetting(SOUND_KEY, isOn);
        }

        private void SaveSetting(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        private bool GetSettings(string key)
        {
            return PlayerPrefs.GetInt(key) > 0;
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}