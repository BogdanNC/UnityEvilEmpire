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

    public GathererFollow FollowGatherer = new GathererFollow();
    public GathererGathering GatheringGatherer = new GathererGathering();
    public GathererIdle IdleGatherer = new GathererIdle();

    public BuilderIdle IdleBuilder = new BuilderIdle();
    public BuilderBuilding BuildingBuilder = new BuilderBuilding();
    public BuilderFollow FollowBuilder = new BuilderFollow();


    public bool toogleFollowKing;
    public bool isAssignedGatherer = false;
    public bool isAssignedBuilder = false;
    public bool hasSmthToBuild = false;
    public Transform buldingTarget = null;
    public Transform kingTransform = null;
    public NavMeshAgent agent;
    public int team;
    public Animator animator;

    void Awake(){
        animator = GetComponent<Animator>();
    }
    
    void Start()
    {
        currentState = IdleCitizen;
        IsKingBlue king = FindObjectOfType<IsKingBlue>();
        this.kingTransform = king.gameObject.transform;
        currentState.EnterState(this);
        Debug.Log(currentState);
        team = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        IsKingBlue king = FindObjectOfType<IsKingBlue>();

        if(king == null)
        {
            return;
        }

        this.kingTransform = king.gameObject.transform;
        currentState.UpdateState(this);
        /*if(currentState == IdleCitizen)
        {
             animator.SetBool("walking", false);
             animator.SetBool("gathering", false);
        }
        else if(currentState == BuildCitizen)
        {
            animator.SetBool("walking", false);
            animator.SetBool("gathering", true);
        }
        else if(currentState == FollowCitizen)
        {
            animator.SetBool("walking", true);
            animator.SetBool("gathering", false);
        }
        else if(currentState == GatherCitizen)
        {
            animator.SetBool("walking", false);
            animator.SetBool("gathering", true);
        }*/
    }
    public void SwitchState(CitizenBaseState newState)
    {
        currentState = newState;
        newState.EnterState(this);
    }
}
