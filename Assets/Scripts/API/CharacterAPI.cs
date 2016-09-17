using UnityEngine;

using System.Collections.Generic;

namespace Assets.Scripts.API
{
    public class CharacterAPI
    {

        private static List<GameObject> objects = new List<GameObject>();

        public static void AddObject(GameObject _object)
        {
            if(!objects.Contains(_object))
            {
                objects.Add(_object);
            }
        }

        public static int GetID(GameObject _object)
        {
            if(objects.Contains(_object))
            {
                return objects.IndexOf(_object);
            }

            Debug.LogError("[CharacterAPI] - Object has no ID because it doesnt exist in the objects list. Make sure its been added!");
            return -1;
        }

        public static CharacterAPIController GetGameObject(int _id)
        {
            if(_id > objects.Count || _id < 0)
            {
                Debug.LogError("[CharacterAPI] - Attempted to get a gameobject out of the objects list bounds!");
            }

            return objects[_id].GetComponent<CharacterAPIController>();
        }

    }
}
