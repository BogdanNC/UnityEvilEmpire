using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class ChaseEnemy : ActionNode
{
    private Transform transform;

    protected override void OnStart() {

        transform = context.gameObject.transform;
        blackboard.moveScript = transform.gameObject.GetComponent<MouseMove>();

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if(blackboard.enemies == null || blackboard.enemies.Count <= 0)
        {
            return State.Failure;
        }

        GameObject target = TargetEnemy(blackboard.enemies);

        if(target == null)
        {
            return State.Failure;
        }

        blackboard.moveScript.SetDestination(target.transform.position);

        if (Vector3.Distance(transform.position, target.transform.position) <= blackboard.attackRng)
        {
            blackboard.moveScript.Stop();

            CombatManager enemy = target.GetComponent<CombatManager>();

            if (enemy.TakeDamage(blackboard.baseDmg))
            {
                return State.Success;
            }
        }
        else
        {
            blackboard.moveScript.MoveToPos();
        }

        return State.Running;
    }

    private GameObject TargetEnemy(List<GameObject> enemies)
    {
        float minDistance = float.MaxValue;
        GameObject target = null;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (minDistance > distance)
            {
                minDistance = distance;
                target = enemy;
            }
        }

        //Target can be null
        return target;
    }
}
