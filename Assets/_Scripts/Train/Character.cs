using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TrainSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TrainSystem
{
    public class Character : MonoBehaviour
    {
        [SerializeField]
        private Material _happyMaterial, _angryMaterial;
        
        private MeshRenderer _meshRenderer;
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void MoveToNearestCarriage(List<Carriage> carriages, Action completeCallback)
        {
            var nearestCarriage = carriages.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude).First();
            transform.
                DOMove(nearestCarriage.transform.position, Random.Range(0.5f, 2.5f))
                .OnComplete(() =>
                {
                    completeCallback?.Invoke();
                    gameObject.SetActive(false);
                });
        }

        public void SetEmotion(Emotion emotion)
        {
            _meshRenderer.material = emotion == Emotion.Happy ? _happyMaterial : _angryMaterial;
        }
    }

    public enum Emotion
    {
        Happy,
        Angry
    }
}

