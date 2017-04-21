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
        public float interactLength = 1.0f;

        public LayerMask interactMask;

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
        /// Sets position of the character. Will completely reset functions list, virtual and target position too.
        /// </summary>
        /// <param name="_position"></param>
        public void SetPosition(Vector2 _position)
        {
            functions.Clear();
            transform.position = _position;
            virtualPosition = _position;
            targetPosition = _position;
        }

        public void ResetPosition()
        {
            var startTile = FindObjectOfType<WorldController>().worldStartPosition;
            SetPosition(new Vector2(startTile.x, startTile.y));
        }

        /// <summary>
        /// Called when the LUA interpreter is started.
        /// </summary>
        public void OnInterpreterStarted()
        {
            virtualPosition = transform.position;
        }

        /// <summary>
        /// Called when the LUA interpreter is terminated.
        /// </summary>
        public void OnInterpreterTerminated()
        {
            functions.Clear();
        }

        /// <summary>
        /// API call to try interact with an object to the left.
        /// </summary>
        public void InteractLeft()
        {
            functions.Add(() => InteractWithObjectAtDir(Vector2.left));
        }

        /// <summary>
        /// API call to try interact with an object to the right.
        /// </summary>
        public void InteractRight()
        {
            functions.Add(() => InteractWithObjectAtDir(Vector2.right));
        }

        /// <summary>
        /// API call to try interact with an object above.
        /// </summary>
        public void InteractUp()
        {
            functions.Add(() => InteractWithObjectAtDir(Vector2.up));
        }

        /// <summary>
        /// API call to try interact with an object bellow.
        /// </summary>
        public void InteractDown()
        {
            functions.Add(() => InteractWithObjectAtDir(Vector2.down));
        }

        /// <summary>
        /// Try to interact with an object in a given normalised direction if it has the interactable object component on it.
        /// </summary>
        /// <param name="_dir"></param>
        private void InteractWithObjectAtDir(Vector2 _dir)
        {
            var hit = Physics2D.Raycast(transform.position, _dir, interactLength, interactMask);

            if(hit)
            {
                var interactableObject = hit.collider.gameObject.GetComponent<InteractableObject>();

                if (interactableObject != null)
                {
                    if (interactableObject.onInteract != null)
                    {
                        interactableObject.onInteract.Invoke();
                    }
                }
            }
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

        public bool CanMove(string _direction)
        {
            var tileOn = World.Current.GetTileAtWorldCoord(virtualPosition);

            switch(_direction.ToLower())
            {
                case "left":
                    return CanMoveInDirection(Direction.Left, tileOn);

                case "right":
                    return CanMoveInDirection(Direction.Right, tileOn);

                case "up":
                    return CanMoveInDirection(Direction.Up, tileOn);

                case "down":
                    return CanMoveInDirection(Direction.Down, tileOn);
            }

            return false;
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
