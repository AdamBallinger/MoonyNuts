using UnityEngine;

namespace Assets.Scripts.Game
{
    public class WorldController : MonoBehaviour
    {

        public Sprite gridSprite;

        [SerializeField]
        private int worldWidth = 16;
        [SerializeField]
        private int worldHeight = 16;

        public void Start()
        {
            World.Create(worldWidth, worldHeight);

            for(var x = 0; x < worldWidth; x++)
            {
                for(var y = 0; y < worldHeight; y++)
                {
                    var tileGO = new GameObject();
                    tileGO.transform.SetParent(transform);
                    tileGO.name = string.Format("Tile:  X:{0}  Y:{1}", x, y);
                    tileGO.tag = "Tile";

                    var tileData = World.Current.GetTileAt(x, y);
                    tileData.RegisterTileTypeChangeCallback(tile => { OnTileTypeChanged(tileGO, tile); });

                    var tileSR = tileGO.AddComponent<SpriteRenderer>();
                    tileSR.sprite = gridSprite;

                    tileGO.transform.position = new Vector2(tileData.X, tileData.Y);
                }
            }
        }

        public void OnTileTypeChanged(GameObject _tileGO, Tile _tileData)
        {
            
        }

        public void Clear()
        {
            World.Current.Clear();
        }
    }
}
