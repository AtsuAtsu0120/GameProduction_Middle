using Game.Scripts.Core.MainGame;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Core.ClearScreen
{
    public class ClearManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _resultText;

        private const string ClearMessage = "Clear";
        private const string OverMessage = "Faild";
        private void Awake()
        {
            ShowResult();
        }

        private void ShowResult()
        {
            _resultText.SetText(GameInfo.IsClear ? ClearMessage : OverMessage);
        }

        public void GoBackStartScreen()
        {
            SceneManager.LoadScene("StartScreen");
        }
    }
}