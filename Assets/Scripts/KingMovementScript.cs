using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovementScript : MonoBehaviour
{
   float speed;

   
    void Update() {
        
        GameObject flag = GameObject.Find("MoveToFlag");
        if(flag != null){
            Vector3 vec = new Vector3(flag.transform.position.x,0,flag.transform.position.z);
            
            if(Vector3.Distance (vec, transform.position) > 1){
                speed = 2;
                float step = speed * Time.deltaTime;
                transform.LookAt(vec); 
                transform.position = Vector3.MoveTowards(transform.position, vec, step);
                
            }else{
                speed=0;
            }
        }
    }
}
