using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CitizenStateManager : MonoBehaviour
{
    
    public CitizenBaseState currentState;
    public UnassignedIdle IdleCitizen = new UnassignedIdle();
    public UnassignedBuild BuildCitizen = new UnassignedBuild();
    public UnassignFollowKing FollowCitizen = new UnassignFollowKing();
    public UnassignedGather GatherCitizen = new UnassignedGather();

    public bool toogleFollowKing;
    public Transform kingTransform;
    public NavMeshAgent agent;
    public int team;

    void Start()
    {
        currentState = IdleCitizen;

        currentState.EnterState(this);

        team = 0;
    }

    // Update is called once per frame
    void Update()
    {

        currentState.UpdateState(this);
    }
    public void SwitchState(CitizenBaseState newState)
    {
        currentState = newState;
        newState.EnterState(this);
    }
    
}
