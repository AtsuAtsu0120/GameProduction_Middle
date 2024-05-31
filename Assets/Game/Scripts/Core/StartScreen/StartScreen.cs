using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Core.StartScreen
{
    public class StartScreen : MonoBehaviour
    {
        private void Update()
        {
            TapToStart();
        }

        private void TapToStart()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SceneManager.LoadScene("MainGame");
            }
        }
    }
}
