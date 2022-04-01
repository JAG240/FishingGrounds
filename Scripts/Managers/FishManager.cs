using UnityEngine;
using Unity.Netcode;

/**
 * This class will determine which fish should selected to bit and the chances of biting 
 */

public class FishManager : NetworkBehaviour
{
    private CatchTable[] catchTables;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        catchTables = GetComponentsInChildren<CatchTable>();
    }

    public FishBase GetFish(Vector3 hookPos)
    {
        //This will work for now as there are very few tables. However this might need upgraded when more tables are added. 
        //index 0 should always be a default table
        CatchTable bestTable = catchTables[0];
        float lowestDistance = float.MaxValue;
        foreach(CatchTable table in catchTables)
        {
            float distance = Vector3.Distance(hookPos, table.transform.position);
            if (distance > lowestDistance || distance > table.radius)
                continue;

            lowestDistance = distance;
            bestTable = table;
        }

        /*FishBase fishBase = bestTable.GetFish();
        GameObject newFish = new GameObject(fishBase.name);
        newFish.AddComponent<Fish>().BuildFish(fishBase, hookPos);*/

        return bestTable.GetFish();
    }
}
