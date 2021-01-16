using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Vector2 position;


    private void Update()
    {
        position = transform.position;
    }
}
