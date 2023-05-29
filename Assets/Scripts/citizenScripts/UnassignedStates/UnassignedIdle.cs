using UnityEngine;

public class UnassignedIdle : CitizenBaseState
{
    public override void EnterState(CitizenStateManager Citizen)
    {
        Debug.Log("idle");
        
    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        if (Citizen.toogleFollowKing == true )
        {
            Citizen.SwitchState(Citizen.FollowCitizen);
        }
    }
}
