using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using StationSystem;
using UnityEngine;

namespace TrainSystem
{
    public class Train : MonoBehaviour
    {
        [ShowInInspector]
        public bool IsMoved { private set; get; } = false;
        public bool IsBlocked { private set; get; }
        
        public event Action StopMoveEvent;
        public event Action <Station> StopInStationEvent;
        public event Action StartMoveEvent;
        
        [SerializeField] private TrainData _data;

        public Carriage[] Carriages => _carriages;
        [SerializeField] private Carriage[] _carriages;
       

        public void Init(TrainData data)
        {
            _data = data;
        }
        
        private void Start()
        {
            _carriages.ForEach(carriages => carriages.Init(this));
            GetLastCarriage().StationExitEvent += OnLastCarriageExit;
        }

        [Button]
        public void Move(float force)
        {
            if(IsBlocked)
                return;
            
            if (force <= 0 || force > 1)
                throw new ArgumentException("force is wrong");

            IsMoved = true;
            StartMoveEvent?.Invoke();
            
            transform.DOMoveX(transform.position.x + _data.Force * force, 5 * force).
                SetEase(_data.SpeedCurve)
                .OnComplete(Stop);
        }

        public void Block() => IsBlocked = true;

        public void Unblock() => IsBlocked = false;

        public bool IsExist() => _carriages.Any(carriage => carriage.IsActive);
        public Carriage GetLastCarriage() => _carriages.LastOrDefault(carriage => carriage.IsActive);
        
        public int GetCountActiveCarriages() => _carriages.Count(carriage => carriage.IsActive);
        
        public List<Carriage> GetActiveCarriages() => _carriages.Where(carriage => carriage.IsActive).ToList();

        private void OnLastCarriageExit(Carriage carriage)
        {
            if (carriage != GetLastCarriage())
            {
                GetLastCarriage().StationExitEvent -= OnLastCarriageExit;
                return;
            }

            carriage.StationExitEvent += OnLastCarriageExit;
            //carriage.InStation
        }
        
        private void Stop()
        {
            IsMoved = false;
            StopMoveEvent?.Invoke();

            if (!IsExist())
                GameManager.Reload();
            else StopInStationEvent?.Invoke(InStation());
        }
        
        private Station InStation() => _carriages.First(carriage => carriage.IsActive).CurrentStation;


    }
}