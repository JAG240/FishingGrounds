using UnityEngine;
using System.Collections.Generic;

public class CatchTable : MonoBehaviour
{
    [SerializeField] private FishBase[] _fish;
    [SerializeField] private float _radius;
    private List<FishBase> _chanceTable;

    public float radius { get; private set; }

    private void Awake()
    {
        foreach(FishBase fish in _fish)
        {
            for(int i = 0; i < fish.rarity; i++)
            {
                _chanceTable.Add(fish);
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
