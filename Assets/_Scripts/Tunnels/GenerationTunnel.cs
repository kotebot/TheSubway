using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using StationSystem;
using TrainSystem;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Tunnels
{
    public class GenerationTunnel : MonoBehaviour
    {
        public int StartMinAmountChunks;
        public int StartMaxAmountChunks;
        
        [SerializeField] private GameObject _chunkTunnel;
        [SerializeField] private GameObject _station;
        [SerializeField] private Train _train;

        [SerializeField]
        private Queue<GameObject> _stationBlocks = new Queue<GameObject>();
        
        private List<Station> _stations = new List<Station>();
    
        private float _currentLength = 0;
        private GameObject _parentTunnel;
        [ShowInInspector, ReadOnly]
        private int _nextStation = 1;
        public void GenerateStartTunnel(int countStation, Train train)
        {
            _train = train;
            _parentTunnel = new GameObject("Tunnel");

            for (int i = 0; i < countStation; i++)
                GenerateStationBlock(StartMinAmountChunks, StartMaxAmountChunks);
        }

        public Station GetFirstStation() => _stations.FirstOrDefault();
        public Station GetNextStation() => _stations[_nextStation];
        
        private void GenerateStationBlock(int minCountChunks, int maxCountChunks)
        {
            _stationBlocks.Enqueue(GenerateStationBlock(_parentTunnel, minCountChunks, maxCountChunks));
        }

        [Button]
        private void RemovePreviewStationBlock()
        {
            var block = _stationBlocks.Dequeue();
            
            var station = block.GetComponentInChildren<Station>();
            _stations.Remove(station);
            station.TrainEnterEvent -= OnTrainEnterStation;
            station.TrainExitEvent -= OnTrainExitStation;
            
            Destroy(block);
        }
        
        private void OnTrainEnterStation(Station station)
        { 
            GenerateStationBlock(StartMinAmountChunks, StartMaxAmountChunks);
            _nextStation++;
        }
        
        private void OnTrainExitStation(Station station)
        {
            _nextStation--;
            RemovePreviewStationBlock();
        }

        private GameObject GenerateStationBlock(GameObject tunnel, int minCountChunks, int maxCountChunks)
        {
            GameObject stationBlock = new GameObject("StationBlock" + _stationBlocks.Count);
            
            int countChunks = (int)(Random.Range(minCountChunks, (float)maxCountChunks));
            
            
            for (int j = 0; j < countChunks; j++)
                SpawnChunkTunnel(ref _currentLength, tunnel).transform.SetParent(stationBlock.transform);
            
            var station = SpawnStation(ref _currentLength, tunnel);

            station.transform.SetParent(stationBlock.transform);
            _stations.Add(station);
            
            stationBlock.transform.SetParent(tunnel.transform);
            
            return stationBlock;
        }
        
        private GameObject SpawnChunkTunnel(ref float _currentLength, GameObject parent = null)
        {
            _currentLength += Constants.ChunkTunnelLength;
            var tunnel = Instantiate(_chunkTunnel, new Vector3(_currentLength, 0, 0), Quaternion.identity);

            if (parent == null)
                return tunnel;
        
            tunnel.transform.SetParent(parent.transform);

            return tunnel;
        }
    
        private Station SpawnStation(ref float _currentLength, GameObject parent = null)
        {
            _currentLength += Utils.Constants.StationLength;
            
            var station = Instantiate(_station, new Vector3(_currentLength - Constants.CountCarriage * Constants.ChunkTunnelLength, 0, 0), Quaternion.identity).
                GetComponent<Station>();//-3 before spawn station in start

            if(_stations.Count != 0)
            {
                station.TrainEnterEvent += OnTrainEnterStation;
                station.TrainExitEvent += OnTrainExitStation;
                station.Init();
            }
            
            if (parent == null)
                return station;
        
            station.transform.SetParent(parent.transform);

            return station;
        }
    
    }

}
