﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class BackgroundManager : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
    {

        private bool pressed;
        [SerializeField] private float x, y;
        private Transform _mainMovable;
        
        // Use this for initialization
        void Start()
        {
            _mainMovable = GameObject.FindGameObjectWithTag("Movable").transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (pressed)
                _mainMovable.localPosition += new Vector3(x*200, y*200, 0)*Time.deltaTime;
            OutOfBounds();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pressed = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pressed = false;
        }





        private void OutOfBounds()
        {
            if (_mainMovable.localPosition.x < GameManager.Instance.MinX)
            {
                _mainMovable.localPosition = new Vector3(GameManager.Instance.MinX, _mainMovable.localPosition.y);
            }
            if (_mainMovable.localPosition.x > GameManager.Instance.MaxX)
            {
                _mainMovable.localPosition = new Vector3(GameManager.Instance.MaxX, _mainMovable.localPosition.y);
            }


            if (_mainMovable.localPosition.y < GameManager.Instance.MinY)
            {
                _mainMovable.localPosition = new Vector3(_mainMovable.localPosition.x, GameManager.Instance.MinY);
            }
            if (_mainMovable.localPosition.y > GameManager.Instance.MaxY)
            {
                _mainMovable.localPosition = new Vector3(_mainMovable.localPosition.x, GameManager.Instance.MaxY);
            }
        }
    }
}

    
    

