using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatModeScript : MonoBehaviour
{
    GameManager gm;

    [SerializeField] private const int cheatWoodAmt = 1000;
    [SerializeField] private const int cheatGoldAmt = 1000;
    [SerializeField] private const int cheatFoodAmt = 1000;


    
    private LayerMask mask;

    private void Start()
    {
        mask = LayerMask.GetMask("Ground");
        gm = GameManager.gm;
    }

    void Update()
    {
        //Add Resouces to Stockpile
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1 pressed!");


        }

        //Destroy enemy base building (Win game)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("2 pressed!");

            GameObject enemyBase = GameObject.Find("Central Hub Red");

            Debug.Log("Enemy base destroyed!");

            Destroy(enemyBase);
        }

        //Spawn soldier at cursor position
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("3 pressed!");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hitData, 1000.0f, mask))
            {
                if(mask == (mask | (1 << hitData.collider.gameObject.layer))){
                    //Ground
                    Debug.Log("Unit spawned!");

                    Instantiate(gm.soldiers, hitData.point, Quaternion.identity);
                }
                else
                {
                    Debug.Log("You can only spawn units on the ground!");
                }
            }
        }

        //Spawn citizen at cursor posiion
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("4 pressed!");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitData, 1000.0f, mask))
            {
                if (mask == (mask | (1 << hitData.collider.gameObject.layer)))
                {
                    //Ground
                    Debug.Log("Unit spawned!");

                    Instantiate(gm.civilians, hitData.point, Quaternion.identity);
                }
                else
                {
                    Debug.Log("You can only spawn units on the ground!");
                }
            }
        }

    }
}
