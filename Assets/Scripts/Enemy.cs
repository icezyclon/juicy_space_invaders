using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool aboutToDestroy = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log($"Enemy - OnTriggerEnter2D: {other.gameObject.name}");
    }
}
