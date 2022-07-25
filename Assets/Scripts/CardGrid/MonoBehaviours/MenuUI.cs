using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGrid
{
    [Serializable]
    public class MenuUI : MonoBehaviour
    {
        public GameObject GameObject;
        public GameObject MainMenu;
        public GameObject Levels;
        public Button BackToMenu;
        public TextMeshProUGUI BestScore;
        public Button Continue;
        public Button NewBattleButton;
        public Button StartLevel;
        public Slider VolumeSlider;
        public TMP_Dropdown LanguageDropdown;
    }
}