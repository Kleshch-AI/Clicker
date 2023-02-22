using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Clicker.UI.Popups
{
    public class LevelResultsUI : BasePopupUI
    {
        [SerializeField] private TextMeshProUGUI results;
        
        // public async Task Show(bool isWin)
        // {
        //     results.text = isWin ? "You win!" : "You lose!";
        //     await AnimateShow();
        //     await Task.Delay(1500);
        //     Close();
        // }
        
        public async void Show(bool isWin)
        {
            results.text = isWin ? "You win!" : "You lose!";
            await AnimateShow();
        }

        public void Hide()
        {
            Close();
        }
    }
}