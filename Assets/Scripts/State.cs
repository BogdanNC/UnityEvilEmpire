using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State : MonoBehaviour
{
    
    private string assignedJob;
    private bool hasJobAssigned = false;
    private bool idle = true;
    private bool idleOrder = false;
    private bool togleKing = false;
    public float speed = 1.5f;
    private Vector3 destination;
    private Transform kingTransform;
    public NavMeshAgent agent;
    //functions that work as getters and setters:
    public void AssignJob(string job)
    {
        this.assignedJob = job;
        this.hasJobAssigned = true;
    }
    public void UnassignJob()
    {
        this.hasJobAssigned = false;
    }
    public void IdleOrder(bool order)
    {
        this.idleOrder = order;
    }
    public void FollowUnfollow(bool followKing)
    {
        this.togleKing = followKing;
    }
    public void FindKing(Transform kingPosition)
    {
        this.kingTransform = kingPosition;
    }
    // the main function of a citizen
    public void Action(GameManager gm)
    {
        if (togleKing == true)
        {
            FollowKing();
        }
        else
        {
            if (hasJobAssigned == true)
            {
                if (idle == true)
                {
                    Idle();
                }
                if (assignedJob == "gatherer")
                {
                    if (idleOrder == true)
                    {
                        IdleOrder();
                    }
                    Gather(gm);
                }
                if (assignedJob == "builder")
                {
                    if (idleOrder == true)
                    {
                        IdleOrder();
                    }
                    bool hasSmthToBuild;
                    hasSmthToBuild = CheckBuildingList(gm);
                    if (hasSmthToBuild == true)
                    {
                        Build(gm);
                    }
                    else
                    {
                        //maybe set to idle
                        //idle = true;
                    }
                }
            }
            else
            {// no job assigned case
                if (idle == true)
                {
                    Idle();
                }
                bool hasSmthToBuild;
                hasSmthToBuild = CheckBuildingList(gm);
                if (hasSmthToBuild == true)
                {
                    Build(gm);
                }
                else
                {
                    Gather(gm);
                }
            }
        }
    }
    //individual functions
    public void Gather(GameManager gm)
    {
        string resourceTag;
        resourceTag = gm.targetResource();// to implement further


    }
    public bool CheckBuildingList(GameManager gm)
    {
        // to implement further
        return true;
    }
    public void Build(GameManager gm)
    {
        // to implement further
    }
    public void Idle()
    {
        // to implement further
    }
    public void IdleOrder()
    {
        //to implement further
    }
    private void FollowKing()
    {
        transform.LookAt(kingTransform.position);
        agent.stoppingDistance = 15.0f;
        agent.SetDestination(kingTransform.position);
        

    }
    public void SetDestination(Vector3 position)
    {
        destination = position;
    }
    
}
