using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Flee : AgentBehavior
{
    //run away from the target
    public override Steering GetSteering()
    {
        //Steering steer = new Steering();
        Steering steer = ScriptableObject.CreateInstance<Steering>();
        steer.Linear = transform.position - target.transform.position;
        steer.Linear.Normalize();
        steer.Linear = steer.Linear * agent.maxAccel;
        return steer;
    }
}
