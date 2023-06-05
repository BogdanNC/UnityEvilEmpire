using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GathererFollow : CitizenBaseState
{

    public override void EnterState(CitizenStateManager Citizen)
    {

        Citizen.transform.LookAt(Citizen.kingTransform.position);
        Citizen.agent.stoppingDistance = 15.0f;
        Citizen.agent.SetDestination(Citizen.kingTransform.position);
    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        if (Citizen.isAssignedGatherer == false)
        {
            Citizen.SwitchState(Citizen.FollowCitizen);
        }
        if (Citizen.toogleFollowKing == false)
        {
            Citizen.SwitchState(Citizen.IdleGatherer);
        }
        Citizen.transform.LookAt(Citizen.kingTransform.position);
        Citizen.agent.stoppingDistance = 15.0f;
        Citizen.agent.SetDestination(Citizen.kingTransform.position);
    }
}
