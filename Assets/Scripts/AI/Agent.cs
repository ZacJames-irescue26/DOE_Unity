using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float MaxSpeed = 10.0f;
    public float TrueMaxSpeed;
    public float maxAccel = 30.0f;

    public float Orientation;
    public float Rotation;
    public Vector3 Velocity;
    protected Steering steer;

    public float MaxRotation = 30.0f;
    public float MaxAngularAccel = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        Velocity = Vector3.zero;
        //steer = new Steering();
        steer = ScriptableObject.CreateInstance<Steering>();

        TrueMaxSpeed = MaxSpeed;
    }

    public void SetSteering(Steering steer, float wight)
    {
        this.steer.Linear += (wight * steer.Linear);
        this.steer.Angular += (wight * steer.Angular);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 displacement = Velocity * Time.deltaTime;
        displacement.y = 0;
        Orientation += Rotation * Time.deltaTime;

        if (Orientation < 0.0f)
        {
            Orientation += 360.0f;
        }
        else if (Orientation > 360.0f)
        {
            Orientation -= 360.0f;
        }

        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, Orientation);

    }

    public virtual void LateUpdate()
    {
        Velocity += steer.Linear * Time.deltaTime;
        Rotation += steer.Angular * Time.deltaTime;
        if(Velocity.magnitude > MaxSpeed)
        {
            Velocity.Normalize();
            Velocity = Velocity * MaxSpeed;

        }
        if(steer.Linear.magnitude == 0.0f)
        {
            Velocity = Vector3.zero;
        }
        //steer = new Steering();
        steer = ScriptableObject.CreateInstance<Steering>();


    }

    public void speedReset()
    {
        MaxSpeed = TrueMaxSpeed;
    }
}
