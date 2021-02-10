using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TrainSystem;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;


namespace StationSystem
{
    public class Station : MonoBehaviour
    {
        public event Action<Station> TrainEnterEvent;
        public event Action<Station> TrainExitEvent;

        [SerializeField]
        private GameObject _characterPrefab;
        [SerializeField]
        private Transform _leftUpPoint, _rightDownPoint;
        
        private bool _trainInStation;
        private List<Character> _characters = new List<Character>();
        private Train _train;
        private int _amountCharacters, _charactersInTrain;

        public void Init()
        {
            float y = _characterPrefab.transform.position.y;
            Transform currentTransform = transform;
            for (int i = 0; i < Random.Range(20, 30); i++)
            {
                float x = Random.Range(_leftUpPoint.position.x, _rightDownPoint.position.x);
                float z = Random.Range(_rightDownPoint.position.z, _leftUpPoint.position.z);
                var character = Instantiate(_characterPrefab,
                    new Vector3(x, y, z),
                    Quaternion.identity,
                    currentTransform).GetComponent<Character>();
                
                _characters.Add(character);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Train>(out var train))
            {
                _train = train;
                train.StopInStationEvent += OnStopInStationEvent;
                TrainEnterEvent?.Invoke(this);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Train>(out var train))
            {
                train.StopInStationEvent -= OnStopInStationEvent;
                TrainExitEvent?.Invoke(this);

                if (!_trainInStation)
                    _characters.ForEach(character => character.SetEmotion(Emotion.Angry));
            }
        }
        
        private void OnStopInStationEvent(Station station)
        {
            Debug.Log(transform.parent.name);
            if(station != this || _trainInStation)
                return;
            
            _amountCharacters = Mathf.FloorToInt((float)_characters.Count * _train.GetCountActiveCarriages() / Constants.CountCarriage);
            
            _train.Block();
            
            _trainInStation = true;

            var activeCarriages = _train.GetActiveCarriages();

            for (int i = 0; i < _amountCharacters; i++)
            {
                _characters[i].MoveToNearestCarriage(activeCarriages, CompleteMoveCallback);
                _characters[i].SetEmotion(Emotion.Happy);
            }

            for (int i = _amountCharacters; i < _characters.Count; i++)
                _characters[i].SetEmotion(Emotion.Angry);

        }

        private void CompleteMoveCallback()
        {
            ++_charactersInTrain;
            
            Score.Value++;
            
            if (_charactersInTrain >= _amountCharacters)
                _train.Unblock();
        }

    }
}
