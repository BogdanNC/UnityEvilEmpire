using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSpawn : MonoBehaviour
{

    Vector3 movePoint;
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 vec = new Vector3(hit.point.x, 0,hit.point.z );
            transform.position = vec;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;
        }*/
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 vec = new Vector3(hit.point.x, 0,hit.point.z );
            transform.position = vec;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(prefab,transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
