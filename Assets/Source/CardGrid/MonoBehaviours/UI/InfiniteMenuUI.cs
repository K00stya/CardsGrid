using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGrid
{
    [Serializable]
    public class InfiniteMenuUI : MonoBehaviour
    {
        public GameObject LevelsContent;
        public Button BackToMenu;
        public TextMeshProUGUI BestScore;
        public Button Continue;
        public Button NewBattleButton;
        public Button LevelButton;
    }
}