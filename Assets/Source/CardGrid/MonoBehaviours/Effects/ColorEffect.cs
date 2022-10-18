using System.Collections;
using System.Collections.Generic;
using CardGrid;
using UnityEngine;

namespace CardGrid
{
    public class ColorEffect : MonoBehaviour
    {
        public ParticleSystem Circle;

        public void SetColor(ColorType cardType)
        {
            switch (cardType)
            {
                case ColorType.Red:
                    Circle.startColor = Color.red;
                    break;
                case ColorType.Blue:
                    Circle.startColor = Color.blue;
                    break;
                case ColorType.Yellow:
                    Circle.startColor = Color.yellow;
                    break;
                case ColorType.Green:
                    Circle.startColor = Color.green;
                    break;
                case ColorType.Purple: 
                    Circle.startColor = Color.magenta;
                    break;
                default:
                    break;
            }
        }
    }
}