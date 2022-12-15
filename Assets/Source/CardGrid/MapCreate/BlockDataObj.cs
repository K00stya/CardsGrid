using System;
using CardGrid;
using UnityEngine;

[ExecuteInEditMode]
public class BlockDataObj : MonoBehaviour
{
    public CT CardType;
    public int FogQuantity;

    [NonSerialized]
    public Action Validate;

    private void OnValidate()
    {
        Validate?.Invoke();
    }
}
