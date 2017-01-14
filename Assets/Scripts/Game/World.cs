
using Boo.Lang;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public sealed class World
    {

        public static World Current { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        private Tile[,] Tiles { get; set; }

        private World() { }

        /// <summary>
        /// Creates a new instance of world.
        /// </summary>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        public static void Create(int _width = 16, int _height = 16)
        {
            Current = new World();
            Current.Width = _width;
            Current.Height = _height;
            Current.Init();
        }

        /// <summary>
        /// Initialises the world by creating the tiles for the grid.
        /// </summary>
        private void Init()
        {
            Current.Tiles = new Tile[Current.Width, Current.Height];

            for(var x = 0; x < Current.Width; x++)
            {
                for(var y = 0; y < Current.Height; y++)
                {
                    Current.Tiles[x, y] = new Tile(x, y);
                }
            }
        }

        /// <summary>
        /// Clears the current world by setting each tile to an empty tile.
        /// </summary>
        public void Clear()
        {
            for(var x = 0; x < Current.Width; x++)
            {
                for(var y = 0; y < Current.Height; y++)
                {
                    Current.Tiles[x, y].Type = TileType.Empty;
                }
            }
        }

        /// <summary>
        /// Returns number of tiles for the current world, including empty tiles.
        /// </summary>
        /// <returns></returns>
        public int GetTileCount()
        {
            return Current.Width * Current.Height;
        }

        /// <summary>
        /// Gets a tile at the given grid X and Y.
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <returns></returns>
        public Tile GetTileAt(int _x, int _y)
        {
            if(_x < 0 || _x > Current.Width || _y < 0 || _y > Current.Height)
            {
                return null;
            }

            return Tiles[_x, _y];
        }

        /// <summary>
        /// Gets the tile  in the world for the given unity world coordinate.
        /// </summary>
        /// <param name="_coord"></param>
        /// <returns></returns>
        public Tile GetTileAtWorldCoord(Vector2 _coord)
        {
            var coord = WorldPointToGridPoint(_coord);
            return GetTileAt((int)coord.x, (int)coord.y);
        }

        /// <summary>
        /// Converts a given unity world position to a position in the world grid.
        /// </summary>
        /// <param name="_coord"></param>
        /// <returns></returns>
        public Vector2 WorldPointToGridPoint(Vector2 _coord)
        {
            // offset by 0.5f as pivot points for gameobjects are in the center.
            var x = Mathf.FloorToInt(_coord.x + 0.5f);
            var y = Mathf.FloorToInt(_coord.y + 0.5f);
            return new Vector2(x, y);
        }
    }
}
