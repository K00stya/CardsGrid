using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGrid
{
    public partial class CardGridGame //UI
    {
        public MainMenu MainMenu;
        public LevelsMenu LevelsMenu;
        public InfiniteMenuUI InfiniteLvlMenu;
        public BattleUI BattleUI;
        Transform[] ButtonsGroup;
        Button[] InfiniteLevelsButtons;
        List<LevelCell> LevelsCells = new();

        public void OpenMainMenu()
        {
            MainMenu.gameObject.SetActive(true);
            
            LevelsMenu.gameObject.SetActive(false);
            InfiniteLvlMenu.gameObject.SetActive(false);
            BattleUI.gameObject.SetActive(false);
        }

        void SubscribeOnButtons()
        {
            MainMenu.LanguageDropdown.onValueChanged.AddListener(language =>
            {
                ChangeLanguage(language);
            });
            
            MainMenu.OpenLevels.onClick.AddListener(() =>
            {
                OpenLevelsMenu();
            });
            MainMenu.OpenInfiniteMenu.onClick.AddListener(()=>
            {
                OpenInfiniteMenu();
            });
            MainMenu.VolumeSlider.onValueChanged.AddListener(value =>
            {
                _CommonState.Volume = value;
            });

            
            LevelsMenu.BackToMenu.onClick.AddListener(() =>
            {
                GoToMenu();
            });
            InfiniteLvlMenu.BackToMenu.onClick.AddListener(() =>
            {
                GoToMenu();
            });

            SetBattleUI();
            
            InfiniteLvlMenu.Continue.onClick.AddListener(() =>
            {
                ActiveBattleUI();
                StartCoroutine(LoadBattle());
            });
            InfiniteLvlMenu.NewBattleButton.onClick.AddListener(() =>
            {
                OpenInfiniteLevelsMenu();
            });
            
            SetLevelsButtons();
        }

        void LoadUI()
        {
            UpdateMainMenuStars();

            MainMenu.VolumeSlider.SetValueWithoutNotify(_CommonState.Volume);
            MainMenu.LanguageDropdown.SetValueWithoutNotify((int) _CommonState.Language);
            
            InfiniteLvlMenu.Continue.gameObject.SetActive(_CommonState.InBattle);
            InfiniteLvlMenu.BestScore.text = _CommonState.BestScore.ToString();
        }

        void OpenInfiniteMenu()
        {
            InfiniteLvlMenu.gameObject.SetActive(true);
            MainMenu.gameObject.SetActive(false);
        }
        
        void OpenInfiniteLevelsMenu()
        {
            for (int i = 0; i < InfiniteLevels.Length; i++)
            {
                if (InfiniteLevels[i].Open || _CommonState.BestScore >= InfiniteLevels[i].NeedScoreToOpen)
                {
                    InfiniteLevelsButtons[i] .interactable = true;
                }
                else
                {
                    InfiniteLevelsButtons[i] .interactable = false;
                }
            }
            
            InfiniteLvlMenu.LevelsContent.SetActive(true);
        }

        void OpenLevelsMenu()
        {
            UpdateStars();
            LevelsMenu.gameObject.SetActive(true);
        }

        void UpdateStars()
        {
            for (int i = 0; i < _CommonState.Levels.Length; i++)
            {
                if (_CommonState.Levels[i].Complete)
                    LevelsCells[i].Star.sprite = LevelsMenu.Star;
            }
        }

        //TODO Stars on level button
        void SetLevelsButtons()
        {
            Common();
            Infinite();
            
            void Common()
            {
                ButtonsGroup = new Transform[CommonLevelsGroups.Length];
                for (int i = 0; i < CommonLevelsGroups.Length; i++)
                {
                    ButtonsGroup[i] = Instantiate(LevelsMenu.LevelGroup, LevelsMenu.LevelsContent.transform);
                }

                for (int i = 0; i < _CommonState.Levels.Length; i++)
                {
                    var group = _CommonState.Levels[i].Group;
                    var levelCell = Instantiate(LevelsMenu.LevelButton, ButtonsGroup[group]);
                    LevelsCells.Add(levelCell); 
                    var levelID = i;
                    levelCell.Number.text = (levelID + 1).ToString();
                    levelCell.Button.onClick.AddListener(() =>
                        StartCoroutine(StartNewBattle(levelID+ BattleState.CommonLevelID)));

                    levelCell.Shading.SetActive(_CommonState.Levels[i].IsOpen);

                    if(_CommonState.Levels[i].Complete)
                        levelCell.Star.sprite = LevelsMenu.Star;
                }
            }

            void Infinite()
            {
                InfiniteLevelsButtons = new Button[InfiniteLevels.Length];
                for (int i = 0; i < InfiniteLevels.Length; i++)
                {
                    var levelID = i;
                    var level = InfiniteLevels[i];
                    var button = Instantiate(InfiniteLvlMenu.LevelButton, InfiniteLvlMenu.LevelsContent.transform);
                    InfiniteLevelsButtons[i] = button;
                    button.GetComponentInChildren<TextMeshProUGUI>().text =
                        !level.Open ? $"{level.LevelName} ({level.NeedScoreToOpen})" : level.LevelName;
                
                    button.onClick.AddListener(() =>
                        StartCoroutine(StartNewBattle(levelID)));
                }
            }
        }

        void SetBattleUI()
        {
            BattleUI.OpenMenu.onClick.AddListener(() =>
            {
                OpenMenu(BattleUI.BattleMenu);
            });
            BattleUI.PlayAgain.onClick.AddListener(() =>
            {
                PlayAgain();
            });
            
            BattleUI.BattleMenu.PlayAgain.onClick.AddListener(() =>
            {
                PlayAgain();
            });
            BattleUI.BattleMenu.ToMenu.onClick.AddListener(() =>
            {
                GoToMenu();
            });
            BattleUI.BattleMenu.Close.onClick.AddListener(() =>
            {
                BattleUI.BattleMenu.gameObject.SetActive(false);
            });
            BattleUI.BattleMenu.NextLevel.onClick.AddListener(() =>
            {
                StartCoroutine(StartNewBattle(_CommonState.BattleState.LevelID + 1));
            });

            void PlayAgain()
            {
                BattleUI.BattleMenu.gameObject.SetActive(false);
                DestroyCards();
                StartCoroutine(StartNewBattle(_CommonState.BattleState.LevelID));
            }
        }

        void UpdateMainMenuStars()
        {
            var maxStars = _CommonState.Levels.Length;
            int currentStars = 0;
            foreach (var level in _CommonState.Levels)
            {
                if (level.Complete)
                    currentStars++;
            }

            MainMenu.StarsQuantity.text = currentStars.ToString() + "/" + maxStars.ToString();
        }

        void ActiveBattleUI()
        {
            MainMenu.gameObject.SetActive(false);
            LevelsMenu.gameObject.SetActive(false);
            InfiniteLvlMenu.gameObject.SetActive(false);
            InfiniteLvlMenu.LevelsContent.SetActive(false);
            BattleUI.BattleMenu.gameObject.SetActive(false);

            BattleUI.gameObject.SetActive(true);
            BattleUI.Score.text = _CommonState.BattleState.Score.ToString();
        }

        void GoToMenu()
        {
            if (!_inputActive) return;
            Save();
            UpdateMainMenuStars();
            InfiniteLvlMenu.Continue.gameObject.SetActive(false);
            BattleUI.gameObject.SetActive(false);
            LevelsMenu.gameObject.SetActive(false);
            InfiniteLvlMenu.gameObject.SetActive(false);
            InfiniteLvlMenu.BestScore.text = _CommonState.BestScore.ToString();
            DestroyAndUnloadCards();
            
            MainMenu.gameObject.SetActive(true);
        }

        public void OpenMenu(BattleMenu menu)
        {
            menu.Lable.text = "Menu"; //
            menu.Image.sprite = menu.MenuSprite;
            menu.Close.gameObject.SetActive(true);
            menu.PlayAgain.gameObject.SetActive(true);
            
            var levelID = _CommonState.BattleState.GetRealLevelID();
            if (levelID < _CommonState.Levels.Length - 1)
                menu.NextLevel.gameObject.SetActive(_CommonState.Levels[levelID].Stars > 0);
            else
                menu.NextLevel.gameObject.SetActive(false);
            
            menu.gameObject.SetActive(true);
        }

        public void OpenDefeat(BattleMenu menu)
        {
            menu.Lable.text = "ITEMS END"; //
            menu.Image.sprite = menu.DefeatSprite;
            menu.Close.gameObject.SetActive(false);
            menu.PlayAgain.gameObject.SetActive(true);
            
            var levelID = _CommonState.BattleState.GetRealLevelID();
            if (_CommonState.BattleState.GetRealLevelID() < _CommonState.Levels.Length - 1)
            {
                menu.NextLevel.gameObject.SetActive(_CommonState.Levels[levelID].Stars > 0);
            }
            else
            {
                menu.NextLevel.gameObject.SetActive(false);
            }
            
            menu.gameObject.SetActive(true);
        }

        public void OpenWin(BattleMenu menu)
        {
            menu.Lable.text = "SUCCESS"; //
            menu.Image.sprite = menu.TrophySprite;
            menu.Close.gameObject.SetActive(false);
            menu.PlayAgain.gameObject.SetActive(true);

            var levelID = _CommonState.BattleState.GetRealLevelID();
            menu.NextLevel.gameObject.SetActive(levelID < _CommonState.Levels.Length - 1);
            
            menu.gameObject.SetActive(true);
        }
    }
}