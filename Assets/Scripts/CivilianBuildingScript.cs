using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class civilianBuildingScript : MonoBehaviour 
{

    public GameObject obj;
    int valuePercentage = 0;
    int finalPercentage = 100;
    // Start is called before the first frame update

    public bool progress()
    {
        
        valuePercentage += 10;
        bool isCompleted = valuePercentage == finalPercentage;
        if(isCompleted){
            Instantiate(obj, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        return isCompleted;
    }
}
