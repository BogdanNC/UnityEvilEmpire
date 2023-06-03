using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovementScript : MonoBehaviour
{
    float speed;
    private MouseMove moveScript;
    private Animator animator;
   
   void Awake(){
        moveScript = GetComponent<MouseMove>();
        //animator = GetComponent<Animator>();
   }

    void Update() {
        
        GameObject flag = GameObject.Find("kingFlag");
        if(flag != null){
            Vector3 vec = new Vector3(flag.transform.position.x,0,flag.transform.position.z);
           // animator.SetBool("walking", true);
            moveScript.SetDestination(vec);
            moveScript.MoveToPos();
           /* 
            
            if(Vector3.Distance (vec, transform.position) > 2){

                speed = 3;
                float step = speed * Time.deltaTime;
                transform.LookAt(vec); 
                transform.position = Vector3.MoveTowards(transform.position, vec, step);
                
            }else{
                speed=0;
            }
            */
        }else{
            //animator.SetBool("walking", false);
            moveScript.Stop();
        }
    }
}
