
using UnityEngine;

public class UnassignFollowKing : CitizenBaseState
{
    public override void EnterState(CitizenStateManager Citizen)
    {
        //Citizen.animator.SetBool("gathering", true);
        Citizen.transform.LookAt(Citizen.kingTransform.position);
        Citizen.agent.stoppingDistance = 15.0f;
        Citizen.agent.SetDestination(Citizen.kingTransform.position);
    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        if (Citizen.isAssignedBuilder == true)
        {
            Citizen.SwitchState(Citizen.FollowBuilder);
        }
        if (Citizen.isAssignedGatherer == true)
        {
            Citizen.SwitchState(Citizen.FollowGatherer);
        }
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
