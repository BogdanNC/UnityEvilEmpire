using UnityEngine;

public abstract class CitizenBaseState
{
    public abstract void EnterState(CitizenStateManager Citizen);

    public abstract void UpdateState(CitizenStateManager Citizen);
}
