using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GathererIdle : CitizenBaseState
{
    private float IdleTime;
    public override void EnterState(CitizenStateManager Citizen)
    {
        //Debug.Log("idle");
        IdleTime = 0.0f;
    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        IdleTime += Time.deltaTime;
        if (Citizen.isAssignedGatherer == false)
        {
            Citizen.SwitchState(Citizen.IdleCitizen);
        }
        if (Citizen.toogleFollowKing == true)
        {
            Citizen.SwitchState(Citizen.FollowCitizen);
        }


        if (GetIdleTime() > 2.0f)
        {
            Citizen.SwitchState(Citizen.GatheringGatherer);
        }

    }
    public float GetIdleTime()
    {
        return IdleTime;
    }

}
