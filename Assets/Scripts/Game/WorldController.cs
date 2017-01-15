using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
    public class WorldController : MonoBehaviour
    {

        public Sprite gridSprite;

        public Sprite[] wallSprites;

        [SerializeField]
        private int worldWidth = 16;
        [SerializeField]
        private int worldHeight = 16;
        [SerializeField]
        private string worldName = "DefaultWorldName";

        private GameObject[,] tileGameObjects;

        public void Start()
        {
            World.Create(worldName, worldWidth, worldHeight);
            World.Current.RegisterWorldModifyCallback(OnWorldChangeFinish);

            tileGameObjects = new GameObject[worldWidth, worldHeight];

            for (var x = 0; x < worldWidth; x++)
            {
                for (var y = 0; y < worldHeight; y++)
                {
                    var tileGO = new GameObject();
                    tileGO.transform.SetParent(transform);
                    tileGO.name = string.Format("Tile:  X:{0}  Y:{1}", x, y);
                    tileGO.tag = "Tile";

                    var tileData = World.Current.GetTileAt(x, y);
                    tileData.RegisterTileTypeChangeCallback(tile => { OnTileTypeChanged(tileGO, tile); });

                    tileGO.AddComponent<SpriteRenderer>();
                    tileGO.transform.position = new Vector2(tileData.X, tileData.Y);
                    tileData.Type = TileType.Empty;

                    tileGameObjects[x, y] = tileGO;
                }
            }

            World.Current.SetBorderAsWalls();
        }

        public void OnTileTypeChanged(GameObject _tileGO, Tile _tileData)
        {
            switch (_tileData.Type)
            {
                case TileType.Empty:
                    _tileGO.GetComponent<SpriteRenderer>().sprite = SceneManager.GetActiveScene().name == "level_builder" ? gridSprite : null;
                    break;
            }
        }

        public void OnWorldChangeFinish()
        {
            for (var x = 0; x < worldWidth; x++)
            {
                for (var y = 0; y < worldHeight; y++)
                {
                    var tile = World.Current.GetTileAt(x, y);

                    switch (tile.Type)
                    {
                        case TileType.Nothing:
                        case TileType.Empty:
                            continue;

                        case TileType.Wall:
                            var tileLeft = World.Current.GetTileAt(x - 1, y);
                            var tileRight = World.Current.GetTileAt(x + 1, y);
                            var tileUp = World.Current.GetTileAt(x, y + 1);
                            var tileDown = World.Current.GetTileAt(x, y - 1);

                            // Set sprite based on surrounding tile types.
                            SetWallSpritesFromAdjacent(tile, tileRight, tileLeft, tileUp, tileDown);
                            break;
                    }
                }
            }
        }

        private void SetWallSpritesFromAdjacent(Tile _tile, Tile _tileRight, Tile _tileLeft, Tile _tileUp, Tile _tileDown)
        {
            _tile.SetAdjacent(AdjacentFlag.None);

            // Compute the adjacent flags.
            _tile.CheckAdjacent(_tileLeft, AdjacentFlag.Left);
            _tile.CheckAdjacent(_tileRight, AdjacentFlag.Right);
            _tile.CheckAdjacent(_tileUp, AdjacentFlag.Up);
            _tile.CheckAdjacent(_tileDown, AdjacentFlag.Down);

            var spriteIndex = 0;

            // Bitmasking method as found here: http://www.angryfishstudios.com/2011/04/adventures-in-bitmasking/ 
            if (!_tile.HasAdjacentFlags(AdjacentFlag.Left))
            {
                spriteIndex += 8;
            }

            if (!_tile.HasAdjacentFlags(AdjacentFlag.Right))
            {
                spriteIndex += 2;
            }

            if (!_tile.HasAdjacentFlags(AdjacentFlag.Up))
            {
                spriteIndex += 1;
            }

            if (!_tile.HasAdjacentFlags(AdjacentFlag.Down))
            {
                spriteIndex += 4;
            }

            tileGameObjects[_tile.X, _tile.Y].GetComponent<SpriteRenderer>().sprite = wallSprites[spriteIndex];
        }

        public void Clear()
        {
            World.Current.Clear();
        }

        public void Save()
        {
            World.Current.Save();
        }

        public void Load(string _worldName)
        {
            World.Current.Load(_worldName);
        }
    }
}
