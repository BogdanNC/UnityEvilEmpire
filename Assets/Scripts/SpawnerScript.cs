using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject resources;
    public float distanceBetweenObjects = 4f;
    List<Vector3> points = new List<Vector3>();
    public float height;
    public float scale;
    public int min;
    public int max;

    // Start is called before the first frame update 
    void Start()
    {
        ObjectPositioning(); 
        for (int i = 0; i < points.Count; i++)
        {
            if (validPlace(points[i])) {
                GameObject resource = Instantiate(resources, points[i], Quaternion.Euler(new Vector3(0, Random.Range(-180f, 180.0f), 0)));
                resource.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }

    void ObjectPositioning()
    {
        int count = Random.Range(min, max);

        for (int i = 0; i < count;)
        {
            float x = Random.Range(-16.0f, 16.0f);
            float z = Random.Range(-10.0f, 10.0f);
            Vector3 point = new Vector3(x + transform.position.x, height, z + transform.position.z); 

            if (points.Count == 0)
            {
                points.Add(point);
                i++;
                continue;
            }

           
            for (int j = 0; j < points.Count; j++)
            {

                if ((point - points[j]).sqrMagnitude > distanceBetweenObjects * distanceBetweenObjects)
                {
                    if (j == points.Count - 1)
                    {
                        points.Add(point);
                        i++;
                    }
                    continue; 
                }
                break;
            }
        }
    }

    bool validPlace(Vector3 pos)
    {
        if ( (pos.z < -85.0f) || (pos.z > 85.0f) || (pos.x < -165.0f) || (pos.x > 165.0f))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /*void ObjectSpawning()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Instantiate(berryBush, points[i], Quaternion.Euler(new Vector3(0, Random.Range(-180f, 180.0f), 0)));
        }
    }*/

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
