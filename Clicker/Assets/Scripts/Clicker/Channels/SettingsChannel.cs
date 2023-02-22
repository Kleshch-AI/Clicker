using UniRx;

namespace Clicker.Channels
{
    public class SettingsChannel
    {
        public ReactiveProperty<bool> music = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> sound = new ReactiveProperty<bool>();
    }
}