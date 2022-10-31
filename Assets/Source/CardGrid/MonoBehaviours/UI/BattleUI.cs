using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGrid
{
    [Serializable]
    public class SpriteColorType
    {
        public ColorType Color;
        public Sprite Sprite;
    }
    
    [Serializable]
    public class BattleUI : MonoBehaviour
    {
        public Slider LevelProgress;
        public TextMeshProUGUI LevelNumber;
        public Button OpenMenu;
        public Button PlayAgain;
        public BattleMenu BattleMenu;
        public LeftCardsPanel LeftCardsPanel;
        public NewLevelUp NewLevelUp;
        public Require[] Requires = new Require[3];
        public SpriteColorType[] Colors;

        public Sprite GetColorSprite(ColorType colorType)
        {
            foreach (var color in Colors)
            {
                if (color.Color == colorType)
                {
                    return color.Sprite;
                }
            }

            DebugSystem.DebugLog("No color sprite", DebugSystem.Type.Error);
            return null;
        }
    }
}