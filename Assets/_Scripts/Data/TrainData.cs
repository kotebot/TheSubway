using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Train", order = 1)]
public class TrainData : ScriptableObject
{
    public string Name;
    public int Cost;
    [PreviewField(128)]
    public Sprite Icon;
    
    [Range(10,1000)]
    public float Force;
    public AnimationCurve SpeedCurve;
}
