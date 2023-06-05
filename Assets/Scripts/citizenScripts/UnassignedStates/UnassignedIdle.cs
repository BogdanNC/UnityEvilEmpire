using UnityEngine;
using System.Collections;

public class UnassignedIdle : CitizenBaseState
{
    private float IdleTime;
     

    public override void EnterState(CitizenStateManager Citizen)
    {
        //Debug.Log("idle");
        IdleTime = 0.0f;
        Citizen.animator.SetBool("walking", true);
    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        IdleTime += Time.deltaTime;
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
