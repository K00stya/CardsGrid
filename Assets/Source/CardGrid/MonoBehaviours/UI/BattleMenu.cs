using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGrid
{
    public class BattleMenu : MonoBehaviour
    {
        public TextMeshProUGUI MenuLable;
        public TextMeshProUGUI EndLable;
        public Image Image;
        public Sprite TrophySprite;
        public Sprite DefeatSprite;
        public Sprite MenuSprite;

        public Button Close;
        public Difficult[] Difficults;
        public Button OpenAchievement;
        public GameObject Notify;

        public GameObject LevelAchievedPanel;
        public TextMeshProUGUI LevelAchievedNumber;
        public Button RateGame;
        public Button PlayAgain;
        public Button NextDifficult;
        public Button NextLevel;
        public Button LevelMenu;
        public Button ToMenu;
        public Slider VolumeSlider;
    }
}
