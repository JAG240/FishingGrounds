using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float _fatigue { get; private set; }

    public void BuildFish(FishBase fishbase, Vector3 pos)
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = fishbase.mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = fishbase.material;

        transform.position = pos;
    }
}
