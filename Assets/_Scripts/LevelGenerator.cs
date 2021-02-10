using System;
using System.Collections;
using System.Collections.Generic;
using TrainSystem;
using UnityEngine;
using Tunnels;
using Utils;

namespace GenerationLevel
{
    public class LevelGenerator: MonoBehaviour
    {
        public GenerationTunnel GenerationTunnel;
        public GameObject TrainPrefab;
        public static event Action<Train> SpawnTrainEvent; 
        
        public void Start()
        {
            var train = Instantiate(TrainPrefab).GetComponent<Train>();

            GenerationTunnel.GenerateStartTunnel(3, train);
            
            train.transform.SetX(GenerationTunnel.GetFirstStation().transform.position.x + Constants.MiddleTrain);
            SpawnTrainEvent?.Invoke(train);
        }
    }

}
