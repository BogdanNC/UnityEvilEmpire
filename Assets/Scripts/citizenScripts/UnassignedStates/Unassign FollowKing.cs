
using UnityEngine;

public class UnassignFollowKing : CitizenBaseState
{
    public override void EnterState(CitizenStateManager Citizen)
    {
        Citizen.animator.SetBool("walking", true);
        Citizen.transform.LookAt(Citizen.kingTransform.position);
        Citizen.agent.stoppingDistance = 15.0f;
        Citizen.agent.SetDestination(Citizen.kingTransform.position);
    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        if (Citizen.hasSmthToBuild == true)
        {
            Citizen.toogleFollowKing = false;
            Citizen.SwitchState(Citizen.BuildCitizen);
        }
        if (Citizen.toogleFollowKing == false)
        {
            Citizen.SwitchState(Citizen.IdleCitizen);
        }
        Citizen.transform.LookAt(Citizen.kingTransform.position);
        Citizen.agent.stoppingDistance = 15.0f;
        Citizen.agent.SetDestination(Citizen.kingTransform.position);
    }
}
