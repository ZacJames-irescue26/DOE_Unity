using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public int Owner = 0;
    public int Team = 0;
    public float Health = 10;
    public float currentHealth = 10;
    public float AttackDamage = 1;
    public float AttackSpeed = 1;
    public float AttackTimer = 1.0f;
    public float BuildTime = 5;
    public bool CanAttack = true;
    public float Timer;
    public float AttackRange = 10;
    public float AggroRange = 20;
    public UnitClass Class;
    public enum UnitClass
    {
        Worker,
        Military
    }
    public void TakeDamage(float Damage)
    {
        currentHealth -= Damage;
    }

    private void Update()
    {
        UpdateTimer();
        IsDestroyed();
    }
    void UpdateTimer()
    {
        if (!CanAttack)
        {
            if (Timer <= 0)
            {
                CanAttack = true;
                Timer = AttackTimer;
            }
            else
            {
                Timer -= AttackSpeed * Time.deltaTime;
            }
        }
    }

    void IsDestroyed()
    {
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

}
