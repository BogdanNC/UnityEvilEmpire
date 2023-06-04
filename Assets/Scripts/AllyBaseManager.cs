using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllyBaseManager : MonoBehaviour
{
    private void OnDestroy()
    {
        Debug.Log("Defeat!!!!!");

        SceneManager.LoadScene("DefeatScreen");
    }
}
