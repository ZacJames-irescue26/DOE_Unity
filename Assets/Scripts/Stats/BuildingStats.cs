using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStats : MonoBehaviour
{
    public int Owner;
    public int Team;
    public float Health = 10;
    public float currentHealth = 10;
    public float BuildingTime = 5;
    public bool IsBuilding = false;
    public BuildingType buildingType;
    public enum BuildingType
    {
        Barraks,
        Base
    }

    public void TakeDamage(float Damage)
    {
        currentHealth -= Damage;
    }

    void IsDestroyed()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        IsDestroyed();
        if(IsBuilding)
        {
            if(BuildingTime > 0)
            {
                float HealthPerTick = Health/BuildingTime;
                if(currentHealth < Health)
                {
                    currentHealth += HealthPerTick * Time.deltaTime;
                }
                BuildingTime -= Time.deltaTime;
            }

        }
    }
}
