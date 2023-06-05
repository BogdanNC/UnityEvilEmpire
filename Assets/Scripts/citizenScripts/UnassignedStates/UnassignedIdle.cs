using UnityEngine;
using System.Collections;

public class UnassignedIdle : CitizenBaseState
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
        if (Citizen.isAssignedGatherer == true)
        {
            Citizen.SwitchState(Citizen.IdleGatherer);
        }
        if (Citizen.isAssignedBuilder == true)
        {
            Citizen.SwitchState(Citizen.IdleBuilder);
        }

        if (Citizen.toogleFollowKing == true )
        {
            Citizen.SwitchState(Citizen.FollowCitizen);
        }
        if (Citizen.hasSmthToBuild == true)
        {
            Citizen.SwitchState(Citizen.BuildCitizen);
        }
        
        
        if (GetIdleTime() > 5.0f)
        {
            Citizen.SwitchState(Citizen.GatherCitizen);
        }
        
    }
    public float GetIdleTime ()
    {
        return IdleTime;
    }
    
}
