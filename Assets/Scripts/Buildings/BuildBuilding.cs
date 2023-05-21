using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBuilding : MonoBehaviour
{
    RaycastHit hit;
    public GameObject BuildingSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 500000.0f, 1<<8))
        {
            transform.position = hit.point;
        }
        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(BuildingSpawn, hit.point, transform.rotation);
            Destroy(gameObject);

        }
        if(Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }

    }
}
