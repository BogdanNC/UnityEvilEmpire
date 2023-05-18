using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenMovement : MonoBehaviour
{
    GameManager gm = GameManager.gm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject moveFlag = GameObject.Find("MoveToFlag");

        if (moveFlag != null)
        {
            MoveToFlag(moveFlag.transform.position);
        }
    }
    void MoveToFlag(Vector3 position)
    {
        Vector3 moveDir = (position - transform.position).normalized;

        //Ignore movement on y-axis
        moveDir.y = 0;

        if (Vector3.Distance(position, transform.position) < 1f)
        {
            //Arrived at position (or close to it), stop moving
            moveDir = Vector3.zero;
        }

        //Move to position
        GetComponent<MoveVelocity>().SetVelocity(moveDir);
    }
}
