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

        /// <summary>
        /// Always updating logic loop
        /// </summary>
        public void Update()
        {
            if ((Vector2)transform.position != targetPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                if (targetPosition.x > transform.position.x)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
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
                    LuaInterpreter.Current.Resume();
                }
            }
        }

        public void Speak()
        {
            // Lambda
            functions.Add(() => GetComponent<AudioSource>().Play());
        }

        public void MoveLeft()
        {
            functions.Add(() => targetPosition.x -= stepDistance);
        }

        public void MoveRight()
        {
            functions.Add(() => targetPosition.x += stepDistance);
        }

        public void MoveUp()
        {
            functions.Add(() => targetPosition.y += stepDistance);
        }

        public void MoveDown()
        {
            functions.Add(() => targetPosition.y -= stepDistance);
        }
    }
}
