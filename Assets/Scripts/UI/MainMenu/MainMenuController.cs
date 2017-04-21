using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {

        public void StartGame()
        {
            SceneManager.LoadScene("Level 1");
        }
    }
}
