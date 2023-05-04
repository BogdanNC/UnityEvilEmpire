using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject enemy;
     public float distanceBetweenObjects = 2f;
     List<Vector3> points = new List<Vector3>();
      
    // Start is called before the first frame update
    void Start()
    {
        ObjectPositioning();
        ObjectSpawning();
    }

    void ObjectPositioning(){
         int count = Random.Range(2, 6);
 
         for(int i = 0; i < count;) {
             float x = Random.Range(-2.0f, 2.0f);
             float y = Random.Range(-4.0f, 4.0f);
             Vector3 point = new Vector3(x, y, 0);

             if (points.Count==0){
                 points.Add(point);
                 i++;
                 continue;
             }

             for(int j = 0; j < points.Count; j++){

                 if ((point-points[j]).sqrMagnitude > distanceBetweenObjects * distanceBetweenObjects){
                     if (j==points.Count-1){
                         points.Add(point);
                         i++;
                     }
                     continue;
                 }
                 break;
             }
         }
     }
 
     void ObjectSpawning(){
         for (int i = 0; i < points.Count; i++){
             Instantiate(enemy, points[i], Quaternion.identity);
         }
     }

    // Update is called once per frame
    void Update()
    {
        
    }
}
