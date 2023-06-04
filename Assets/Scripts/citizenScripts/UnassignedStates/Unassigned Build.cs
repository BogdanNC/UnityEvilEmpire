
using UnityEngine;

public class UnassignedBuild : CitizenBaseState
{
    public override void EnterState(CitizenStateManager Citizen)
    {
        Citizen.transform.LookAt(Citizen.buldingTarget.position);
        Citizen.agent.stoppingDistance = 1.5f;
        Citizen.agent.SetDestination(Citizen.buldingTarget.position);
    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        if (Citizen.toogleFollowKing == true)
        {
            
            Citizen.SwitchState(Citizen.FollowCitizen);
        }
        if (Citizen.buldingTarget != null)
        {
            Citizen.transform.LookAt(Citizen.buldingTarget.position);
            Citizen.agent.stoppingDistance = 1.5f;
            Citizen.agent.SetDestination(Citizen.buldingTarget.position);
        }
        else
        {
            Citizen.hasSmthToBuild = false;
            Citizen.SwitchState(Citizen.IdleCitizen);
        }
    }
}
