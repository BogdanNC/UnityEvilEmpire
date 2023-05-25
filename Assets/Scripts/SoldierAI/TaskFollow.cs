using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

using BehaviourTree;

public class TaskFollow : Node
{
    private Transform self;
    private Transform king;

    public TaskFollow(Transform self, Transform king)
    {
        this.self = self;
        this.king = king;
    }

    public override NodeState Evaluate()
    {
        Vector3 kingPos = king.position;
        NavMeshAgent agent = self.GetComponent<NavMeshAgent>();

        bool reachedKing = ( Vector3.Distance(self.position, kingPos) < 5f );

       // Debug.Log("REACHED? " + reachedKing);

        if (reachedKing)
        {
            //Can't figure out why the soldier doesn't stop
           // Debug.Log("is stopped 1? " + agent.isStopped);
            agent.isStopped = true;
           // Debug.Log("is stopped 2? " + agent.isStopped);
           // Debug.Log("corner 1: " + agent.path.corners[0]);
           // Debug.Log("corner 2: " + agent.path.corners[1]);
            return NodeState.SUCCESS;
        }

        agent.SetDestination(kingPos);

        return NodeState.RUNNING;
    }
}
