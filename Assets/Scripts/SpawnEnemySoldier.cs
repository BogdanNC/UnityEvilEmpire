using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemySoldier : MonoBehaviour
{
    [SerializeField] private GameObject enemySoldier;
    [SerializeField] private Transform spawnPoint;

    private GameManager gm;

    private float cooldown = 3.0f;
    private float waitTime = 0.0f;
    private bool canSpawn = true;

    private void Start()
    {
        gm = GameManager.gm;
    }

    // Update is called once per frame
    void Update()
    {
        ResourceManager.ResourceDistribution[] stockpile = gm.team[1].table;

        if (canSpawn)
        {
            if(stockpile[1].amountOwned >= 50.0f && stockpile[2].amountOwned >= 50.0f)
            {
                canSpawn = false;
                Instantiate(enemySoldier, spawnPoint.position, spawnPoint.rotation);
                stockpile[1].amountOwned -= 50.0f;
                stockpile[2].amountOwned -= 50.0f;
            }
        }
        else
        {
            waitTime += Time.deltaTime;

            if(waitTime >= cooldown)
            {
                canSpawn = true;
                waitTime = 0;
            }
        }
        
    }
}
