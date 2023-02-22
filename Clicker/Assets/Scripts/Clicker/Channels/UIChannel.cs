using System.Threading.Tasks;
using Clicker.UI.Popups.LevelInfo;
using Configuration;
using Reactive;

namespace Clicker.Channels
{
    public class UIChannel
    {
        public ReactiveTrigger onShowSettingsUI = new ReactiveTrigger();
        public ReactiveTrigger<LevelInfoUI.Ctx> onShowLevelInfoUI = new ReactiveTrigger<LevelInfoUI.Ctx>();
        public ReactiveTrigger<bool, Return<Task>> onShowLevelResults = new ReactiveTrigger<bool, Return<Task>>();

        /// <param name="1. int">level id</param>
        public ReactiveTrigger<int> onClickStartLevel = new ReactiveTrigger<int>();

        public ReactiveTrigger<LevelInfo> onShowLevelUI = new ReactiveTrigger<LevelInfo>();
    }
}