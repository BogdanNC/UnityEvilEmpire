using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuildListManager
{
    /*
    public struct BuildingAndBuilder
    {
        public GameObject building;
        public bool hasBuilderAssigned;

    }
    public struct TeamBuildingLists
    {
        public BuildingAndBuilder[] buildingList;
    }
    private static void AddToBuildingList(BuildingAndBuilder[] buildList, GameObject building)
    {
        int i = 0;
        while (i < buildList.Length)
        {
            if (buildList[i].building == null)
            {
                buildList[i].building = building;
                i = buildList.Length;
            }
            i++;
        }
    }
    private static void PopBuildingList(BuildingAndBuilder[] buildList, GameObject building)
    {
        int i = 0;
        while (i < buildList.Length)
        {
            if (buildList[i].building == building)
            {
                buildList[i].building = null;
                i = buildList.Length;
            }
            i++;
        }
    }
    public static void NotifyNewConstruction(GameObject newBuilding)
    {
        TeamBuildingLists[] team = GameManager.gm.teamBuildList;
        if (newBuilding.tag == "BuildTaskTeam1")
        {
            AddToBuildingList(team[0].buildingList, newBuilding);
            foreach (BuildingAndBuilder build in team[0].buildingList)
            {
                if (build.hasBuilderAssigned == false)
                {
                    
                }
            }
        }
        if (newBuilding.tag == "BuildTaskTeam2")
        {
            Debug.Log("team2 build order");
        }
    }
    */

}
