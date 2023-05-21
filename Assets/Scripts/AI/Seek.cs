using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : AgentBehavior
{
    public override Steering GetSteering()
    {
        //Steering steer = new Steering();
        Steering steer = ScriptableObject.CreateInstance<Steering>();
        steer.Linear = target.transform.position - transform.position;
        steer.Linear.Normalize();
        steer.Linear = steer.Linear * agent.maxAccel;

        return steer;
    }
}
