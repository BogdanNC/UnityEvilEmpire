using UnityEngine;

public class SoldierScript : MonoBehaviour
{
    GameManager gm = GameManager.gm;

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

        if(Vector3.Distance(position, transform.position) < 1f)
        {
            //Arrived at position (or close to it), stop moving
            moveDir = Vector3.zero;
        }

        //Move to position
        GetComponent<MoveVelocity>().SetVelocity(moveDir);
    }
}
