using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class ThanksScreenController : MonoBehaviour
    {

        public string questionnaireURL;

        public void OnQuestionnaireButtonPress()
        {
            Application.OpenURL(questionnaireURL);
        }

        public void OnMenuButtonPress()
        {
            SceneManager.LoadScene("menu");
        }

        public void OnQuitButtonPress()
        {
            Application.Quit();
        }
    }
}
