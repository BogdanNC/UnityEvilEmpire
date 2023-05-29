using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDictionary : MonoBehaviour
{
    public Dictionary<int, GameObject> selectedUnits = new Dictionary<int, GameObject>();

    public void AddSelected(GameObject obj)
    {
        Debug.Log("Select!");

        //Saves the instance id to the dictionary as a key, so no objects repeat
        int id = obj.GetInstanceID();
        if (!selectedUnits.ContainsKey(id))
        {
            selectedUnits.Add(id, obj);
            obj.AddComponent<SelectionComponent>();
        }
    }

    public void DeselectObject(int id)
    {
        Debug.Log("Deselect!");

        //Destroy selection component (deselect)
        DestroyImmediate(selectedUnits[id].GetComponent<SelectionComponent>());
        selectedUnits.Remove(id);
    }

    public void deselectAll()
    {
        Debug.Log("Deselect all!");

        foreach (KeyValuePair<int, GameObject> pair in selectedUnits)
        {
            if(pair.Value != null)
            {
                DestroyImmediate(selectedUnits[pair.Key].GetComponent<SelectionComponent>());
            }
        }

        selectedUnits.Clear();
    }
}
