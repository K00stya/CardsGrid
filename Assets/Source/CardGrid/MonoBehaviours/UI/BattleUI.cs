﻿using System;
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
        public RotateLineButtonsPanel LeftRows;
        public RotateLineButtonsPanel RightRows;
        public Button RotateRight;
        public Button RotateLeft;
        public Slider LevelProgress;
        public TextMeshProUGUI LevelNumber;
        public GameObject RotateButtons;
        public LeftCardsPanel LeftCardsPanel;
        public Button GetAdItems;
        public Button EndAttempt;
        public PanelWithItems NewLevelPanel;
        public PanelWithItems EndItemsReward;

        public Require[] Requires = new Require[4];
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