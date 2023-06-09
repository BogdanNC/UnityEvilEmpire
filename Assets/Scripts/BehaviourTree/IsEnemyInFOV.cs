using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class IsEnemyInFOV : ActionNode
{
    private Transform transform;

    protected override void OnStart() {
        transform = context.gameObject.transform;

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        List<GameObject> enemies = FindEnemies();

        if (enemies != null && enemies.Count != 0)
        {
            blackboard.enemies = enemies;
            return State.Success;
        }

        return State.Failure;
    }

    //Returns a List of all enemies present within a radius of the character
    private List<GameObject> FindEnemies()
    {
        List<GameObject> enemies = new();
        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, blackboard.fov);

        foreach (var collider in surroundingColliders)
        {
            //Checks the collider's GameObject's tag
            if (collider.gameObject.CompareTag("Ally"))
            {
                //Add the enemy to the list of enemies
                enemies.Add(collider.gameObject);
            }
        }

        return enemies;
    }
}
