using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    bool isWrappingOnX = false, isWrappingOnY = false;
    
    private void Update() => CheckIfWrappingIsNeeded();

    private void CheckIfWrappingIsNeeded()
    {
        if (IsOffScreen())
        {
            WrapObject();
        }
        else
        {
            isWrappingOnX = false;
            isWrappingOnY = false;
        }
    }
    private void WrapObject()
    {
        if (isWrappingOnX && isWrappingOnY) return;
        var newPosition = transform.position;
        if (IsOffScreenVertical())
        {
            newPosition.y = -newPosition.y;
            isWrappingOnY = true;
        }
        else if (IsOffScreenHorizontal())
        {
            newPosition.x = -newPosition.x;
            isWrappingOnX = true;
        }
        transform.position = newPosition;
    }
    private bool IsOffScreenHorizontal()
    {
        return !isWrappingOnX && (transform.position.x > 6.5 || transform.position.x < -6.5);
    }
    private bool IsOffScreenVertical()
    {
        return !isWrappingOnY && (transform.position.y > 5 || transform.position.y < -5);
    }
    private bool IsOffScreen()
    {
        return !gameObject.GetComponent<Renderer>().isVisible;
    }
}
