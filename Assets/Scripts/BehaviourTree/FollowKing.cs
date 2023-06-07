using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class FollowKing : ActionNode
{
    private Transform transform;

    private float distance = 10.0f;

    protected override void OnStart() {

        Debug.Log("HERE!!");

        blackboard.king = FindKing();

        Debug.Log("King: " + blackboard.king.transform.position);

        transform = context.gameObject.transform;
        blackboard.moveScript = transform.gameObject.GetComponent<MouseMove>();

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if(blackboard.king == null)
        {
            return State.Failure;
        }

        blackboard.moveScript.SetDestination(blackboard.king.transform.position);

        if(Vector3.Distance(transform.position, blackboard.king.transform.position) <= distance)
        {
            blackboard.moveScript.Stop();
        }
        else
        {
            blackboard.moveScript.MoveToPos();
        }

        return State.Success;
    }

    private GameObject FindKing()
    {
        return GameObject.Find("EnemyKing");
    }
}
