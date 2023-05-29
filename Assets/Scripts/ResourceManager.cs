using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceManager
{
    public struct ResourceDistribution
    {
        public int nrOfGatherers;
        public string resourceName;
    }
    public struct TeamDistribution
    {
        public ResourceDistribution[] table;
    }
    
    public static string GiveJob(CitizenStateManager Citizen)
    {
        TeamDistribution[] team = GameManager.gm.team;
        int minimum = 5000, minIndex = -1;
        for (int i = 0; i < 3; i++) {
           if (team[Citizen.team].table[i].nrOfGatherers < minimum) {
                minimum = team[Citizen.team].table[i].nrOfGatherers;
                minIndex = i;
            }
        }
        if (minIndex == -1)
        {
            Debug.LogError("no team assigned");
        }
        team[Citizen.team].table[minIndex].nrOfGatherers++;
        return team[Citizen.team].table[minIndex].resourceName;
    }
}
