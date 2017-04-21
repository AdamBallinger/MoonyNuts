using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TutorialBoxController : MonoBehaviour
    {
        public InputField luaInput;
        public Button[] luaButtons;

        public Text tutorialText;
        public Text pageNumText;

        public Button nextButton;
        public Button prevButton;

        [TextArea(12, 30)]
        public List<string> pages = new List<string>();

        private int currentPage;

        private void OnEnable()
        {
            luaInput.interactable = false;
            luaButtons[0].interactable = false;
            luaButtons[1].interactable = false;

            currentPage = 0;
            OnPageChange();
        }

        private void Update()
        {
            pageNumText.text = "Page " + (currentPage + 1) + " of " + pages.Count;

            prevButton.interactable = currentPage > 0;
            nextButton.interactable = currentPage < pages.Count - 1;
        }

        public void NextPage()
        {
            currentPage++;
            if (currentPage > pages.Count - 1) currentPage = pages.Count - 1;

            OnPageChange();
        }

        public void PrevPage()
        {
            currentPage--;
            if (currentPage < 0) currentPage = 0;

            OnPageChange();
        }

        public void Done()
        {
            luaInput.interactable = true;
            luaButtons[0].interactable = true;
            luaButtons[1].interactable = true;

            gameObject.SetActive(false);
        }

        private void OnPageChange()
        {
            tutorialText.text = pages[currentPage];
        }
    }
}
