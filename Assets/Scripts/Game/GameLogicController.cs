using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameLogicController : MonoBehaviour
    {

        public WorldController worldController;
        public GameObject player;

        private Tile worldStartTile;
        private Tile worldEndTile;

        private Tile playerCurrentTile;

        public void Start()
        {
            if(worldController == null || player == null)
            {
                Debug.LogError("[GameLogicController] - Player or world controller null.");
                return;
            }

            worldStartTile = World.Current.GetTileAtWorldCoord(worldController.worldStartPosition);
            worldEndTile = World.Current.GetTileAtWorldCoord(worldController.worldEndPosition);
        }

        public void Update()
        {
            playerCurrentTile = World.Current.GetTileAtWorldCoord(player.transform.position);

            if(playerCurrentTile == worldEndTile)
            {
                // TODO: Level complete.
                Debug.Log("End of level reached!");
                return;
            }
        }
    }
}
