using UnityEngine;

namespace Assets.Scripts.Game.Interactables
{
    public abstract class BaseInteractable : MonoBehaviour
    {

        /// <summary>
        /// Called when something interacts with this components game object.
        /// </summary>
        /// <param name="_interactSource">The game object that interacted with this component's game object.</param>
        public abstract void OnInteract(GameObject _interactSource);

    }
}
