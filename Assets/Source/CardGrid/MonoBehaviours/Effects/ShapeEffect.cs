using System.Collections;
using System.Collections.Generic;
using CardGrid;
using UnityEngine;

public class ShapeEffect : MonoBehaviour
{
    public ParticleSystem Material;

    public void SetShape(Texture2D shape)
    {
        Material.GetComponent<Renderer>().material.mainTexture = shape;
    }
}
