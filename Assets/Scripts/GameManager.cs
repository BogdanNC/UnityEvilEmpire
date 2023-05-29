using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Instance for when reference needed in other scripts
    public static GameManager gm;

    //Move Order Vars
    [SerializeField] private Mesh moveFlagSkin;
    public LayerMask layersToHit;

    public GameObject[] trees;
    public GameObject[] citizens;
    private CitizenStateManager[] AllCitizens;
    private int[] buildList;


    private void Awake()
    {
        gm = this;
    }

    // Update is called once per frame
    void Update()
    {
        bool rightClick = Input.GetMouseButtonDown(1);
        //Debug.Log("running");
        AllCitizens = FindObjectsOfType<CitizenStateManager>();
        Debug.Log("found: " + AllCitizens.Length + " in total ");



        if (rightClick)
        {
            Debug.Log("click!");
            SpawnMoveFlag();
        }
    }

    void SpawnMoveFlag()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 position;

        //Raycast a ray from the mouse to the world, along the direction of the projection
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out RaycastHit hitData, 1000, layersToHit))
        {
            position = hitData.point;

            //Might need to be changed to check a specific attribute
            bool hitBuilding = ( hitData.collider.gameObject.layer == LayerMask.NameToLayer("Buildings") );

            //Check if object already exists
            GameObject obj = GameObject.Find("MoveToFlag");

            if(obj == null)
            {
                //Spawn the Object to move to
                obj = new GameObject("MoveToFlag");
                obj.AddComponent<MeshFilter>();
                obj.AddComponent<MeshRenderer>();
            }

            //Set object properties
            obj.GetComponent<MeshFilter>().mesh = moveFlagSkin;

            if (hitBuilding)
            {
                //Eventualy we need to somehow place the marker around the building's hitbox
                obj.transform.position = hitData.collider.gameObject.transform.position;
            }
            else
            {
                obj.transform.position = position;
            }
        }

    }
    public string targetResource()
    {

        trees = GameObject.FindGameObjectsWithTag("Wood");

        return "Wood";// to implement further
    }
    public void GiveCitizenTasks(CitizenStateManager[] AllCitizens)
    {
        
    }
}
