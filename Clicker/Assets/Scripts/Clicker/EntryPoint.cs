using UI;
using UnityEngine;

namespace Clicker
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;

        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = new GameManager(uiManager);
        }

        private void OnDisable()
        {
            _gameManager?.Dispose();
        }
    }
}
