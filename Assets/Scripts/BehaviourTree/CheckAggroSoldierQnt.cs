using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckAggroSoldierQnt : ActionNode
{
    private Transform transform;

    private int criticalMass = 10;

    protected override void OnStart() {
        transform = context.gameObject.transform;
        blackboard.nearbySoldiers = 0;

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        blackboard.nearbySoldiers = CountSoldiers();

        if(blackboard.nearbySoldiers < criticalMass)
        {
            return State.Failure;
        }

        return State.Success;
    }

    private int CountSoldiers()
    {
        int result = 0;
        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, blackboard.fov);

        foreach (var collider in surroundingColliders)
        {
            //Checks the collider's GameObject's tag
            if (collider.gameObject.name.Contains("Soldier") && collider.gameObject.CompareTag("Enemy"))
            {
                //Add the enemy to the list of enemies
                result++;
            }
        }

        return result;
    }
}
