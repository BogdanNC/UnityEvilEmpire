using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

using BehaviourTree;

public class TaskDefend : Node
{
    private Transform self;
    private Transform waypoint;

    public TaskDefend(Transform self, Transform waypoint)
    {
        this.self = self;
        this.waypoint = waypoint;
    }

    public override NodeState Evaluate()
    {
        Vector3 waypointPos = waypoint.position;
        NavMeshAgent agent = self.GetComponent<NavMeshAgent>();

        bool reachedWP = (Vector3.Distance(self.position, waypointPos) < 1f);

        if (reachedWP)
        {
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }

        agent.SetDestination(waypointPos);

        return NodeState.RUNNING;
    }
}
