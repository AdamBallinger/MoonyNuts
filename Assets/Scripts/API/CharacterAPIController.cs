using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.API
{
    public class CharacterAPIController : MonoBehaviour
    {

        public float speed = 1f;

        public float stepDistance = 1f;

        private Vector2 targetPosition;

        private List<Action> functions;

        public void Start()
        {
            functions = new List<Action>();
            CharacterAPI.AddObject(gameObject);

            targetPosition = transform.position;
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
            functions.Add(() => targetPosition.x -= stepDistance);
        }

        /// <summary>
        /// API call to make the character move right by the specified step distance.
        /// </summary>
        public void MoveRight()
        {
            functions.Add(() => targetPosition.x += stepDistance);
        }

        /// <summary>
        /// API call to make the character move up by the specified step distance.
        /// </summary>
        public void MoveUp()
        {
            functions.Add(() => targetPosition.y += stepDistance);
        }

        /// <summary>
        /// API call to make the character move down by the specified step distance.
        /// </summary>
        public void MoveDown()
        {
            functions.Add(() => targetPosition.y -= stepDistance);
        }
    }
}
