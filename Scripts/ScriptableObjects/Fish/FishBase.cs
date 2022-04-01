using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fish", order = 53)]
public class FishBase : ScriptableObject
{
    [field: SerializeField] public Mesh mesh { get; private set; }
    [field: SerializeField] public Material material { get; private set; }
    [field: SerializeField] public float maxFatigue { get; private set; }
    [field: SerializeField] public float recoveryRate { get; private set; }
    [field: SerializeField] public float hookStrength { get; private set; }
    [field: SerializeField] public float minSize { get; private set; }
    [field: SerializeField] public float maxSize { get; private set; }
}
