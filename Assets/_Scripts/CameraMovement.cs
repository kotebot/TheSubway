using System;
using System.Collections;
using System.Collections.Generic;
using GenerationLevel;
using TrainSystem;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;
    
    private Transform _transform;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void OnEnable()
    {
        LevelGenerator.SpawnTrainEvent += LevelGeneratorOnSpawnTrainEvent;
    }
    
    private void OnDisable()
    {
        LevelGenerator.SpawnTrainEvent -= LevelGeneratorOnSpawnTrainEvent;
    }

    private void LevelGeneratorOnSpawnTrainEvent(Train train)
    {
        _target = train.transform;
    }

    private void Start()
    {
        _transform = transform;
    }

    void LateUpdate()
    {
        if (!_target)
            return;
        
        Vector3 targetPosition = new Vector3(_target.position.x, _transform.position.y, _transform.position.z);
        
        _transform.position = Vector3.SmoothDamp(_transform.position, targetPosition, ref velocity, smoothTime);
    }
}
