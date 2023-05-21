using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Behavior : MonoBehaviour
{

    public int team;

    //links to the different behavior components
    //public idle_script idle;
    public Seek_Target seekScript;
    public CheckingForTargets checking;
    //public BoidCohesion boidCoh;
   // public BoidsSepparation boidSep;
    public Attack_Script attack;
    public Build_script build;
    public GoToPoint MoveToPoint;
    //gps is our general pathfinding script
    //public general_pathfinding gps;

    //intelligent movement scripts
    public Agent agentScript;

    public Seek seek;
    //public Flee fleeScript;

    public float maxSpeed;

    public GameObject target;
    public UnitFSM state;

    //gps is our general pathfinding script
    public General_Pathfinding gps;

    public float viewDistance = 360.0f; //180 is probably a fine number
    public float shootDistance = 60.0f;
    public enum UnitFSM //states
    {
        Attack,
        Seek,
        Idle,
        MoveAttack,
        Build
    }

    // Start is called before the first frame update
    void Start()
    {
        agentScript = gameObject.AddComponent<Agent>(); //add agent
        agentScript.MaxSpeed = maxSpeed;

        changeState(UnitFSM.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (state == UnitFSM.Idle)
            {
                changeState(UnitFSM.Seek);
            }
            else
            {
                changeState(UnitFSM.Idle);
            }
            
        }
    }

    public void changeState(UnitFSM new_state)
    {

        state = new_state;

        switch (new_state)
        {
            case UnitFSM.Idle:

                /* if (gameObject.GetComponent<idle_script>() == null)
                 {
                     idle = gameObject.AddComponent<idle_script>();
                 }
                 DestroyImmediate(attack);*/
                if(gameObject.GetComponent<CheckingForTargets>() == null) 
                {
                    checking = gameObject.AddComponent<CheckingForTargets>();
                }
                Destroy(seekScript);
                Destroy(attack);
                Destroy(MoveToPoint);
                Destroy(build);

                break;

            case UnitFSM.Seek:

                if (gameObject.GetComponent<Seek_Target>() == null)
                {
                    seekScript = gameObject.AddComponent<Seek_Target>();
                    seekScript.target = target;
                }
                Destroy(attack);
                Destroy(checking);
                Destroy(build);
                Destroy(MoveToPoint);

                //DestroyImmediate(idle);

                break;

            case UnitFSM.MoveAttack:
                if (gameObject.GetComponent<Seek_Target>() == null)
                {
                    seekScript = gameObject.AddComponent<Seek_Target>();
                    seekScript.target = target;
                }
                if (gameObject.GetComponent<CheckingForTargets>() == null)
                {
                    checking = gameObject.AddComponent<CheckingForTargets>();
                }

                Destroy(attack);
                Destroy(build);
                Destroy(MoveToPoint); 

                break;

            case UnitFSM.Attack:

                if (gameObject.GetComponent<Attack_Script>() == null)
                {
                    attack = gameObject.AddComponent<Attack_Script>();
                    
                }
                //agentScript.Velocity = Vector3.zero;
                Debug.Log("Target: " + target);
                /*
                DestroyImmediate(idle);
                */
                Destroy(seekScript);
                Destroy(checking);
                Destroy(build);
                Destroy(MoveToPoint);

                break;
            case UnitFSM.Build:
                if (gameObject.GetComponent<GoToPoint>() == null)
                {
                    MoveToPoint = gameObject.AddComponent<GoToPoint>();
                    MoveToPoint.target = target;
                }
                if (gameObject.GetComponent<Build_script>() == null)
                {
                    build = gameObject.AddComponent<Build_script>();
                }
                Destroy(checking);
                Destroy(attack);
                Destroy(seekScript);
                break;



        }
    }
}
