using System;

namespace Assets.Scripts.Game.Interactables
{
    public class TileDoor : TileInteractable
    {
        /// <summary>
        /// If the door is locked, then it requires a password to open.
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Password used to open door if it is locked.
        /// </summary>
        public string Password { get; set; }


        public TileDoor(int _x, int _y) : base(_x, _y)
        {

        }

        public override void OnInteract(object _interactData)
        {
            if (!Locked)
            {
                OpenDoor();
            }
            else
            {
                var str = _interactData as string;

                // Check if the given data is a valid string
                if (str != null)
                {
                    var givenPassword = str;

                    if (givenPassword == Password)
                    {
                        OpenDoor();
                    }
                }
            }
        }

        private void OpenDoor()
        {
            // TODO: Open door :P
        }
    }
}
