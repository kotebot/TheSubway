using System;
using UnityEngine;
using Utils;

public static class Score
{
    private static int _value;
    public static int Value
    {
        set
        {
            if (value < 0) return;
            
            _value = value;
            OnValueChanged?.Invoke(_value);

            if (_value > MaxValue)
                MaxValue = _value;
        }
        get => _value;
    }

    private static int MaxValueSave
    {
        set => PlayerPrefs.SetInt(Constants.Score, value);
        get => PlayerPrefs.GetInt(Constants.Score, 0);
    }
    
    public static int MaxValue { 
        private set
        {
            if (value < Value) return;
            
            MaxValueSave = value;
            OnMaxValueChanged?.Invoke(MaxValueSave);
        }
        get => MaxValueSave;
        
    }

    public static event Action<int> OnValueChanged;
    public static event Action<int> OnMaxValueChanged;
}
