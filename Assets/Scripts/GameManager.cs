using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //Instance for when reference needed in other scripts
    public static GameManager gm;

    private const string SOLDIER = "soldier";
    private const string CITIZEN = "citizen";
    private const string KING = "king";

    //Move Order Vars
    [SerializeField] private Mesh moveFlagSkin;

    public GameObject house;
    public GameObject inProgressHouse;
    public GameObject newHouse;
    public GameObject barrack;
    public GameObject inProgressBarrack;
    public GameObject newBarrack;
    public GameObject tower;
    public GameObject inProgressTower;
    public GameObject newTower;
    public GameObject soldiers;
    public GameObject civilians;
    public GameObject flagMarker;
    public LayerMask layersToHit;
    private CitizenStateManager[] AllCitizens;
    private GameObject[] allPrebuiltBuildings;
    private IEnumerator coroutine;
    private IEnumerator buttonCoroutine;

    public ResourceManager.TeamDistribution[] team = new ResourceManager.TeamDistribution[2];

    GameObject button;
    GameObject button2;
    GameObject button3;
    GameObject button4;
    GameObject button5;
    GameObject button6;
    GameObject button7;
    GameObject button8;
    GameObject button9;

    GameObject mainCamera;
    GameObject secondCamera;
    GameObject selectedBarrack;
    GameObject selectedHouse;
    GameObject selectedTower;
    GameObject newTowerTransparent;
    GameObject newHouseTransparent;
    GameObject newBarrackTransparent;
    GameObject newFlagMarker;

    public List<GameObject> selectedUnits;
    

    bool cheatCamera= false;
    bool placingBuildingHouse = false;
    bool placingBuildingBarrack = false;
    bool placingBuildingTower = false;
    bool placingFlagMarker = false;
    bool alreadyActivatedBarrack = false;
    bool alreadyActivatedHouse = false;
    bool alreadyActivatedTower = false;
    bool buildingActivateButtons= false;
    bool cameraKey = true;

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

        button9 = GameObject.Find("Button (8)");
        button9.GetComponent<Button>().onClick.AddListener(ClickTower);
        button9.SetActive(false);

        mainCamera = GameObject.Find("Main Camera");
        secondCamera = GameObject.Find("Camera");
    }

    private void Awake()
    {
        selectedUnits = new List<GameObject>();
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
    
    public void ClickTower()
    {
       

        if(placingBuildingTower){
            placingBuildingTower = false;
            Destroy(newTowerTransparent);
        }
        else
        {

            placingBuildingHouse = false;
            placingBuildingTower = true;
            placingFlagMarker = false;
            placingBuildingBarrack = false;

            //Destroy any "blueprint" that might be instanciated
            ClearBlueprints();

            newTowerTransparent = Instantiate(tower, Vector3.zero, Quaternion.identity);
            newTowerTransparent.tag = "TranspTower";
        }
    }

    public void ClickHouse()
    {
       

        if(placingBuildingHouse){
            placingBuildingHouse = false;
            Destroy(newHouseTransparent);
        }else{

            placingBuildingHouse = true;
            placingFlagMarker = false;
            placingBuildingBarrack = false;
            placingBuildingTower = false;
           
            //Destroy any "blueprint" that might be instanciated
            ClearBlueprints();

            newHouseTransparent = Instantiate(house, Vector3.zero, Quaternion.identity);
            newHouseTransparent.tag = "TranspHouse";
        }
    }

    public void ClickBarrack()
    {

        if(placingBuildingBarrack){
            placingBuildingBarrack = false;
            Destroy(newBarrackTransparent);
        }
        else
        {
            placingBuildingBarrack= true;
            placingFlagMarker = false;
            placingBuildingHouse = false;
            placingBuildingTower = false;
            
            //Destroy any "blueprint" that might be instanciated
            ClearBlueprints();

            newBarrackTransparent = Instantiate(barrack, Vector3.zero, Quaternion.identity);
            newBarrackTransparent.tag = "TranspBarrack";
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

    private IEnumerator buttonWait()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            cameraKey = true;
        }
        
    }
   

    public void ClickAttack()
    {
        Debug.Log("The attack was clicked.");

        foreach (GameObject unit in selectedUnits)
        {
            if (unit.name.ToLowerInvariant().Contains(SOLDIER))
            {
                //If the unit is a soldier
                unit.GetComponent<SoldierBrain>().SetState(SoldierBrain.SoldierState.CHARGING);
            }
        }
    }

    private void DefendOrder(Vector3 target)
    {
        foreach (GameObject unit in selectedUnits)
        {
            if (unit.name.ToLowerInvariant().Contains(SOLDIER))
            {
                SoldierBrain soldier = unit.GetComponent<SoldierBrain>();
                //If the unit is a soldier
                soldier.SetDefendPos(target);
                soldier.SetState(SoldierBrain.SoldierState.DEFENDING);
            }
        }
    }

    public void ClickDefend()
    {
        GetComponent<SelectionBoxManager>().SetButtonClicked(true);
        
        if (placingFlagMarker)
        {
            placingFlagMarker = false;
            Destroy(newFlagMarker);
        }
        else
        {
            placingFlagMarker = true;
            placingBuildingHouse = false;
            placingBuildingBarrack = false;
            placingBuildingTower = false;

            ClearBlueprints();

            newFlagMarker = Instantiate(flagMarker, Vector3.zero , Quaternion.Euler(Vector3.right * 90));
        }

    }

    public void ClickBuilding()
    {
        Debug.Log("The building was clicked.");
        if(!buildingActivateButtons){
            button7.SetActive(true);
            button8.SetActive(true);
            button9.SetActive(true);
            buildingActivateButtons = true;
        }else{
            button7.SetActive(false);
            button8.SetActive(false);
            button9.SetActive(false);
            buildingActivateButtons = false;
        }
        
        
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
        if (Input.GetKey(KeyCode.U) && cameraKey)
        {
            if(!cheatCamera){
                cameraKey = false;
                mainCamera.GetComponent<Camera>().enabled = false;
                secondCamera.GetComponent<Camera>().enabled = true;
                button.SetActive(false);
                button2.SetActive(false);
                button3.SetActive(false);
                button4.SetActive(false);
                button5.SetActive(false);
                button6.SetActive(false);
                cheatCamera = true;
                
            }else{
                cameraKey = false;
                mainCamera.GetComponent<Camera>().enabled = true;
                secondCamera.GetComponent<Camera>().enabled = false;
                button.SetActive(true);
                button2.SetActive(true);
                button3.SetActive(true);
                button4.SetActive(true);
                cheatCamera = false;
                
            }
                coroutine = buttonWait();
                StartCoroutine(coroutine);
        }
    }

    void HandleInput()
    {
        raytrace();
        

        
        
        

        if(placingBuildingHouse){
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Environment", "Ground");
            if (Physics.Raycast(ray,  out  hit, 100000f, mask)) {
                newHouseTransparent.transform.position = hit.point;
            }
        
            if (Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current != null &&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                Collider[] colliderNeighbors = Physics.OverlapSphere(newHouseTransparent.transform.position, 6);
                bool conflict = false;
                foreach (Collider collider in colliderNeighbors)
                {
                    if (collider.gameObject.tag != "Map" && collider.gameObject.tag != "TranspHouse")
                    {
                        conflict= true;
                        break;
                    }
                }
                if(!conflict){
                    Instantiate(inProgressHouse, hit.point, Quaternion.identity);
                    Destroy(newHouseTransparent);
                    placingBuildingHouse = false;
                }
                conflict = false;
            }
        }

        if(placingBuildingBarrack){
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Environment", "Ground");
            if (Physics.Raycast(ray,  out  hit, 100000.0f, mask)) {
                newBarrackTransparent.transform.position = hit.point;
            }
            if (Input.GetMouseButtonDown(0)&& UnityEngine.EventSystems.EventSystem.current != null &&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {                                  
                Collider[] colliderNeighbors = Physics.OverlapSphere(newBarrackTransparent.transform.position, 10);
                bool conflict = false;
                foreach (Collider collider in colliderNeighbors)
                {
                    if (collider.gameObject.tag != "Map" && collider.gameObject.tag != "TranspBarrack")
                    {
                        conflict= true;
                        break;
                    }
                }
                if(!conflict){
                    Instantiate(inProgressBarrack, hit.point, Quaternion.identity);
                    Destroy(newBarrackTransparent);
                    placingBuildingBarrack = false;
                }
                conflict = false;
            }
        }

        if(placingBuildingTower){
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Environment", "Ground");
           
        
            if (Physics.Raycast(ray,  out  hit, 100000f, mask)) {
                newTowerTransparent.transform.position = hit.point;
            }
        
            if (Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current != null &&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                Collider[] colliderNeighbors = Physics.OverlapSphere(newTowerTransparent.transform.position, 6);
                bool conflict = false;
                foreach (Collider collider in colliderNeighbors)
                {
                    if (collider.gameObject.CompareTag("Map") && collider.gameObject.CompareTag("TranspTower"))
                    {
                        conflict= true;
                        break;
                    }
                }
                if(!conflict){
                    Debug.Log("click!");
                    Instantiate(inProgressTower, hit.point, Quaternion.identity);
                    Destroy(newTowerTransparent);
                    placingBuildingTower = false;
                }
                conflict = false;
            }
        }

        if (placingFlagMarker)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            LayerMask mask = LayerMask.GetMask("Buildings", "Ground");

            if (Physics.Raycast(ray, out RaycastHit hitData, 10000.0f, mask))
            {
                //Hit something
                newFlagMarker.transform.position = hitData.point;
            }

            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<SelectionBoxManager>().SetButtonClicked(false);
                placingFlagMarker = false;

                DefendOrder(newFlagMarker.transform.position);

                //Destroy marker after a short delay
                Destroy(newFlagMarker, 0.2f*Time.deltaTime);
            }
        }
    }

    void SpawnMoveFlag()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 position;

        //Raycast a ray from the mouse to the world, along the direction of the projection
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out RaycastHit hitData, 1000f, layersToHit))
        {
            position = hitData.point;

            //Might need to be changed to check a specific attribute
            bool hitBuilding = ( hitData.collider.gameObject.layer == LayerMask.NameToLayer("Buildings") );
            bool hitGround = (hitData.collider.gameObject.layer == LayerMask.NameToLayer("Ground") );

            //Check if object already exists
            GameObject obj = GameObject.Find("MoveToFlag");
            GameObject kingDestination = GameObject.Find("kingFlag");

            if(obj == null)
            {
                //Spawn the Object to move to
                obj = new GameObject("MoveToFlag");
                obj.AddComponent<MeshFilter>();
                obj.AddComponent<MeshRenderer>();
            }

            if(kingDestination == null)
            {
                //Spawn the Object to move to
                kingDestination = new GameObject("kingFlag");
                kingDestination.AddComponent<MeshFilter>();
                kingDestination.AddComponent<MeshRenderer>();
            }

            //Set object properties
            obj.GetComponent<MeshFilter>().mesh = moveFlagSkin;

            if (hitBuilding)
            {
                //Eventualy we need to somehow place the marker around the building's hitbox
                obj.transform.position = hitData.collider.gameObject.transform.position;
            }
            else if(hitGround)
            {
                kingDestination.transform.position = new Vector3(position.x, 0, position.z);

            }else{
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
                team[i].table[j].amountOwned = 0;
            }
            team[i].table[0].resourceName = "Wood";
            team[i].table[1].resourceName = "Gold";
            team[i].table[2].resourceName = "Food";
        }
    }
    void CheckBuildingList()
    {
        allPrebuiltBuildings = GameObject.FindGameObjectsWithTag("BuildTaskTeam1");
        if (allPrebuiltBuildings.Length > 0)
        {

        }
    }

    void ClearBlueprints()
    {
        Destroy(newHouseTransparent);
        Destroy(newBarrackTransparent);
        Destroy(newFlagMarker);
        Destroy(newTowerTransparent);
    }
}