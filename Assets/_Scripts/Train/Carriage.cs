using System;
using System.Collections;
using System.Collections.Generic;
using StationSystem;
using UnityEngine;

namespace TrainSystem
{
    public class Carriage : MonoBehaviour
    {
        public bool IsActive { private set; get; } = true;
        public Station CurrentStation;
        public event Action<Carriage> StationExitEvent;
        public bool InStation { private set; get; }
        
        private Train _parentTrain;
       
        
        public void Init(Train train)
        {
            _parentTrain = train;
            _parentTrain.StopMoveEvent += OnStopTrain;
        }

        private void OnTriggerEnter(Collider other)
        {
            CurrentStation = other.GetComponent<Station>();
            if (CurrentStation != null)
                InStation = true;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Station>() != null)
            {
                StationExitEvent?.Invoke(this);
                CurrentStation = null;
                InStation = false;
            }
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            
            IsActive = true;
        }
        
        public void Disable()
        {
            gameObject.SetActive(false);
            IsActive = false;
        }
        
        private void OnStopTrain()
        {
            if(!InStation)
                Disable();
        }
        

    }

}
