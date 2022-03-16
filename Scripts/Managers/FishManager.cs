using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class will determine which fish should selected to bit and the chances of biting 
 */

public class FishManager : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] GameObject tables;
    private CatchTable[] catchTables;

    void Start()
    {
        catchTables = tables.GetComponentsInChildren<CatchTable>();
    }

    public GameObject GetFish(Vector3 bobberPos)
    {
        //This will work for now as there are very few tables. However this might need upgraded when more tables are added. 
        CatchTable bestTable = null;
        float lowestDistance = float.MaxValue;
        foreach(CatchTable table in catchTables)
        {
            float distance = Vector3.Distance(bobberPos, table.transform.position);
            if (distance > lowestDistance)
                continue;

            lowestDistance = distance;
            bestTable = table;
        }

        FishBase fishBase = bestTable.GetFish();
        GameObject newFish = new GameObject(fishBase.name);
        MeshFilter meshFilter = newFish.AddComponent<MeshFilter>();
        meshFilter.mesh = fishBase.mesh;

        return null;
    }
}
