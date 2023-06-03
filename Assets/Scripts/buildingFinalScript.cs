using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingFinalScript : MonoBehaviour
{
    public GameObject obj;
    float time = 0;
    float limit = 5.0f; 
    void Update()
    {
        Collider[] colliderNeighbors = Physics.OverlapSphere(transform.position, 15);
        bool closeBy = false;
        foreach (Collider collider in colliderNeighbors)
        {
            if (collider.gameObject.name.Contains("Citizen"))
            {
                closeBy= true;
                break;
            }
        }
        if(closeBy){
            time += Time.deltaTime;
        }
        if(time >= limit){
            Instantiate(obj, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
