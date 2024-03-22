using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDecoration : MonoBehaviour
{
    [Header("--- Required References")]
    public MeshRenderer meshRenderer;

    [Header("--- Player Decoration Settings")]
    public FlagType type;
    public Color blue;
    public Color blueGlow;
    public Color red;
    public Color redGlow;
    public float intensity;

    private Material material;

    void Awake()
    {
        material = meshRenderer.materials[1];
        if (type == FlagType.Red)
        {
            material.SetColor("_BaseColor", red);
            material.SetColor("_EmissionColor", redGlow * intensity);
        }
        else if (type == FlagType.Blue)
        {
            material.SetColor("_BaseColor", blue * intensity);
            material.SetColor("_EmissionColor", blueGlow * intensity);
        }
    }
}
