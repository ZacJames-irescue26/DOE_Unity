using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckingForTargets : MonoBehaviour
{
    private Collider[] RangeColldiers;
    public UnitStats unitstats;
    public bool HasAggro = false;
    // Start is called before the first frame update
    void Start()
    {
        unitstats = GetComponent<UnitStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!HasAggro)
        {
            CheckForEnemyTargets();

        }

    }

    private void CheckForEnemyTargets()
    {
        RangeColldiers = Physics.OverlapSphere(transform.position, 0.8f*unitstats.AttackRange, 1<<6 | 1<<10 | 1<<11);
        for(int i = 0; i < RangeColldiers.Length; i++)
        {
            GameObject UnknownTarget = RangeColldiers[i].gameObject;
            if (UnknownTarget.GetComponent<UnitStats>() != null)
            {
                int team = UnknownTarget.GetComponent<UnitStats>().Team;
                if (team != unitstats.Team)
                {
                    HasAggro = true;
                    Base_Behavior bb = gameObject.GetComponent<Base_Behavior>();
                    bb.target = UnknownTarget;
                    bb.changeState(Base_Behavior.UnitFSM.Attack);
                }

            }
            else if(UnknownTarget.GetComponent<BuildingStats>() != null)
            {
                int team = UnknownTarget.GetComponent<BuildingStats>().Team;
                if (team != unitstats.Team)
                {
                    HasAggro = true;

                    Base_Behavior bb = gameObject.GetComponent<Base_Behavior>();
                    bb.target = UnknownTarget;
                    bb.changeState(Base_Behavior.UnitFSM.Attack);

                }
            }
        }
    }
}
