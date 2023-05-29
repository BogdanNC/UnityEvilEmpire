using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CitizenBrain : MonoBehaviour
{
    GameManager gm = GameManager.gm;
    public Transform kingPozition;
    public State state;
    void Start()
    {
        state.FindKing(kingPozition);
        state.FollowUnfollow(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        
        state.Action(gm);
        
    }

}
