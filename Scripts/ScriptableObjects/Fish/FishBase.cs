using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fish", order = 53)]
public class FishBase : ScriptableObject
{

    [SerializeField] private Mesh _mesh;
    [SerializeField] private float _maxFatigue;
    [SerializeField] private int _rarity;
    [SerializeField] private float _recoveryRate;

    public Mesh mesh { get; private set; }
    public float maxFatigue { get; private set; }
    public int rarity { get; private set; }
    public float recoveryRate { get; private set; }

    private void OnEnable()
    {
        mesh = _mesh;
        maxFatigue = _maxFatigue;
        rarity = _rarity;
        recoveryRate = _recoveryRate;
    }
}
