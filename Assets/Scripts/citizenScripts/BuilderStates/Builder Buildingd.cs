using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderBuilding : CitizenBaseState
{
    public override void EnterState(CitizenStateManager Citizen)
    {
        Citizen.transform.LookAt(Citizen.buldingTarget.position);
        Citizen.agent.stoppingDistance = 4.5f;
        Citizen.agent.SetDestination(Citizen.buldingTarget.position);
    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        if (Citizen.isAssignedBuilder == false)
        {
            Citizen.SwitchState(Citizen.BuildCitizen);
        }

        if (Citizen.toogleFollowKing == true)
        {
            Citizen.SwitchState(Citizen.FollowBuilder);
        }
        if (Citizen.buldingTarget != null)
        {
            Citizen.transform.LookAt(Citizen.buldingTarget.position);
            Citizen.agent.stoppingDistance = 4.5f;
            Citizen.agent.SetDestination(Citizen.buldingTarget.position);
        }
        else
        {
            Citizen.hasSmthToBuild = false;
            Citizen.SwitchState(Citizen.IdleBuilder);
        }
    }
}
