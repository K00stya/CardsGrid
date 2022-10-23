using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGrid
{
    public class BattleMenu : MonoBehaviour
    {
        public TextMeshProUGUI Lable;
        public Image Image;
        public Sprite TrophySprite;
        public Sprite DefeatSprite;
        public Sprite MenuSprite;

        public Button Close;
        public Difficult[] Difficults;

        public Button PlayAgain;
        public Button NextDifficult;
        public Button NextLevel;
        public Button LevelMenu;
        public Button ToMenu;
    }
}
