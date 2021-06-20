using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public Vector2 position;
    public float x = 0;
    public float y = 0;

    public static bool operator <(Fly a, float lowerBound)
    {
        if (a.x < lowerBound || a.y < lowerBound)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator >(Fly a, float upperBound)
    {
        if (a.x > upperBound || a.y > upperBound)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateXDimension(float x)
    {
        this.x = x;
    }

    public void UpdateYDimension(float y)
    {
        this.y = y;
    }

    public void UpdatePosition(float x, float y)
    {
        position = new Vector2(x, y);
        transform.position = new Vector2(x, y);
    }
}
