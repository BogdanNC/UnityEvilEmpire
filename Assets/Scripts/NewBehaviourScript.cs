using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject enemy;
     public float distanceBetweenObjects = 4f;
     List<Vector3> points = new List<Vector3>();
      
    // Start is called before the first frame update
    void Start()
    {
        ObjectPositioning();
        ObjectSpawning();
    }

    void ObjectPositioning(){
         int count = Random.Range(50, 60);
 
         for(int i = 0; i < count;) {
             float x = Random.Range(-16.0f, 16.0f);
             float z = Random.Range(-10.0f, 10.0f);
             Vector3 point = new Vector3(x, 0, z);

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
            Instantiate(enemy, points[i], Quaternion.Euler(new Vector3(0,Random.Range(-180f, 180.0f),0)));
         }
     }

    /*public GameObject cubePrefab;

    void Start()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-10, 11), 5, Random.Range(-10, 11));
            Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);
        }
    }*/
}
