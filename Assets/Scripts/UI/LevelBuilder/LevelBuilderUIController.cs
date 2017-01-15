using Assets.Scripts.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.LevelBuilder
{
    public class LevelBuilderUIController : MonoBehaviour
    {

        public InputField worldNameInput;

        private WorldController worldController;
        private MouseController mouseController;

        public void Start()
        {
            worldController = FindObjectOfType<WorldController>();
            mouseController = FindObjectOfType<MouseController>();

            if (worldController == null)
            {
                Debug.LogError("[LevelBuilderUIController] - Failed to find world controller object.");
            }

            if (mouseController == null)
            {
                Debug.LogError("[LevelBuilderUIController] - Failed to find mouse controller object.");
            }

            Directories.Check();
        }

        public void OnBuildWallButtonPress()
        {
            if(mouseController != null)
            {
                mouseController.SelectMode = SelectionMode.Build;
                mouseController.BuildTileType = TileType.Wall;
            }
        }

        public void OnRemoveWallButtonPress()
        {
            if(mouseController != null)
            {
                mouseController.SelectMode = SelectionMode.Build;
                mouseController.BuildTileType = TileType.Empty;
            }
        }

        public void OnSaveLevelButtonPress()
        {
            if(worldController != null)
            {
                worldController.Save();
            }
        }

        public void OnLoadLevelButtonPress()
        {
            if(worldController != null)
            {
                worldController.Load(worldNameInput.text);
            }
        }
    }
}
