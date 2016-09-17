﻿using System;
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
            }
            else
            {
                if (functions.Count > 0)
                {
                    functions[0]();
                    functions.RemoveAt(0);
                }
            }
        }

        public void MoveLeft()
        {
            functions.Add(Left);
        }

        private void Left()
        {
            targetPosition.x -= stepDistance;
        }

        public void MoveRight()
        {
            functions.Add(Right);
        }

        private void Right()
        {
            targetPosition.x += stepDistance;
        }

        public void MoveUp()
        {
            functions.Add(Up);
        }

        private void Up()
        {
            targetPosition.y += stepDistance;
        }

        public void MoveDown()
        {
            functions.Add(Down);
        }

        private void Down()
        {
            targetPosition.y -= stepDistance;
        }
    }
}