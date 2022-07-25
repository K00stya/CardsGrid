using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGrid
{
    [Serializable]
    public class BattleUI : MonoBehaviour
    {
        public GameObject GameObject;
        public TextMeshProUGUI Score;
        public Button OpenMenu;
        public GameObject Defeat;
        public Button ToMenuOnDeafeat;
    }
}