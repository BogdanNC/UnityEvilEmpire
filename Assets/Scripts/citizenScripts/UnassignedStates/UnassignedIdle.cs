using UnityEngine;

public class UnassignedIdle : CitizenBaseState
{
    private float IdleTime;
    public override void EnterState(CitizenStateManager Citizen)
    {
        Debug.Log("idle");
        IdleTime = 0.0f;
    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        IdleTime += Time.deltaTime;
        if (Citizen.toogleFollowKing == true )
        {
            Citizen.SwitchState(Citizen.FollowCitizen);
        }
        
        if (GetIdleTime() > 3.0f)
        {
            Citizen.SwitchState(Citizen.GatherCitizen);
        }
        
    }
    public float GetIdleTime ()
    {
        return IdleTime;
    }
}
