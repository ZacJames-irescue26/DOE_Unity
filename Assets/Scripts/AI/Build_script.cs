using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_script : MonoBehaviour
{
    public GameObject target;
    public Base_Behavior bb;
    public UnitStats stats;
    private BuildingStats buildingStats;

    // Start is called before the first frame update
    void Start()
    {
        bb = GetComponent<Base_Behavior>();
        target = bb.target;
        Debug.Log("Building building");
        buildingStats = target.gameObject.GetComponent<BuildingStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(this);
        }
        if(target != null)
        {
            if ((target.transform.position - transform.position).magnitude > 1.0f && !buildingStats.IsBuilding)
            {
                buildingStats.IsBuilding = true;
            }

        }
    }
    private void OnDestroy()
    {
        buildingStats.IsBuilding = false;

    }
}
