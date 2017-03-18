using System;

namespace Assets.Scripts.Game
{
    public abstract class TileInteractable : Tile
    {
        protected TileInteractable(int _x, int _y) : base(_x, _y)
        {

        }

        /// <summary>
        /// Called when the tile is interacted with. Takes a single parameter for passing data to interact behaviour.
        /// </summary>
        /// <param name="_interactData"></param>
        public abstract void OnInteract(object _interactData);
    }
}
