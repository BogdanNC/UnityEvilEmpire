using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Instance for when reference needed in other scripts
    public static GameManager gm;

    //Move Order Vars
    [SerializeField] private Mesh moveFlagSkin;
    public LayerMask layersToHit;

    private void Awake()
    {
        gm = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool rightClick = Input.GetMouseButtonDown(1);
        Debug.Log("running");

        if (rightClick)
        {
            Debug.Log("click!");
            MoveOrder();
        }
    }

    void MoveOrder()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 position;

        //Raycast a ray from the mouse to the world, along the direction of the projection
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out RaycastHit hitData, 1000, layersToHit))
        {
            position = hitData.point;

            //Spawn the Object to move to
            GameObject obj = GameObject.Find("MoveToFlag");

            if(obj == null)
            {
                obj = new GameObject("MoveToFlag");
            }

            obj.AddComponent<MeshFilter>();
            obj.AddComponent<MeshRenderer>();
            obj.GetComponent<MeshFilter>().mesh = moveFlagSkin;
            obj.transform.position = position;
        }

    }
}
