using Assets.Scripts.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.LevelBuilder
{
    public class LevelBuilderUIController : MonoBehaviour
    {

        public InputField worldSaveLevelInput;
        public InputField worldLoadLevelInput;

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

            worldSaveLevelInput.text = World.Current.WorldName;
            worldLoadLevelInput.text = World.Current.WorldName;
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

        public void OnSetStartButtonPress()
        {
            mouseController.SelectMode = SelectionMode.SetStart;
        }

        public void OnSetEndButtonPress()
        {
            mouseController.SelectMode = SelectionMode.SetEnd;
        }

        public void OnSaveLevelButtonPress()
        {
            if(worldController != null)
            {
                World.Current.WorldName = worldSaveLevelInput.text;
                worldController.Save();
            }
        }

        public void OnLoadLevelButtonPress()
        {
            if(worldController != null)
            {
                worldController.Load(worldLoadLevelInput.text);
            }
        }
    }
}
