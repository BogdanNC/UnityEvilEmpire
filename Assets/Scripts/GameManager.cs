using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //Instance for when reference needed in other scripts
    public static GameManager gm;

    //Move Order Vars
    [SerializeField] private Mesh moveFlagSkin;

    public GameObject house;
    public GameObject inProgressHouse;
    public GameObject newHouse;
    public GameObject barrack;
    public GameObject inProgressBarrack;
    public GameObject newBarrack;
    public GameObject soldiers;
    public GameObject civilians;
    public LayerMask layersToHit;
    private CitizenStateManager[] AllCitizens;
    private IEnumerator coroutine;

    public ResourceManager.TeamDistribution[] team = new ResourceManager.TeamDistribution[2];

    GameObject button;
    GameObject button2;
    GameObject button3;
    GameObject button4;
    GameObject button5;
    GameObject button6;
    GameObject button7;
    GameObject button8;

    GameObject mainCamera;
    GameObject secondCamera;
    GameObject selectedBarrack;
    GameObject selectedHouse;
    GameObject newHouseTransparent;
    GameObject newBarrackTransparent;

    List<GameObject> selectedUnits;
    

    bool cheatCamera= false;
    bool placingBuilding = false;
    bool placingBuildingBarrack = false;
    bool alreadyActivatedBarrack = false;
    bool alreadyActivatedHouse = false;
    bool buildingActivateButtons= false;

    void Start()
    {
        selectedUnits = new List<GameObject>();
        InitialDistribution();

        button = GameObject.Find("Button (3)");
        button.GetComponent<Button>().onClick.AddListener(ClickGatherer);

        button2 = GameObject.Find("Button (2)");
        button2.GetComponent<Button>().onClick.AddListener(ClickAttack);

        button3 = GameObject.Find("Button (1)");
        button3.GetComponent<Button>().onClick.AddListener(ClickBuilding);

        button4 = GameObject.Find("Button");
        button4.GetComponent<Button>().onClick.AddListener(ClickDefend);

        button5 = GameObject.Find("Button (4)");
        button5.GetComponent<Button>().onClick.AddListener(ClickTrain);
        button5.SetActive(false);
        button6 = GameObject.Find("Button (5)");
        button6.GetComponent<Button>().onClick.AddListener(ClickCivilian);
        button6.SetActive(false);
        button7 = GameObject.Find("Button (6)");
        button7.GetComponent<Button>().onClick.AddListener(ClickHouse);
        button7.SetActive(false);
        button8 = GameObject.Find("Button (7)");
        button8.GetComponent<Button>().onClick.AddListener(ClickBarrack);
        button8.SetActive(false);

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
    

    public void ClickHouse()
    {
        placingBuilding = true;
        newHouseTransparent = Instantiate(house,new Vector3(0,0,0), Quaternion.identity);
        newHouseTransparent.tag = "TranspHouse";
    }

    public void ClickBarrack()
    {
        placingBuildingBarrack= true;
        newBarrackTransparent = Instantiate(barrack,new Vector3(0,0,0), Quaternion.identity);
        newBarrackTransparent.tag = "TranspBarrack";
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
        if(!buildingActivateButtons){
            button7.SetActive(true);
            button8.SetActive(true);
            buildingActivateButtons = true;
        }else{
            button7.SetActive(false);
            button8.SetActive(false);
            buildingActivateButtons = false;
        }
        
        
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

        if (rightClick)
        {
            Debug.Log("click!");
            SpawnMoveFlag();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            //Selected soldiers will defend
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            //Selected soldiers will charge
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //Selected soldiers will follow
        }
    }

    void HandleInput()
    {
        raytrace();
       //CitizenHandler();
        

        if (Input.GetKey(KeyCode.N))
        {

            mainCamera.GetComponent<Camera>().enabled = false;
            secondCamera.GetComponent<Camera>().enabled = true;
            button.SetActive(false);
            button2.SetActive(false);
            button3.SetActive(false);
            button4.SetActive(false);
            button5.SetActive(false);
            button6.SetActive(false);
            cheatCamera = true;

        }
        if (Input.GetKey(KeyCode.C))
        {
            mainCamera.GetComponent<Camera>().enabled = true;
            secondCamera.GetComponent<Camera>().enabled = false;
            button.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(true);
            button4.SetActive(true);
            cheatCamera = false;
        }

        if(placingBuilding){
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Environment");
        /*if (Physics.Raycast(ray, out hit))
        {
            Instantiate(house,hit.point, Quaternion.identity);
            Vector3 vec = new Vector3(hit.point.x, 0,hit.point.z );
            transform.position = vec;
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        }*/
        //
        
        if (Physics.Raycast(ray,  out  hit, 100000f, mask)) {
            Debug.Log("Touching---------------------------------------");
            
            newHouseTransparent.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Collider[] colliderNeighbors = Physics.OverlapSphere(newHouseTransparent.transform.position, 6);
            bool conflict = false;
            //int LayerIgnoreRaycast = LayerMask.NameToLayer("Environment");
            foreach (Collider collider in colliderNeighbors)
            {
                if (collider.gameObject.tag != "Map" && collider.gameObject.tag != "TranspHouse")
                {
                    conflict= true;
                        Debug.Log("--------------------------------------------------awful");
                      Debug.Log(collider.gameObject.tag);
                    break;
                }
            }
            if(conflict){
                  Debug.Log("--------------------------------------------------awful");
            }
            if(!conflict){
                Debug.Log("--------------------------------------------------alright");
                Instantiate(inProgressHouse,hit.point, Quaternion.identity);
                Destroy(newHouseTransparent);
                placingBuilding = false;
                button6.SetActive(false);
            }
            conflict = false;
        }
        }

        if(placingBuildingBarrack){
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Environment");
        if (Physics.Raycast(ray,  out  hit, 100000f, mask)) {
            newBarrackTransparent.transform.position = new Vector3(hit.point.x, -1.66f, hit.point.z);
        }
        
        if (Input.GetMouseButtonDown(0))
        {                                  
            Collider[] colliderNeighbors = Physics.OverlapSphere(newBarrackTransparent.transform.position, 10);
            bool conflict = false;
            //int LayerIgnoreRaycast = LayerMask.NameToLayer("Environment");
            foreach (Collider collider in colliderNeighbors)
            {
                if (collider.gameObject.tag != "Map" && collider.gameObject.tag != "TranspBarrack")
                {
                    conflict= true;
                        Debug.Log("--------------------------------------------------awful");
                      Debug.Log(collider.gameObject.tag);
                    break;
                }
            }
            if(conflict){
                  Debug.Log("--------------------------------------------------awful");
            }
            if(!conflict){
                Debug.Log("--------------------------------------------------alright");
                Vector3 vec = new Vector3 (hit.point.x, -1.66f, hit.point.z);
                Instantiate(inProgressBarrack,vec, Quaternion.identity);
                Destroy(newBarrackTransparent);
                placingBuildingBarrack = false;
                button6.SetActive(false);
            }
            conflict = false;
        }
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
        if (Input.GetMouseButtonDown(0) && !cheatCamera)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var hitObject = hit.transform.gameObject;
                if (hitObject.CompareTag("Barrack") && !alreadyActivatedBarrack)
                {
                    Debug.Log("-------------------------------------------------It's working!");
                    alreadyActivatedBarrack = true;
                    selectedBarrack = hitObject;
                    button5.SetActive(true);
                    coroutine = passiveMe(10,5);
                    StartCoroutine(coroutine);
                    alreadyActivatedBarrack = false;
                }
                if (hitObject.CompareTag("House") && !alreadyActivatedHouse)
                {
                    Debug.Log("-------------------------------------------------It's working!");
                    alreadyActivatedHouse = true;
                    selectedHouse = hitObject;
                    button6.SetActive(true);
                    coroutine = passiveMe(10,6);
                    StartCoroutine(coroutine);
                    alreadyActivatedHouse = false;
                }
            }
        }
    }
    void CitizenHandler()
    {
        AllCitizens = FindObjectsOfType<CitizenStateManager>();
        foreach (var citizen in AllCitizens)
        {
            //Debug.Log("este in state " + citizen.currentState);
            if (citizen.currentState.ToString() == "UnassignedIdle")
            {
                
            }
        }
    }
    void InitialDistribution()
    {
        for (int i = 0; i <= 1; i++)
        {
            team[i].table = new ResourceManager.ResourceDistribution[3];
            for (int j = 0; j <= 2; j++)
            {
                team[i].table[j].nrOfGatherers = 0;
            }
            team[i].table[0].resourceName = "Wood";
            team[i].table[1].resourceName = "Gold";
            team[i].table[2].resourceName = "Food";
        }
    }
    
    
}