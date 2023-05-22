using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    private string assignedJob;
    private bool hasJobAssigned = false;
    private bool idle = true;
    private bool idleOrder = false;
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
    public void Action(GameManager gm)
    {
        if (hasJobAssigned == true) {
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
                } else {
                    //maybe set to idle
                    //idle = true;
                }
            }
        } else {// no job assigned case
            if (idle == true) {
                Idle();
            }
            bool hasSmthToBuild;
            hasSmthToBuild = CheckBuildingList(gm);
            if (hasSmthToBuild == true)
            {
                Build(gm);
            } else {
                Gather(gm);
            }
        }
    }
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

}
public class CitizenBrain : MonoBehaviour
{
    GameManager gm = GameManager.gm;
    public State state;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        state.Action(gm);
    }

}
