using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TrainSystem
{
    public class TrainInput : MonoBehaviour
    {
        public event Action<float> ChangeForceEvent;
        
        private Train _train;

        [SerializeField]
        private float Force
        {
            get => _force;
            set
            {
                _force = value;
                ChangeForceEvent?.Invoke(_force);
            }
        }
        [SerializeField]
        private float _force;

        
        
        private void Awake()
        {
            _train = GetComponent<Train>();
        }

        private void OnEnable()
        {
            TouchTracker.PointerUpEvent += OnPointerUp;
            TouchTracker.PointerPressEvent += OnPointerPress;
        }


        private void OnDisable()
        {
            TouchTracker.PointerUpEvent -= OnPointerUp;
            TouchTracker.PointerPressEvent -= OnPointerPress;
        }

        private void OnPointerUp()
        {
            StartMove();
        }
        
        private void OnPointerPress()
        {
            AccumulationForce();
        }

        private void FixedUpdate()
        {
            if(_train.IsMoved || _train.IsBlocked)
                return;
            
            if (Input.GetKey(KeyCode.Space))
            {
                AccumulationForce();
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                StartMove();
            }
        }

        private void AccumulationForce()
        {
            Force = Mathf.Clamp(Force + Time.fixedDeltaTime / 2, 0, 1);
        }

        private void StartMove()
        {
            _train.Move(_force);
            Force = 0;
        }
    }

}
