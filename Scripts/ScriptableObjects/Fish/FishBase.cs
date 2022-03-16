using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fish", order = 53)]
public class FishBase : ScriptableObject
{

    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    [SerializeField] private float _maxFatigue;
    [SerializeField] private float _recoveryRate;

    public Mesh mesh { get; private set; }
    public Material material { get; private set; }
    public float maxFatigue { get; private set; }
    public float recoveryRate { get; private set; }

    private void OnEnable()
    {
        mesh = _mesh;
        material = _material;
        maxFatigue = _maxFatigue;
        recoveryRate = _recoveryRate;
    }
}
