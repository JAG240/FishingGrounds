using UnityEngine;

public class Fish : MonoBehaviour
{
    public float fatigue { get; private set; }
    public bool alive { get; private set; }
    public RodControls rodControls;
    private bool _inFight = false;

    void Update()
    {
        if(_inFight)
        {

        }
    }

    public void BuildFish(FishBase fishbase, Vector3 pos)
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = fishbase.mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = fishbase.material;

        float size = Random.Range(fishbase.minSize, fishbase.maxSize);
        Vector3 scale = transform.localScale;
        scale.Set(size, size, size);
        transform.localScale = scale;

        transform.position = pos;

        Vector3 rot = transform.eulerAngles;
        rot.Set(180f, 0f, 0f);
        transform.eulerAngles = rot;

        rodControls.reelIn += ReelIn;
    }

    public void StartFight()
    {
        //build fighting paths
        _inFight = true;
    }

    private void ReelIn(bool state)
    {
        _inFight = false;
    }
}
