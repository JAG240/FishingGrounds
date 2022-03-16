using UnityEngine;
using System.Collections.Generic;

public class CatchTable : MonoBehaviour
{
    [SerializeField] private FishOdds[] _fishTable;
    [SerializeField] private float _radius;
    private List<FishBase> _chanceTable = new List<FishBase>();

    public float radius { get; private set; }

    [System.Serializable]
    private struct FishOdds
    {
        public FishBase fishBase;
        public int rarity;
    }

    private void Awake()
    {
        foreach(FishOdds fish in _fishTable)
        {
            for(int i = 0; i < fish.rarity; i++)
            {
                _chanceTable.Add(fish.fishBase);
            }
        }
    }

    private void OnEnable()
    {
        radius = _radius;
    }

    public FishBase GetFish()
    {
        int fishIndex = Random.Range(0, _chanceTable.Count);
        return _chanceTable[fishIndex];
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
    #endif
}
