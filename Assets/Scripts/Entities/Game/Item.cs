using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("--- Item Settings")]
    public PowerUp itemType;
    public GameObject itemPrefab;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Animator animator;

    public delegate void OnPickedUp();
    public OnPickedUp onPickedUp;

    public Item(Mesh mesh, PowerUp type)
    {
        itemType = type;
        meshFilter.mesh = mesh;
    }

    private void OnDisable()
    {
        onPickedUp?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onPickedUp?.Invoke();
            animator.SetTrigger("onPickup");
        }
    }

    public void Disable()
    {
        enabled = false;
    }

    public void Enable()
    {
        enabled = true;
    }
}
