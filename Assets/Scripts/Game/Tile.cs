
using System;

namespace Assets.Scripts.Game
{
    public enum TileType
    {
        Empty,
        Wall
    }

    public class Tile
    {

        public int X { get; private set; }
        public int Y { get; private set; }

        private Action<Tile> typeChangeCallback;

        private TileType type;
        private TileType oldType;
        public TileType Type
        {
            get { return type; }
            set
            {
                oldType = type;
                type = value;
                if (typeChangeCallback != null && oldType != type)
                    typeChangeCallback(this);
            }
        }

        public Tile(int _x, int _y, TileType _type = TileType.Empty)
        {
            X = _x;
            Y = _y;
            Type = _type;
            oldType = Type;
        }

        /// <summary>
        /// Registers a callback function to the tile that gets called when the type is changed.
        /// </summary>
        /// <param name="_callback"></param>
        public void RegisterTileTypeChangeCallback(Action<Tile> _callback)
        {
            typeChangeCallback += _callback;
        }
    }
}
