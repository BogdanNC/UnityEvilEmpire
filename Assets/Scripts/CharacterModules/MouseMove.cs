using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseMove : MonoBehaviour
{
    //A reference to the game manager script
    private GameManager gm = GameManager.gm;
    public NavMeshAgent agent;
    private Vector3 destination;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPos();
    }

    public void SetDestination(Vector3 position)
    {
        if(destination != position)
        {
            ResumeMovement();
        }

        destination = position;
    }

    public void MoveToPos()
    {

        //Ignore movement on y-axis
        destination.y = 0;

        //Move to position
        agent.SetDestination(destination);
    }

    public void Stop()
    {
        agent.isStopped = true;
    }

    public void ResumeMovement()
    {
        agent.isStopped = false;
    }
}
