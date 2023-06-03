using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovementAnimScript : MonoBehaviour
{
    float speed;
    private MouseMove moveScript;
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;
   
   void Awake(){
        moveScript = GetComponent<MouseMove>();
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
   }

    void Update() {
        
        GameObject flag = GameObject.Find("kingFlag");
        if(flag != null){
            Vector3 vec = new Vector3(flag.transform.position.x,0,flag.transform.position.z);
            Vector3 vec2 = new Vector3(transform.position.x,0,transform.position.z);
             if(Vector3.Distance(vec, vec2) < 1){
                animator.SetBool("walking", false);
                moveScript.Stop();
            }else{
            animator.SetBool("walking", true);
            moveScript.SetDestination(vec);
            moveScript.MoveToPos();
            }
        }else{
            animator.SetBool("walking", false);
            moveScript.Stop();
        }
    }
    
}
