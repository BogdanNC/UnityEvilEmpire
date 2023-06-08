using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class ChaseEnemy : ActionNode
{
    private Transform transform;

    private float cooldown = 1.0f;
    private float waitTime = 0.0f;
    private bool canAttack = true;

    protected override void OnStart() {
        transform = context.gameObject.transform;
        blackboard.moveScript = transform.gameObject.GetComponent<MouseMove>();

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (blackboard.enemies == null || blackboard.enemies.Count <= 0)
        {
            return State.Failure;
        }

        GameObject target = TargetEnemy(blackboard.enemies);

        if(target == null)
        {
            return State.Failure;
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= blackboard.attackRng)
        {
            if (canAttack)
            {
                blackboard.moveScript.Stop();

                CombatManager enemy = target.GetComponent<CombatManager>();

                if (enemy.TakeDamage(blackboard.baseDmg))
                {
                    target = TargetEnemy(blackboard.enemies);
                    return State.Success;
                }

                canAttack = false;
            }
            else
            {
                waitTime += Time.deltaTime;

                if(waitTime >= cooldown)
                {
                    canAttack = true;
                    waitTime = 0.0f;
                }
            }
        }
        else
        {
            blackboard.moveScript.SetDestination(target.transform.position);
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
