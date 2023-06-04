
using UnityEngine;

public class UnassignedGather : CitizenBaseState
{
    GameManager gm = GameManager.gm;
    private string targetResource;
    private GameObject target;
    private float gatherTimer = 5.0f;

    public override void EnterState(CitizenStateManager Citizen)
    {
        targetResource = ResourceManager.GiveJob(Citizen);
        target = findNearestResource(Citizen);
    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        if (target == null)
        {
            target = findNearestResource(Citizen);
        }
        if (Citizen.toogleFollowKing == true)
        {
            ResourceManager.FreeJob(Citizen,target.tag.ToString());
            Citizen.SwitchState(Citizen.FollowCitizen);
        }
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
                amount = resource.Gather();
                ResourceManager.addAmound(Citizen,target.tag.ToString(),amount);
                gatherTimer = 0;
                Debug.Log("i got " + amount + target.tag.ToString());
            }
            else
            {
                gatherTimer += Time.deltaTime;
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
