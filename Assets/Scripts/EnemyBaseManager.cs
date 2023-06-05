using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBaseManager : MonoBehaviour
{
    private void OnDestroy()
    {
        Debug.Log("Victory!!!!");

        SceneManager.LoadScene("VictoryScreen");
    }
}
