using Assets.Scripts.Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public enum SelectionMode
    {
        None, // Selection does nothing.
        Build // Handle building and clearing tiles.
    }

    public class MouseController : MonoBehaviour
    {
        public GameObject mouseCursor;

        public float zoomSpeed = 1.0f;
        public float maxZoomIn = 1.0f;
        public float maxZoomOut = 5.0f;

        public SelectionMode SelectMode { get; set; }

        private Vector2 currentMousePosition;
        private Vector2 lastMousePosition;

        private Vector2 mouseDragStartPosition;
        private bool mouseDragging = false;

        public void Start()
        {
            SelectMode = SelectionMode.None;
        }

        public void Update()
        {
            currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            HandleCursor();
            HandleMouseDrag();
            HandleCameraMovement();
            HandleZooming();

            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void HandleCursor()
        {
            if (mouseDragging) return;

            var tileHovered = World.Current.GetTileAtWorldCoord(currentMousePosition);
            if(tileHovered != null)
            {
                mouseCursor.SetActive(true);
                mouseCursor.transform.position = new Vector2(tileHovered.X, tileHovered.Y);
            }
            else
            {
                mouseCursor.SetActive(false);
            }
        }

        private void HandleMouseDrag()
        {
            var isMouseOnUI = EventSystem.current.IsPointerOverGameObject();

            if(Input.GetMouseButtonDown(0) && !isMouseOnUI)
            {
                mouseDragging = true;
                mouseDragStartPosition = currentMousePosition;
            }

            if(mouseDragging)
            {
                // Add 0.5f to compensate for Unity gameobjects pivot points being the center of the object.
                var startX = Mathf.FloorToInt(mouseDragStartPosition.x + 0.5f);
                var endX = Mathf.FloorToInt(currentMousePosition.x + 0.5f);
                var startY = Mathf.FloorToInt(mouseDragStartPosition.y + 0.5f);
                var endY = Mathf.FloorToInt(currentMousePosition.y + 0.5f);

                // Flip if dragging mouse left because endX would be less than startX and the for loop wouldnt loop
                if (endX < startX)
                {
                    var tmp = endX;
                    endX = startX;
                    startX = tmp;
                }

                // Same for Y if the mouse is dragged down.
                if (endY < startY)
                {
                    var tmp = endY;
                    endY = startY;
                    startY = tmp;
                }

                // If left mouse button is being held down display the drag area by resizing the select cursor over dragged area.
                if (Input.GetMouseButton(0) && !isMouseOnUI)
                {
                    // Because the world/tile map start at 0,0 we need to add 1 to the drag dimensions so that the cursor is the correct size.
                    var dragWidth = endX - startX + 1f;
                    var dragHeight = endY - startY + 1f;

                    mouseCursor.transform.localScale = new Vector2(dragWidth, dragHeight);

                    var newCursorPosition = new Vector2(startX + dragWidth / 2 - 0.5f, startY + dragHeight / 2 - 0.5f);
                    mouseCursor.transform.position = newCursorPosition;
                }

                // End mouse drag.
                if (Input.GetMouseButtonUp(0) && !isMouseOnUI)
                {
                    mouseDragging = false;
                    // Reset size to 1 tile.
                    mouseCursor.transform.localScale = Vector2.one;

                    for (var x = startX; x <= endX; x++)
                    {
                        for (var y = startY; y <= endY; y++)
                        {
                            var tile = World.Current.GetTileAt(x, y);

                            if (tile != null)
                            {
                                ProcessTileSelected(tile);
                            }
                        }
                    }

                    World.Current.OnWorldModifyFinishCallback();
                }
            }
        }

        private void HandleCameraMovement()
        {
            // If the middle or right mouse buttons are being held down the move camera with the mouse.
            if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
            {
                var difference = lastMousePosition - currentMousePosition;
                Camera.main.transform.Translate(difference);
            }
        }

        private void HandleZooming()
        {
            Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, maxZoomIn, maxZoomOut);
        }

        private void ProcessTileSelected(Tile _tile)
        {
            
        }
    }
}
