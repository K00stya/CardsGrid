using System.Collections;
using System.Collections.Generic;
using CardGrid;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu]
public class CardSO : ScriptableObject
{
    public string Name;
    public int StartQuantity;
    public TypeCard Type;
    public Sprite Sprite;
    public Maps ImpactMap;
    public GameObject Effect;
}
