using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GathererGathering : CitizenBaseState
{
    GameManager gm = GameManager.gm;
    private string targetResource;
    private GameObject target;
    private float gatherTimer = 5.0f;

    public override void EnterState(CitizenStateManager Citizen)
    {
        targetResource = ResourceManager.GiveJob(Citizen);
        target = findNearestResource(Citizen);
        if (target == null)
        {
            Debug.Log("there is no resource of this type left ");
            ResourceManager.FreeJob(Citizen, target.tag.ToString());
            Citizen.SwitchState(Citizen.IdleCitizen);
        }
    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        if (Citizen.isAssignedGatherer == false)
        {
            ResourceManager.FreeJob(Citizen, target.tag.ToString());
            Citizen.SwitchState(Citizen.IdleCitizen);
        }
        if (target == null)
        {
            target = findNearestResource(Citizen);
            if (target == null)
            {
                Debug.Log("there is no resource of this type left ");
                ResourceManager.FreeJob(Citizen, target.tag.ToString());
                Citizen.SwitchState(Citizen.IdleCitizen);
            }
        }
        if (Citizen.toogleFollowKing == true)
        {

            ResourceManager.FreeJob(Citizen, target.tag.ToString());
            Citizen.SwitchState(Citizen.FollowGatherer);
        }
        if (target != null)
        {
            Citizen.transform.LookAt(target.transform.position);
            Citizen.agent.stoppingDistance = 5.0f;
            Citizen.agent.SetDestination(target.transform.position);
            Vector3 distanceToTarget;
            distanceToTarget = Citizen.transform.position - target.transform.position;


            ResourceClass resource = (ResourceClass)target;

            if (distanceToTarget.magnitude <= 5.5f)

            {
                int amount;

                if (gatherTimer > 6.0f)
                {
                    Citizen.animator.SetBool("walking", false);
                    Citizen.animator.SetBool("gathering", true);
                    amount = resource.Gather();
                    ResourceManager.addAmound(Citizen, target.tag.ToString(), amount);
                    gatherTimer = 0;
                    Debug.Log("i got " + amount + target.tag.ToString());
                }
                else
                {
                    gatherTimer += Time.deltaTime;
                }
            }
        }
    }
    private GameObject findNearestResource(CitizenStateManager Citizen)
    {
        GameObject[] allResources = GameObject.FindGameObjectsWithTag(targetResource);
        float closest = Mathf.Infinity;
        GameObject target = null;
        foreach (var individualResource in allResources)
        {
            Vector3 diff = Citizen.transform.position - individualResource.transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < closest)
            {
                target = individualResource;
                closest = curDistance;
            }
        }
        return target;
    }
}
