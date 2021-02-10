using System;
using System.Collections;
using System.Collections.Generic;
using GenerationLevel;
using TMPro;
using TrainSystem;
using Tunnels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Image _forceMoveImage;
        [SerializeField] private TMP_Text _rangeToNextStation; 
        [SerializeField] private TMP_Text _value; 
        [SerializeField] private TMP_Text _maxValue; 
        private Train _train;
        private TrainInput _trainInput;
        
        [Inject] private GenerationTunnel _generationTunnel;
        
        private const string _nextStation = "NEXT\nSTATION -> ";
        
        private void OnEnable()
        {
            LevelGenerator.SpawnTrainEvent += LevelGeneratorOnSpawnTrainEvent;
            Score.OnValueChanged += ScoreOnValueChanged;
            Score.OnMaxValueChanged += ScoreOnMaxValueChanged;
        }
        
        private void OnDisable()
        {
            LevelGenerator.SpawnTrainEvent -= LevelGeneratorOnSpawnTrainEvent;
            
            if(_trainInput)
                _trainInput.ChangeForceEvent -= TrainInputOnChangeForce;
        }
        
        private void Update()
        {
            if(_train.IsMoved)
                UpdateRangeText();
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            ScoreOnValueChanged(0);
            ScoreOnMaxValueChanged(Score.MaxValue);
        }

        private void ScoreOnValueChanged(int value)
        {
            _value.SetText(value.ToString());
        }
        
        private void ScoreOnMaxValueChanged(int maxValue)
        {
            _maxValue.SetText(maxValue.ToString());
        }
        
        
        private void TrainInputOnChangeForce(float force)
        {
            _forceMoveImage.fillAmount = force;
        }
        
        private void LevelGeneratorOnSpawnTrainEvent(Train train)
        {
            _train = train;
            _trainInput = _train.GetComponent<TrainInput>();
            _trainInput.ChangeForceEvent += TrainInputOnChangeForce;
            UpdateRangeText();
        }
        
        private void UpdateRangeText()
        {
            _rangeToNextStation.SetText(_nextStation + GetDistance());
        }
        
        private int GetDistance()
        {
            return (int)(Vector3.Distance(
                _train.transform.position,
                _generationTunnel.GetNextStation().transform.position) / 5);
        }
    }

}
