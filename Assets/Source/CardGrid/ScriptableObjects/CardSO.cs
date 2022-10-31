using System.Collections;
using System.Collections.Generic;
using CardGrid;
using UnityEngine;

[CreateAssetMenu]
public class CardSO : ScriptableObject
{
    public string Name;
    public CT CardType;
    public TypeCard Type;
    public ShapeType Shape;
    public ColorType ColorType;
    public Sprite Sprite;
    public Texture2D ShapeSprite;
    public Maps ImpactMap;
    public GameObject Effect;
    public GameObject ShapeEffect;
    public AudioClip SoundEffect;
}
