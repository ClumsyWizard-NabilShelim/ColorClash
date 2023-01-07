using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private LayerMask sliceableLayer;
    [SerializeField] private float sliceRadius;
    [SerializeField] private GameObject trail;
    private Action<SliceableObject> onSlice;
    private GameObject previousCol;

    public void Initialize(Action<SliceableObject> callback)
    {
        onSlice = callback;
        trail.SetActive(false);
    }

    public void Slice()
    {
        trail.SetActive(true);
        Collider2D col = Physics2D.OverlapCircle(transform.position, sliceRadius, sliceableLayer);

        if(col && col.gameObject != previousCol)
        {
            previousCol = col.gameObject;
            onSlice(col.GetComponent<SliceableObject>());
        }
    }

    public void StopSlice()
    {
        trail.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sliceRadius);
    }
}
