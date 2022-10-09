using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGrid
{
    public class LevelsMenu : MonoBehaviour
    {
        public TextMeshProUGUI StarsQuantity;
        public Button BackToMenu;
        public Transform LevelsContent;
        public Transform LevelGroup;
        public LevelCell LevelButton;

        public Sprite Star;
    }
}
