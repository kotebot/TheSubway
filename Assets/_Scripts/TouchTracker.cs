using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchTracker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public static event Action PointerDownEvent;
    public static event Action PointerUpEvent;
    public static event Action PointerClickEvent;
    public static event Action PointerPressEvent;

    private bool _isPressing;

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDownEvent?.Invoke();
        _isPressing = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUpEvent?.Invoke();
        _isPressing = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerClickEvent?.Invoke();
    }

    public void FixedUpdate()
    {
        if(_isPressing)
            PointerPressEvent?.Invoke();
    }
}
