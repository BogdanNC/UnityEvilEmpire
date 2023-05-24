using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Instance for when reference needed in other scripts
    public static GameManager gm;

    //Move Order Vars
    [SerializeField] private Mesh moveFlagSkin;
    public LayerMask layersToHit;

    void Start()
    {
        Button button = GameObject.Find("Button (3)").GetComponent<Button>();
        button.onClick.AddListener(ClickGatherer);
        Button button2 = GameObject.Find("Button (2)").GetComponent<Button>();
        button2.onClick.AddListener(ClickAttack);
        Button button3 = GameObject.Find("Button (1)").GetComponent<Button>();
        button3.onClick.AddListener(ClickBuilding);
        Button button4 = GameObject.Find("Button").GetComponent<Button>();
        button4.onClick.AddListener(ClickDefend);
    }

    private void Awake()
    {
        gm = this;
    }

    public void ClickGatherer()
    {
        Debug.Log("The gatherer was clicked.");
    }

    public void ClickAttack()
    {
        Debug.Log("The attack was clicked.");
    }

    public void ClickBuilding()
    {
        Debug.Log("The building was clicked.");
    }

    public void ClickDefend()
    {
        Debug.Log("The defend was clicked.");
    }

    // Update is called once per frame
    void Update()
    {
        bool rightClick = Input.GetMouseButtonDown(1);
        //Debug.Log("running");

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
}
