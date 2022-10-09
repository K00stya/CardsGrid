using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGrid
{
    [Serializable]
    public class BattleUI : MonoBehaviour
    {
        public TextMeshProUGUI Score;
        public Button OpenMenu;
        public BattleMenu BattleMenu;
    }
}