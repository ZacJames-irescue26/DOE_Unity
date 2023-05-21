using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBuilding : MonoBehaviour
{
    public GameObject buildingIcon;
    public void Spawn()
    {
        Instantiate(buildingIcon);
    }
}
