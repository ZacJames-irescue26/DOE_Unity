using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    float speed = 0.06f;
    float zoomeSpeed = 10.0f;
    float rotateSpeed = 0.1f;

    float maxheight = 40.0f;
    float minheight = 4.0f;

    Vector2 p1;
    Vector2 p2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hsp = speed * Input.GetAxis("Horizontal");
        float vsp = speed * Input.GetAxis("Vertical");
        float scrollSp = -zoomeSpeed * Input.GetAxis("Mouse ScrollWheel");

        if((transform.position.y >= maxheight) && (scrollSp > 0))
        {
            scrollSp = 0;
        }
        else if((transform.position.y <= minheight) && scrollSp < 0) 
        {
            scrollSp = 0;
        }

        if((transform.position.y + scrollSp) > maxheight) 
        {
            scrollSp = maxheight - transform.position.y;
        }
        else if((transform.position.y + scrollSp) < minheight) 
        { 
            scrollSp = minheight - transform.position.y;
        }

        Vector3 VerticleMove = new Vector3(0, scrollSp, 0);
        Vector3 LateralMove = hsp * transform.right;
        Vector3 forwardMove = transform.forward;
        forwardMove.y = 0;
        forwardMove.Normalize();
        forwardMove *= vsp;

        Vector3 move = VerticleMove + LateralMove + forwardMove;

        transform.position += move;
        getCameraRotation();
    }

    void getCameraRotation()
    {
        if(Input.GetMouseButtonDown(2)) // check if the middle mouse button is pressed
        {
            p1 = Input.mousePosition;
        }
        if(Input.GetMouseButton(2)) // check if the middle mouse button is being held down
        {
            p2 = Input.mousePosition;
            float dx = (p2 - p1).x * rotateSpeed;
            float dy = (p2 - p1).y * rotateSpeed;

            transform.rotation *= Quaternion.Euler(new Vector3(0, dx, 0)); // Y rotation
            transform.GetChild(0).rotation *= Quaternion.Euler(new Vector3(-dy,0,0));
            p1 = p2;
        }
    }
}
