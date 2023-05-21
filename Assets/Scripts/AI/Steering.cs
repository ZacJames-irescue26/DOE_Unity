using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : ScriptableObject
{
    public float Angular;
    public Vector3 Linear;
    public Steering()
    {
        Angular = 0.0f;
        Linear = new Vector3();
    }
}
