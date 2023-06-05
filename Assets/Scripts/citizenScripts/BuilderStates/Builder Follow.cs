using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderFollow : CitizenBaseState
{
    public override void EnterState(CitizenStateManager Citizen)
    {

        Citizen.transform.LookAt(Citizen.kingTransform.position);
        Citizen.agent.stoppingDistance = 15.0f;
        Citizen.agent.SetDestination(Citizen.kingTransform.position);
    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        if (Citizen.isAssignedBuilder == false)
        {
            Citizen.SwitchState(Citizen.FollowCitizen);
        }

        if (Citizen.hasSmthToBuild == true)
        {
            Citizen.toogleFollowKing = false;
            Citizen.SwitchState(Citizen.BuildingBuilder);
        }
        if (Citizen.toogleFollowKing == false)
        {
            Citizen.SwitchState(Citizen.IdleBuilder);
        }
        Citizen.transform.LookAt(Citizen.kingTransform.position);
        Citizen.agent.stoppingDistance = 15.0f;
        Citizen.agent.SetDestination(Citizen.kingTransform.position);
    }
}
