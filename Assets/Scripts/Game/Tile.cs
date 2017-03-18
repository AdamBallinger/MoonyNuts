using System;

namespace Assets.Scripts.Game
{
    public enum TileType
    {
        Nothing,
        Empty,
        Wall
    }

    [Flags]
    public enum AdjacentFlag
    {
        None = 1,
        Left = 1 << 1,
        Right = 1 << 2,
        Up = 1 << 3,
        Down = 1 << 4,

        All = Left | Right | Up | Down,
        LeftRight = Left | Right,
        UpDown = Up | Down,
        UpLeft = Up | Left,
        UpRight = Up | Right,
        DownLeft = Down | Left,
        DownRight = Down | Right,
        UpLeftDown = UpLeft | Down,
        UpRightDown = UpRight | Down,
        DownLeftRight = Down | LeftRight,
        UpLeftRight = Up | LeftRight
    }

    public class Tile
    {

        public int X { get; private set; }
        public int Y { get; private set; }

        private AdjacentFlag Adjacent { get; set; }

        private Action<Tile> typeChangeCallback;

        private TileType type;
        private TileType oldType = TileType.Nothing;
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

        public Tile(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }

        /// <summary>
        /// Registers a callback function to the tile that gets called when the type is changed.
        /// </summary>
        /// <param name="_callback"></param>
        public void RegisterTileTypeChangeCallback(Action<Tile> _callback)
        {
            typeChangeCallback += _callback;
        }

        public static TileType GetTypeFromString(string _typeName)
        {
            switch (_typeName)
            {
                case "Empty":
                    return TileType.Empty;
                case "Wall":
                    return TileType.Wall;

                default:
                    return TileType.Nothing;
            }
        }

        public bool HasAdjacentFlags(AdjacentFlag _flag)
        {
            return (Adjacent & _flag) == _flag;
        }

        public void SetAdjacent(AdjacentFlag _flag)
        {
            Adjacent = _flag;
        }

        public void AddAdjacentFlag(AdjacentFlag _flag)
        {
            Adjacent &= ~AdjacentFlag.None;
            Adjacent |= _flag;
        }

        public void CheckAdjacent(Tile _tile, AdjacentFlag _direction)
        {
            if (_tile == null || _tile.Type == TileType.Empty)
            {
                AddAdjacentFlag(_direction);
            }
        }
    }
}
