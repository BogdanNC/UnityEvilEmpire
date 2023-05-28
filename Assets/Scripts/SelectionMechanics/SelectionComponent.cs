using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionComponent : MonoBehaviour
{
    /* 
     * This component changes the appearance of the units
     * in order to communicate if they are selected or deselected
     */
    private void Start()
    {
        //Add skin for selection
    }

    private void OnDestroy()
    {
        //Remove skin added previously (deselection)
    }
}
