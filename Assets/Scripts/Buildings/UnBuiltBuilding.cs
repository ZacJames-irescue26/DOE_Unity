using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnBuiltBuilding : MonoBehaviour
{
    BuildingStats stats;
    public GameObject Building;
    void Start()
    {
        stats = GetComponent<BuildingStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stats.BuildingTime < 0)
        {
            stats.currentHealth = stats.Health;
            Instantiate(Building, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
