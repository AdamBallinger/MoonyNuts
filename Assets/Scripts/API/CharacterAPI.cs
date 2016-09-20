using UnityEngine;

using System.Collections.Generic;

namespace Assets.Scripts.API
{
    public static class CharacterAPI
    {

        /// <summary>
        /// A list of gameobjects that are registered to the CharacterAPI. Only objects in here will work with the LUA interpreter API calls.
        /// </summary>
        private static List<GameObject> objects = new List<GameObject>();

        /// <summary>
        /// Adds a new character gameobject to the API.
        /// </summary>
        /// <param name="_object"></param>
        public static void AddObject(GameObject _object)
        {
            if(!objects.Contains(_object))
            {
                objects.Add(_object);
            }
        }

        /// <summary>
        /// Gets the ID for a given gameobject.
        /// </summary>
        /// <param name="_object"></param>
        /// <returns></returns>
        public static int GetID(GameObject _object)
        {
            if(objects.Contains(_object))
            {
                return objects.IndexOf(_object);
            }

            Debug.LogError("[CharacterAPI] - Object has no ID because it doesnt exist in the objects list. Make sure its been added!");
            return -1;
        }

        /// <summary>
        /// Returns the API controller for the gameobject with the given ID, or null if no object exists with the given ID.
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
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
