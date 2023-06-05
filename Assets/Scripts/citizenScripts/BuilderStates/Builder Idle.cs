using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderIdle : CitizenBaseState
{
    private float IdleTime;
    public override void EnterState(CitizenStateManager Citizen)
    {

    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        if (Citizen.isAssignedBuilder == false)
        {
            Citizen.SwitchState(Citizen.IdleCitizen);
        }

        if (Citizen.toogleFollowKing == true)
        {
            Citizen.SwitchState(Citizen.FollowBuilder);
        }
        if (Citizen.hasSmthToBuild == true)
        {
            Citizen.SwitchState(Citizen.BuildingBuilder);
        }


    }
}
