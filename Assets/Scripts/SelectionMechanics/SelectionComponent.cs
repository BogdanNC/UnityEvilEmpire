using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionComponent : MonoBehaviour
{
    private List<Color> colors;

    /* 
     * This component changes the appearance of the units
     * in order to communicate if they are selected or deselected
     */
    private void Start()
    {
        //Add skin for selection
        colors = new();
        Renderer[] comps = gameObject.GetComponentsInChildren<Renderer>();

         foreach(Renderer renderer in comps)
         {
            colors.Add(renderer.material.color);
            renderer.material.color = Color.blue;
         }
    }

    private void OnDestroy()
    {
        //Remove skin added previously (deselection)

        Renderer[] comps = gameObject.GetComponentsInChildren<Renderer>();
        int i = 0;

        foreach (Renderer renderer in comps)
        {
            renderer.material.color = colors[i];
            i++;
        }
    }
}
