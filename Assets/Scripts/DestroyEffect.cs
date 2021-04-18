using System;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    [SerializeField] public float ttl = 1;
    public void Start()
    {
        Destroy(gameObject, ttl);
    }
}