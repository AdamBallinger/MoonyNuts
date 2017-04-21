using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
    public class WorldController : MonoBehaviour
    {

        public Sprite gridSprite;

        public Sprite[] startEndSprites;
        public Sprite[] wallSprites;

        public Vector2 worldStartPosition = Vector2.one;
        public Vector2 worldEndPosition = Vector2.one;

        [SerializeField]
        private int worldWidth = 16;
        [SerializeField]
        private int worldHeight = 16;
        [SerializeField]
        private string worldName = "DefaultWorldName";
        [SerializeField]
        private bool loadLevelOnStart = false;

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

                    var tileSR = tileGO.AddComponent<SpriteRenderer>();
                    tileSR.sortingOrder = -1;
                    tileGO.transform.position = new Vector2(tileData.X, tileData.Y);
                    tileData.Type = TileType.Empty;

                    tileGameObjects[x, y] = tileGO;
                }
            }

            World.Current.SetBorderAsWalls();

            if (loadLevelOnStart)
                Load(worldName);
        }

        public void OnTileTypeChanged(GameObject _tileGO, Tile _tileData)
        {
            switch (_tileData.Type)
            {
                case TileType.Empty:
                    _tileGO.GetComponent<SpriteRenderer>().sprite = SceneManager.GetActiveScene().name == "level_builder" ? gridSprite : null;
                    break;
                case TileType.Start:
                    _tileGO.GetComponent<SpriteRenderer>().sprite = startEndSprites[0];
                    break;
                case TileType.End:
                    _tileGO.GetComponent<SpriteRenderer>().sprite = startEndSprites[1];
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
            var xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "    ";
            xmlSettings.NewLineOnAttributes = false;

            var saveFile = Path.Combine(Directories.Save_Directory, World.Current.WorldName + ".xml");

            Debug.Log("Saving: " + saveFile);

            using (var xmlWriter = XmlWriter.Create(saveFile, xmlSettings))
            {
                xmlWriter.WriteStartDocument();

                xmlWriter.WriteStartElement("LevelSaveFile");
                xmlWriter.WriteStartElement("LevelData");

                xmlWriter.WriteAttributeString("WorldName", worldName);
                xmlWriter.WriteAttributeString("WorldWidth", World.Current.Width.ToString());
                xmlWriter.WriteAttributeString("WorldHeight", World.Current.Height.ToString());
                xmlWriter.WriteAttributeString("WorldStartPositionX", worldStartPosition.x.ToString());
                xmlWriter.WriteAttributeString("WorldStartPositionY", worldStartPosition.y.ToString());
                xmlWriter.WriteAttributeString("WorldEndPositionX", worldEndPosition.x.ToString());
                xmlWriter.WriteAttributeString("WorldEndPositionY", worldEndPosition.y.ToString());

                xmlWriter.WriteStartElement("WorldTiles");

                for (var x = 0; x < World.Current.Width; x++)
                {
                    for (var y = 0; y < World.Current.Height; y++)
                    {
                        var tile = World.Current.Tiles[x, y];
                        if (tile.Type == TileType.Empty) continue;

                        xmlWriter.WriteStartElement("Tile");
                        xmlWriter.WriteAttributeString("TileType", tile.Type.ToString());
                        xmlWriter.WriteAttributeString("TileX", tile.X.ToString());
                        xmlWriter.WriteAttributeString("TileY", tile.Y.ToString());
                        xmlWriter.WriteEndElement();
                    }
                }

                xmlWriter.WriteEndElement(); // end WorldTiles element
                xmlWriter.WriteEndElement(); // end LevelData element

                xmlWriter.WriteStartElement("CameraData");
                xmlWriter.WriteAttributeString("CameraX", Camera.main.transform.position.x.ToString());
                xmlWriter.WriteAttributeString("CameraY", Camera.main.transform.position.y.ToString());
                xmlWriter.WriteAttributeString("CameraZoom", Camera.main.orthographicSize.ToString());
                xmlWriter.WriteEndElement(); // end CameraData element

                xmlWriter.WriteEndElement(); // end LevelSaveFile element

                xmlWriter.WriteEndDocument(); // end xml doc
            }
        }

        public void Load(string _worldName)
        {
            World.Current.WorldName = _worldName;
            var loadFile = Path.Combine(Directories.Save_Directory, _worldName + ".xml");
            Clear();

            Debug.Log("Loading: " + loadFile);

            using (var xmlReader = XmlReader.Create(loadFile))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.IsStartElement())
                    {
                        switch (xmlReader.Name)
                        {
                            case "LevelData":
                                var worldStartX = int.Parse(xmlReader["WorldStartPositionX"]);
                                var worldStartY = int.Parse(xmlReader["WorldStartPositionY"]);
                                var worldEndX = int.Parse(xmlReader["WorldEndPositionX"]);
                                var worldEndY = int.Parse(xmlReader["WorldEndPositionY"]);

                                worldStartPosition = new Vector2(worldStartX, worldStartY);
                                worldEndPosition = new Vector2(worldEndX, worldEndY);
                                break;

                            case "Tile":
                                var tileX = int.Parse(xmlReader["TileX"]);
                                var tileY = int.Parse(xmlReader["TileY"]);
                                var tileType = Tile.GetTypeFromString(xmlReader["TileType"]);

                                World.Current.Tiles[tileX, tileY].Type = tileType;
                                break;

                            case "CameraData":
                                var cameraX = float.Parse(xmlReader["CameraX"]);
                                var cameraY = float.Parse(xmlReader["CameraY"]);
                                var cameraZoom = float.Parse(xmlReader["CameraZoom"]);

                                var camPos = Camera.main.transform.position;
                                camPos.x = cameraX;
                                camPos.y = cameraY;
                                Camera.main.transform.position = camPos;
                                Camera.main.orthographicSize = cameraZoom;
                                break;
                        }
                    }
                }
            }

            if (World.Current.OnWorldModifyFinishCallback != null)
                World.Current.OnWorldModifyFinishCallback();
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(worldStartPosition, Vector2.one * 0.5f);

            Gizmos.color = Color.red;
            Gizmos.DrawCube(worldEndPosition, Vector2.one * 0.5f);
        }
    }
}
