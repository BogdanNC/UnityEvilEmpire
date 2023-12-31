using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //Instance for when reference needed in other scripts
    public static GameManager gm;
    
    private const string SOLDIER = "soldier";

    //Move Order Vars
    [SerializeField] private Mesh moveFlagSkin;

    private GameObject enemyBase;
    private GameObject allyKing;

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
   // public BuildListManager.TeamBuildingLists[] teamBuildList = new BuildListManager.TeamBuildingLists[2];

    GameObject button;
    GameObject button2;
    GameObject button3;
    GameObject button4;
    GameObject button5;
    GameObject button6;
    GameObject button7;
    GameObject button8;
    GameObject button9;
    GameObject button10;
    GameObject resourceGUI;

    GameObject mainCamera;
    GameObject secondCamera;
    GameObject selectedBarrack;
    GameObject selectedHouse;
    GameObject newTowerTransparent;
    GameObject newHouseTransparent;
    GameObject newBarrackTransparent;
    GameObject newFlagMarker;

    public GameObject selectedSkin;

    public List<GameObject> selectedUnits;
    

    bool cheatCamera= false;
    bool placingBuildingHouse = false;
    bool placingBuildingBarrack = false;
    bool placingBuildingTower = false;
    bool placingFlagMarker = false;
    bool alreadyActivatedBarrack = false;
    bool alreadyActivatedHouse = false;
    bool buildingActivateButtons= false;
    bool gamePaused = false;

    public TextMeshProUGUI FpsText;
    [SerializeField] private TextMeshProUGUI FoodAmt;
    [SerializeField] private TextMeshProUGUI WoodAmt;
    [SerializeField] private TextMeshProUGUI GoldAmt;

    private float pollingTime = 1f;
    private float time;
    private int frameCount;

    void Start()
    {
        WoodAmt.text = 0.ToString();
        GoldAmt.text = 0.ToString();
        FoodAmt.text = 0.ToString();

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

        button10 = GameObject.Find("Button (9)");
        button10.GetComponent<Button>().onClick.AddListener(ClickFollowKing);

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

        resourceGUI = GameObject.Find("ResourceGUI");

        mainCamera = GameObject.Find("Main Camera");
        secondCamera = GameObject.Find("Camera");

        allyKing = GameObject.Find("King");
        enemyBase = GameObject.Find("Central Hub Red");
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
    
    public void ClickFollowKing()
    {
        Debug.Log("The following King was clicked.");

        foreach(GameObject unit in selectedUnits)
        {
            CitizenStateManager sManager = unit.GetComponent<CitizenStateManager>();
            SoldierBrain sBrain = unit.GetComponent<SoldierBrain>();

            if(sBrain != null)
            {
                sBrain.SetState(SoldierBrain.SoldierState.FOLLOWING);
            }
            else if(sManager != null)
            {
                sManager.toogleFollowKing = !sManager.toogleFollowKing;
            }
        }
    }

    public void ClickCivilian()
    {
        Debug.Log("The civilian was clicked.");
        if (selectedHouse != null)
        {
            GameObject child = selectedHouse.transform.GetChild(8).gameObject;
            Debug.Log(child.transform.position);

            if(team[0].table[2].amountOwned >= 50.0f)
            {
                Instantiate(soldiers, new Vector3(child.transform.position.x, child.transform.position.y, child.transform.position.z), Quaternion.identity);
                team[0].table[2].amountOwned = team[0].table[2].amountOwned - 50.0f;
            }
        }
    }
    
    public void ClickTower()
    {
        //check for resources 
        if (team[0].table[0].amountOwned >= 100)
        {

            if (placingBuildingTower)
            {
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
                //newTowerTransparent.tag = "TranspTower";
            }
        }
    }

    public void ClickHouse()
    {

        if (team[0].table[0].amountOwned >= 50  )
        {
            if (placingBuildingHouse)
            {
                placingBuildingHouse = false;
                Destroy(newHouseTransparent);
            }
            else
            {

                placingBuildingHouse = true;
                placingFlagMarker = false;
                placingBuildingBarrack = false;
                placingBuildingTower = false;

                //Destroy any "blueprint" that might be instanciated
                ClearBlueprints();

                newHouseTransparent = Instantiate(house, Vector3.zero, Quaternion.identity);
                //newHouseTransparent.tag = "TranspHouse";
            }
        }
    }

    public void ClickBarrack()
    {
        if (team[0].table[0].amountOwned >= 100.0f && team[0].table[1].amountOwned >= 50.0f)
        {
            if (placingBuildingBarrack)
            {
                placingBuildingBarrack = false;
                Destroy(newBarrackTransparent);
            }
            else
            {
                placingBuildingBarrack = true;
                placingFlagMarker = false;
                placingBuildingHouse = false;
                placingBuildingTower = false;

                //Destroy any "blueprint" that might be instanciated
                ClearBlueprints();

                newBarrackTransparent = Instantiate(barrack, Vector3.zero, Quaternion.identity);
                //newBarrackTransparent.tag = "TranspBarrack";
            }
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

            if (team[0].table[2].amountOwned >= 50.0f && team[0].table[1].amountOwned >= 50.0f)
            {
                Instantiate(soldiers, new Vector3(child.transform.position.x, child.transform.position.y, child.transform.position.z), Quaternion.identity);
                team[0].table[2].amountOwned = team[0].table[2].amountOwned - 50.0f;
                team[0].table[1].amountOwned = team[0].table[1].amountOwned - 50.0f;
            }
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

    private void UpdateResourceVals()
    {
        int woodAmt = Mathf.FloorToInt(team[0].table[0].amountOwned);
        int goldAmt = Mathf.FloorToInt(team[0].table[1].amountOwned);
        int foodAmt = Mathf.FloorToInt(team[0].table[2].amountOwned);

        WoodAmt.text = woodAmt.ToString();
        GoldAmt.text = goldAmt.ToString();
        FoodAmt.text = foodAmt.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateResourceVals();

        if(allyKing == null)
        {
            Debug.Log("Defeat!!!!!");

            SceneManager.LoadScene("DefeatScreen");
        }
        else if(enemyBase == null)
        {
            Debug.Log("Victory!!!!");

            SceneManager.LoadScene("VictoryScreen");
        }

        HandleInput();
        bool rightClick = Input.GetMouseButtonDown(1);

        if (rightClick)
        {
            Debug.Log("click!");
            SpawnMoveFlag();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            PauseOrResume();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            //Selected soldiers will defend
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(!FpsText.enabled){
                FpsText.enabled = true;
            }else{
                FpsText.enabled = false;
            }
        }
        
        time += Time.deltaTime;
        frameCount ++;
        if(time>= pollingTime){
            int frameRate = Mathf.RoundToInt(frameCount/time);
            FpsText.text = frameRate.ToString() + "FPS";
            time-= pollingTime;
            frameCount =0;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            //Selected soldiers will charge
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //Selected soldiers will follow
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
      
            if (!cheatCamera){
                
                mainCamera.GetComponent<Camera>().enabled = false;
                secondCamera.GetComponent<Camera>().enabled = true;
                resourceGUI.SetActive(false);
                DeactivateAllButtons();
                cheatCamera = true;
                
            }else{ 

                mainCamera.GetComponent<Camera>().enabled = true;
                secondCamera.GetComponent<Camera>().enabled = false;
                resourceGUI.SetActive(true);
                ActivateAllButtons();
                cheatCamera = false;
                
            }
                coroutine = buttonWait();
                StartCoroutine(coroutine);
        }
    }


    void DeactivateAllButtons() {
        button.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
        button10.SetActive(false);
        button5.SetActive(false);
        button6.SetActive(false);
    }

    void ActivateAllButtons() {
        button.SetActive(true);
        button2.SetActive(true);
        button3.SetActive(true);
        button4.SetActive(true);
        button10.SetActive(true);
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
                       if (collider.gameObject.tag != "Map" && !collider.gameObject.name.Contains("Transparent House"))
                    {
                        conflict= true;
                        break;
                    }
                }
                if(!conflict){
                    Instantiate(inProgressHouse, hit.point, Quaternion.identity);
                    team[0].table[0].amountOwned = team[0].table[0].amountOwned - 50.0f;
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
                    
                    if (collider.gameObject.tag != "Map" && !collider.gameObject.name.Contains("Transparent Barrack"))
                    {
                        conflict= true;
                        break;
                    }
                }
                if(!conflict){
                    Instantiate(inProgressBarrack, hit.point, Quaternion.identity);
                    team[0].table[0].amountOwned = team[0].table[0].amountOwned - 100.0f;
                    team[0].table[1].amountOwned = team[0].table[1].amountOwned - 50.0f;
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
                    if (collider.gameObject.tag != "Map" && !collider.gameObject.name.Contains("Transparent Tower"))
                    {

                        conflict= true;
                        break;
                    }
                }
                if(!conflict){
                    Debug.Log("click!");
                    Instantiate(inProgressTower, hit.point, Quaternion.identity);
                    team[0].table[0].amountOwned = team[0].table[0].amountOwned - 100.0f;
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
                if (hitObject.name.Contains("Barrack Blue") && !alreadyActivatedBarrack)
                {
                    Debug.Log("-------------------------------------------------It's working!");
                    alreadyActivatedBarrack = true;
                    selectedBarrack = hitObject;
                    button5.SetActive(true);
                    coroutine = passiveMe(10,5);
                    StartCoroutine(coroutine);
                    alreadyActivatedBarrack = false;
                }
                if (hitObject.name.Contains("House Blue") && !alreadyActivatedHouse)
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

    void ClearBlueprints()
    {
        Destroy(newHouseTransparent);
        Destroy(newBarrackTransparent);
        Destroy(newFlagMarker);
        Destroy(newTowerTransparent);
    }

    void PauseOrResume() {
        if (gamePaused == false) {

             Time.timeScale = 0;
             gamePaused = true;

            if (cheatCamera == false)
                DeactivateAllButtons();

        }
        else {
            
             Time.timeScale = 1;
             gamePaused = false;

            if (cheatCamera == false)
                ActivateAllButtons();

        }
        }
}