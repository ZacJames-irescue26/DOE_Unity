using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected_Dictionary : MonoBehaviour
{
    public Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();

    public void AddSelected(GameObject go)
    {
        int id = go.GetInstanceID();

        if(!selectedTable.ContainsKey(id))
        {
            selectedTable.Add(id, go);
            go.AddComponent<Selection_component>();
        }

    }

    public void Deselect(int id)
    {
        Destroy(selectedTable[id].GetComponent<Selection_component>());
        selectedTable.Remove(id);
    }

    public void DeselectAll()
    {
        foreach(KeyValuePair<int, GameObject> pair in selectedTable)
        {
            if(pair.Value != null) 
            {
                Destroy(selectedTable[pair.Key].GetComponent<Selection_component>());
            }
        }
        selectedTable.Clear();
    }

    public bool HaveSelectedUnits()
    {
        if (selectedTable.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
