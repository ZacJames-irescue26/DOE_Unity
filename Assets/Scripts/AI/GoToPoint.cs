using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GoToPoint : MonoBehaviour
{
    public GameObject target;
    Base_Behavior ub;
    // Start is called before the first frame update
    void Start()
    {
        ub = gameObject.GetComponent<Base_Behavior>();

        ub.gps = gameObject.AddComponent<General_Pathfinding>(); //create the general pathfinding script

        ub.gps.target = target;
    }

    // Update is called once per frame
    void Update()
    {
       seekPoint();
    }
    void seekPoint() //seek a non-enemy target
    {

        if ((target.transform.position - transform.position).magnitude > 1.0f)
        {
            ub.gps.target = target;
            ub.gps.steeringEnable();
        }
        else//we are at the target
        {
            Destroy(ub.gps);
            Destroy(this);
        }
    }
}
