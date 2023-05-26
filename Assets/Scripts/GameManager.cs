using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    //Instance for when reference needed in other scripts
    public static GameManager gm;

    //Move Order Vars
    [SerializeField] private Mesh moveFlagSkin;

    public GameObject soldiers;
    public LayerMask layersToHit;
    private IEnumerator coroutine;

    Button button;
    Button button2;
    Button button3;
    Button button4;
    GameObject button5;
    GameObject button6;

    GameObject mainCamera;
    GameObject secondCamera;
    GameObject selectedBarrack;
    GameObject selectedHouse;

    void Start()
    {
        button = GameObject.Find("Button (3)").GetComponent<Button>();
        button.onClick.AddListener(ClickGatherer);
        button2 = GameObject.Find("Button (2)").GetComponent<Button>();
        button2.onClick.AddListener(ClickAttack);
        button3 = GameObject.Find("Button (1)").GetComponent<Button>();
        button3.onClick.AddListener(ClickBuilding);
        button4 = GameObject.Find("Button").GetComponent<Button>();
        button4.onClick.AddListener(ClickDefend);
        button5 = GameObject.Find("Button (4)");
        button5.GetComponent<Button>().onClick.AddListener(ClickTrain);
        button5.SetActive(false);
        button6 = GameObject.Find("Button (5)");
        button6.GetComponent<Button>().onClick.AddListener(ClickCivilian);
        button6.SetActive(false);

        mainCamera = GameObject.Find("Main Camera");
        secondCamera = GameObject.Find("Camera");
    }

    private void Awake()
    {
        gm = this;
    }

    public void ClickGatherer()
    {
        Debug.Log("The gatherer was clicked.");
    }
    

    public void ClickCivilian()
    {
        Debug.Log("The civilian was clicked.");
        if (selectedHouse != null)
        {
            GameObject child = selectedHouse.transform.GetChild(8).gameObject;
            Debug.Log(child.transform.position);

            Instantiate(soldiers, new Vector3(child.transform.position.x, child.transform.position.y, child.transform.position.z), Quaternion.identity);
            Instantiate(soldiers, new Vector3(child.transform.position.x + 1.38f, child.transform.position.y, child.transform.position.z + 1.38f), Quaternion.identity);
            Instantiate(soldiers, new Vector3(child.transform.position.x - 1.38f, child.transform.position.y, child.transform.position.z - 1.38f), Quaternion.identity);

        }
    }

    public void ClickTrain()
    {
        Debug.Log("The trainer was clicked.");
        //dar spawn de 3 unidades 
        if (selectedBarrack != null)
        {
            GameObject child = selectedBarrack.transform.GetChild(15).gameObject;
            Debug.Log(child.transform.position);
            
            Instantiate(soldiers, new Vector3(child.transform.position.x, child.transform.position.y, child.transform.position.z), Quaternion.identity);
            Instantiate(soldiers, new Vector3(child.transform.position.x+1.38f, child.transform.position.y, child.transform.position.z+1.38f), Quaternion.identity);
            Instantiate(soldiers, new Vector3(child.transform.position.x - 1.38f, child.transform.position.y, child.transform.position.z - 1.38f), Quaternion.identity);

        }
    }
    private IEnumerator passiveMe(int secs, int buttonNum)
    {
        //5 is the soldier, 6 is the civilian
        yield return new WaitForSeconds(secs);
        if(buttonNum == 5)
        {
            button5.SetActive(false);
        }
        else
        {
            button6.SetActive(false);
        }
        
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
        HandleInput();
        bool rightClick = Input.GetMouseButtonDown(1);
        //Debug.Log("running");

        if (rightClick)
        {
            Debug.Log("click!");
            SpawnMoveFlag();
        }
    }

    void HandleInput()
    {
        raytrace();
        

        if (Input.GetKey(KeyCode.N))
        {

            mainCamera.GetComponent<Camera>().enabled = false;
            secondCamera.GetComponent<Camera>().enabled = true;

        }
        if (Input.GetKey(KeyCode.C))
        {
            mainCamera.GetComponent<Camera>().enabled = true;
            secondCamera.GetComponent<Camera>().enabled = false;
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

    void raytrace()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var hitObject = hit.transform.gameObject;
                if (hitObject.tag == "Barrack")
                {
                    Debug.Log("-------------------------------------------------It's working!");
                    selectedBarrack = hitObject;
                    button5.SetActive(true);
                    coroutine = passiveMe(10,5);
                    StartCoroutine(coroutine);
                }
                if (hitObject.tag == "House")
                {
                    Debug.Log("-------------------------------------------------It's working!");
                    selectedHouse = hitObject;
                    button6.SetActive(true);
                    coroutine = passiveMe(10,6);
                    StartCoroutine(coroutine);
                }
            }
        }
    }
}
