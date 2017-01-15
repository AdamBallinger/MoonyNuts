using System;
using System.Collections.Generic;
using Assets.Scripts.Game;
using UnityEngine;

namespace Assets.Scripts.API
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public class CharacterAPIController : MonoBehaviour
    {

        public float speed = 1f;

        public float stepDistance = 1f;

        private Vector2 targetPosition;
        private Vector2 virtualPosition;

        private List<Action> functions;

        public void Start()
        {
            functions = new List<Action>();
            CharacterAPI.AddObject(gameObject);

            targetPosition = transform.position;
            virtualPosition = transform.position;
        }

        public void Update()
        {
            if ((Vector2)transform.position != targetPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                GetComponent<SpriteRenderer>().flipX = targetPosition.x > transform.position.x;
            }
            else
            {
                if (functions.Count > 0)
                {
                    functions[0]();
                    functions.RemoveAt(0);
                }
                else
                {
                    // Resume the LUA coroutine if no functions left in the list.
                    LuaInterpreter.Current.Resume();
                }
            }
        }

        /// <summary>
        /// Called when the LUA interpreter is terminated.
        /// </summary>
        public void OnInterpreterTerminated()
        {
            functions.Clear();
            virtualPosition = transform.position;
        }

        /// <summary>
        /// API call to make the character speak.
        /// </summary>
        public void Speak()
        {
            // Lambda
            functions.Add(() => GetComponent<AudioSource>().Play());
        }

        /// <summary>
        /// API call to make the character move left by the specified step distance.
        /// </summary>
        public void MoveLeft()
        {
            var tileOn = World.Current.GetTileAtWorldCoord(virtualPosition);

            if(CanMoveInDirection(Direction.Left, tileOn))
            {
                virtualPosition.x -= stepDistance;
                functions.Add(() => targetPosition.x -= stepDistance);
            }
        }

        /// <summary>
        /// API call to make the character move right by the specified step distance.
        /// </summary>
        public void MoveRight()
        {
            var tileOn = World.Current.GetTileAtWorldCoord(virtualPosition);

            if (CanMoveInDirection(Direction.Right, tileOn))
            {
                virtualPosition.x += stepDistance;
                functions.Add(() => targetPosition.x += stepDistance);
            }
        }

        /// <summary>
        /// API call to make the character move up by the specified step distance.
        /// </summary>
        public void MoveUp()
        {
            var tileOn = World.Current.GetTileAtWorldCoord(virtualPosition);

            if (CanMoveInDirection(Direction.Up, tileOn))
            {
                virtualPosition.y += stepDistance;
                functions.Add(() => targetPosition.y += stepDistance);
            }
        }

        /// <summary>
        /// API call to make the character move down by the specified step distance.
        /// </summary>
        public void MoveDown()
        {
            var tileOn = World.Current.GetTileAtWorldCoord(virtualPosition);

            if (CanMoveInDirection(Direction.Down, tileOn))
            {
                virtualPosition.y -= stepDistance;
                functions.Add(() => targetPosition.y -= stepDistance);
            }
        }

        private bool CanMoveInDirection(Direction _direction, Tile _originTile)
        {
            switch (_direction)
            {
                case Direction.Left:
                    var tileLeft = World.Current.GetTileAt(_originTile.X - 1, _originTile.Y);
                    return tileLeft != null && tileLeft.Type == TileType.Empty;

                case Direction.Right:
                    var tileRight = World.Current.GetTileAt(_originTile.X + 1, _originTile.Y);
                    return tileRight != null && tileRight.Type == TileType.Empty;

                case Direction.Up:
                    var tileUp = World.Current.GetTileAt(_originTile.X, _originTile.Y + 1);
                    return tileUp != null && tileUp.Type == TileType.Empty;

                case Direction.Down:
                    var tileDown = World.Current.GetTileAt(_originTile.X, _originTile.Y - 1);
                    return tileDown != null && tileDown.Type == TileType.Empty;

                default:
                    return false;
            }
        }
    }
}
