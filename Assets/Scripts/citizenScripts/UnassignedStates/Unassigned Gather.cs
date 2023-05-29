
using UnityEngine;

public class UnassignedGather : CitizenBaseState
{
    GameManager gm = GameManager.gm;
    public override void EnterState(CitizenStateManager Citizen)
    {
        string targetResource;
        Debug.Log("bam");
        targetResource = ResourceManager.GiveJob(Citizen);
        Debug.Log(targetResource);

    }

    public override void UpdateState(CitizenStateManager Citizen)
    {
        
    }
}
