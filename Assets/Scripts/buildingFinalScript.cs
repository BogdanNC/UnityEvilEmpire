using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingFinalScript : MonoBehaviour
{
    public GameObject obj;
    public bool hasBuilder;
    float time = 0;
    float limit = 5.0f;
    float checkForBuilderTimer = 0.0f;
    void Start()
    {
        CitizenStateManager closestCitizen = findClosestCitizens();
        if (closestCitizen != null)
        {
            hasBuilder = true;
            closestCitizen.hasSmthToBuild = true;
            closestCitizen.buldingTarget = this.gameObject.transform;
        }
        else
        {
            checkForBuilderTimer += Time.deltaTime;
        }
        //also aproach the case where no builder is assigned
        //TO DO
    }
    void Update()
    {
        Collider[] colliderNeighbors = Physics.OverlapSphere(transform.position, 2);
        bool closeBy = false;
        foreach (Collider collider in colliderNeighbors)
        {
            if (collider.gameObject.name.Contains("Citizen"))
            {
                closeBy= true;
                break;
            }
        }
        if(closeBy){
            time += Time.deltaTime;
        }
        if(time >= limit){
            Instantiate(obj, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (hasBuilder == false)
        {
            checkForBuilderTimer += Time.deltaTime;
        }
        if (checkForBuilderTimer > 1.0f)
        {
            CitizenStateManager closestCitizen = findClosestCitizens();
            if (closestCitizen != null)
            {
                hasBuilder = true;
                closestCitizen.hasSmthToBuild = true;
                closestCitizen.buldingTarget = this.gameObject.transform;
            }
            else
            {
                checkForBuilderTimer = 0;
            }
        }

    }
    public CitizenStateManager findClosestCitizens()
    {
        CitizenStateManager[] AllCitizens;
        List<CitizenStateManager> AllIdleCitizens = new List<CitizenStateManager>();
        AllCitizens = FindObjectsOfType<CitizenStateManager>();
        foreach(CitizenStateManager citizen in AllCitizens)
        {
            if(this.gameObject.tag == "BuildTaskTeam1")
            {
                if (citizen.currentState.ToString() == "UnassignedIdle")
                {
                    AllIdleCitizens.Add(citizen);
                }
                if (citizen.currentState.ToString() == "UnassignFollowKing")
                {
                    AllIdleCitizens.Add(citizen);
                }
                if (citizen.currentState.ToString() == "BuilderIdle")
                {
                    AllIdleCitizens.Add(citizen);
                }
                if (citizen.currentState.ToString() == "BuilderFollow")
                {
                    AllIdleCitizens.Add(citizen);
                }
            }
        }
        if (AllIdleCitizens.Count < 1)
        {
            Debug.Log("no empty citizens");
            return null;
        } 
        else
        {
            return getClosestFreeCitizen(AllIdleCitizens);
        }
    }
    public CitizenStateManager getClosestFreeCitizen(List<CitizenStateManager> AllIdleCitizens)
    {
        CitizenStateManager closestCitizen = null;
        float closest = float.PositiveInfinity;

        foreach (CitizenStateManager citizen in AllIdleCitizens)
        {
            Vector3 distance = this.gameObject.transform.position - citizen.gameObject.transform.position;
            float lineDistance = distance.magnitude;
            if (lineDistance < closest)
            {
                closest = lineDistance;
                closestCitizen = citizen;
            }
        }
        return closestCitizen;
    }
}
