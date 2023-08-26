using System;
using System.Collections;
using Interactable.Utility;
using Managers;
using UnityEngine;

namespace mini_game.ElectronicMaze
{
    public class Ball : MonoBehaviour
    {
        
         
        [SerializeField] private GameObject ballLose;
        [SerializeField] private GameObject ballWin;

        private readonly Vector2 _startPosition = new Vector2(150, 135);
        private Vector2 _direction;
        private const float Speed = 27.5f;
        private bool _hasWin;

        private ElectronicMaze _controller;

        private void Start()
        {
            _controller = GameObject.FindWithTag("ElectronicMaze").GetComponent<ElectronicMaze>();
            _direction = Vector2.zero;
        }

        private void Update()
        {
            if (_hasWin) return;
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _direction = Vector2.down;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _direction = Vector2.left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _direction = Vector2.right;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _direction = Vector2.up;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _controller.ResetAndClose();
            }

            transform.Translate(_direction * (Speed * Time.deltaTime));
            OutOfBoundsCheck();
        }

        private void OutOfBoundsCheck()
        {
            if (transform.localPosition.x > 150)
            {
                transform.localPosition = _startPosition;
            }
            
            if (transform.localPosition.x < -151)
            {
                transform.localPosition = _startPosition;
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.name.Contains("Wall"))
            {
                _direction = Vector2.zero;
                var transform1 = transform;
                transform1.localPosition = _startPosition;
                Instantiate(ballLose, transform1.position, Quaternion.identity);
            }else if (col.gameObject.name.Equals("Target"))
            {
                if (_hasWin) return;
                _hasWin = true;
                Instantiate(ballWin, transform.position, Quaternion.identity);
                _controller.Win();
            }
        }
    }
}