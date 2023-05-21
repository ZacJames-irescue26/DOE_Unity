using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Script : MonoBehaviour
{
    public GameObject target;
    public Base_Behavior bb;
    public UnitStats stats;
    
    
    // Start is called before the first frame update
    void Start()
    {
        bb = GetComponent<Base_Behavior>();
        stats = GetComponent<UnitStats>();
        target = bb.target;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsAlive())
        {
            
            if(stats.CanAttack)
            {
                if (IsInRange())
                {
                    if (target.GetComponent<UnitStats>())
                    {
                        target.GetComponent<UnitStats>().TakeDamage(stats.AttackDamage);
                    }
                    else
                    {
                        target.GetComponent<BuildingStats>().TakeDamage(stats.AttackDamage);
                    }
                    stats.CanAttack = false;
                }
                else
                {
                    bb.changeState(Base_Behavior.UnitFSM.Idle);
                    Destroy(this);
                }

            }
            
            
        }
        else
        {
            bb.changeState(Base_Behavior.UnitFSM.Idle);
            Destroy(this);
        }
    }
    public bool IsAlive()
    {
        return target != null;
    }

    public bool IsInRange()
    {
        if ((gameObject.transform.position - target.transform.position).magnitude < (stats.AttackRange))
        {
            return true;
        }
        else
        {
            bb.changeState(Base_Behavior.UnitFSM.Idle);
            Destroy(this);
            return false;
        }
    }


}
