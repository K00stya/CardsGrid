using TMPro;
using UnityEngine;

namespace CardGrid
{
    public class CardGameObject : MonoBehaviour
    {
        public CardState CardState;
        
        public SpriteRenderer Sprite;
        public TextMeshProUGUI QuantityText;
        public GameObject Highlight;
        public TextMeshProUGUI DebugPosition;
        public GameObject Block;
    }
}
