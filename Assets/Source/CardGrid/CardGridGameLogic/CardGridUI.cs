using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGrid
{
    public partial class CardGridGame //UI
    {
        public Image Fade;
        public MainMenu MainMenu;
        public LevelsMenu LevelsMenu;
        public InfiniteMenuUI InfiniteLvlMenu;
        public BattleUI BattleUI;
        Transform[] ButtonsGroup;
        Button[] InfiniteLevelsButtons;
        List<LevelCell> LevelsCells = new();
        
        public AudioClip ClickSound;
        bool _battleMenuOpen;

        public void OpenMainMenu()
        {
            UpdateMainMenuRecords();
            MainMenu.gameObject.SetActive(true);
            
            AchievementsPanel.gameObject.SetActive(false);
            LevelsMenu.gameObject.SetActive(false);
            InfiniteLvlMenu.gameObject.SetActive(false);
            BattleUI.gameObject.SetActive(false);
        }

        void SubscribeOnButtons()
        {
            MainMenu.LanguageDropdown.onValueChanged.AddListener(language =>
            {
                PlayClickSound();
                ChangeLanguage(language);
                Save();
            });

            MainMenu.OpenAhievements.onClick.AddListener(() =>
            {
                OpenAchievements();
            });
            AchievementsPanel.Close.onClick.AddListener(() =>
            {
                CloseAchievements();
            });
            MainMenu.OpenLevels.onClick.AddListener(() =>
            {
                PlayClickSound();
                OpenLevelsMenu();
            });
            MainMenu.ClassicLevel.onClick.AddListener(()=>
            {
                WithQuantity = false;
                //OpenInfiniteMenu();
                PlayClickSound();
                StartNewBattle(0);
            });
            MainMenu.QuantityLevel.onClick.AddListener(()=>
            {
                WithQuantity = true;
                //OpenInfiniteMenu();
                PlayClickSound();
                StartNewBattle(0);
            });
            
            BattleUI.BattleMenu.OpenAchievement.onClick.AddListener(() =>
            {
                OpenAchievements();
            });
            BattleUI.BattleMenu.VolumeSlider.onValueChanged.AddListener(value =>
            {
                PlayClickSound();
                _CommonState.Volume = value;
                MenuAudioSource.volume = value;
                BattleAudioSource.volume = value;
                DelaySave();
            });
            MainMenu.VolumeSlider.onValueChanged.AddListener(value =>
            {
                PlayClickSound();
                _CommonState.Volume = value;
                MenuAudioSource.volume = value;
                BattleAudioSource.volume = value;
                DelaySave();
            });

            
            LevelsMenu.BackToMenu.onClick.AddListener(() =>
            {
                PlayClickSound();
                GoToMenu();
            });
            InfiniteLvlMenu.BackToMenu.onClick.AddListener(() =>
            {
                PlayClickSound();
                GoToMenu();
            });

            SetBattleUI();
            
            // InfiniteLvlMenu.Continue.onClick.AddListener(() =>
            // {
            //     ActiveBattleUI();
            //     LoadBattle();
            // });
            // InfiniteLvlMenu.NewBattleButton.onClick.AddListener(() =>
            // {
            //     OpenInfiniteLevelsMenu();
            // });
            
            SetLevelsButtons();
        }

        IEnumerator DelaySave()
        {
            StopCoroutine(DelaySave());
            yield return new WaitForSeconds(1);
            Save();
        }

        void LoadUI()
        {
            BattleUI.BattleMenu.VolumeSlider.SetValueWithoutNotify(_CommonState.Volume);
            MainMenu.VolumeSlider.SetValueWithoutNotify(_CommonState.Volume);
            MainMenu.LanguageDropdown.SetValueWithoutNotify((int) _CommonState.Language);
            LoadAchievementsPanel();
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
                InfiniteLevelsButtons[i] .interactable = true;
            }
            
            InfiniteLvlMenu.LevelsContent.SetActive(true);
        }

        void OpenLevelsMenu()
        {
            UpdateLevelsMenuStars();
            UpdateLevelButtonsStars();
            LevelsMenu.gameObject.SetActive(true);
        }

        void UpdateLevelButtonsStars()
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
            //Common_V1();
            //Infinite();
            Common();
            
            void Common()
            {
                var fieldInfo = typeof(LevelsMaps).GetField("Levels");
                var levels = (Level[])fieldInfo.GetValue(null);
                
                ButtonsGroup = new Transform[levels.Length];
                int groups = 0;
                foreach (var level in levels)
                {
                    if (level.Group > groups)
                        groups = level.Group;
                }
                
                for (int i = 0; i < groups; i++)
                {
                    ButtonsGroup[i] = Instantiate(LevelsMenu.LevelGroup, LevelsMenu.LevelsContent.transform);
                }

                for (int i = 0; i < levels.Length; i++)
                {
                    var group = levels[i].Group - 1;
                    var levelCell = Instantiate(LevelsMenu.LevelButton, ButtonsGroup[group]);
                    LevelsCells.Add(levelCell); 
                    var levelID = i;
                    levelCell.Number.text = (levelID + 1).ToString();
                    levelCell.Button.onClick.AddListener(() =>
                    {
                        PlayClickSound();
                        StartNewBattle(levelID + BattleState.CommonLevelID);
                    });

                    levelCell.Shading.SetActive(_CommonState.Levels[i].IsOpen);

                    if(_CommonState.Levels[i].Complete)
                        levelCell.Star.sprite = LevelsMenu.Star;
                }
            }
            
            void Common_V1()
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
                        StartNewBattle(levelID+ BattleState.CommonLevelID));

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
                
                    button.onClick.AddListener(() => StartNewBattle(levelID));
                }
            }
        }

        void SetBattleUI()
        {
            BattleUI.RotateRight.onClick.AddListener(()=>
                {
                    if (_inputActive)
                    {
                        DisableHighlightInfo();
                        StartCoroutine(RotateRight());
                    }
                });
            BattleUI.RotateLeft.onClick.AddListener(()=>
                {
                    if (_inputActive)
                    {
                        DisableHighlightInfo();
                        StartCoroutine(RotateLeft());
                    }
                });
            
            BattleUI.OpenMenu.onClick.AddListener(() =>
            {
                _battleMenuOpen = true;
                PlayClickSound();
                OpenMenu(BattleUI.BattleMenu);
            });
            BattleUI.TutorButton.onClick.AddListener(() =>
            {
                PlayClickSound();
                ActivateTextTutor();
            });
            
            
            BattleUI.BattleMenu.PlayAgain.onClick.AddListener(() =>
            {
                PlayClickSound();
                PlayAgain();
            });
            BattleUI.BattleMenu.ToMenu.onClick.AddListener(() =>
            {
                PlayClickSound();
                GoToMenu();
                _battleMenuOpen = false;
            });
            BattleUI.BattleMenu.Close.onClick.AddListener(() =>
            {
                PlayClickSound();
                BattleUI.BattleMenu.gameObject.SetActive(false);
                
                _battleMenuOpen = false;
            });
            BattleUI.BattleMenu.NextLevel.onClick.AddListener(() =>
            {
                PlayClickSound();
                EndBattle();
                StartNewBattle(_CommonState.BattleState.LevelID + 1);
            });
            BattleUI.BattleMenu.LevelMenu.onClick.AddListener(() =>
            {
                PlayClickSound();
                EndBattle();
                OpenLevelsMenu();
            });
            
            BattleUI.EndAttempt.onClick.AddListener(() =>
            {
                BattleUI.EndItemsReward.gameObject.SetActive(false);
                OpenDefeat(BattleUI.BattleMenu);
            });
            
#if UNITY_WEBGL && !UNITY_EDITOR
            
            BattleUI.GetAdItems.onClick.AddListener(() =>
            {
                _rewarded = false;
                rewardsLeft--;
                Yandex.ShowRewardAd();
            });

            Yandex.SetActiveRateButton();
            BattleUI.BattleMenu.RateGame.onClick.AddListener(() =>
            {
                Yandex.RateGame();
            });
#endif

            void PlayAgain()
            {
                BattleUI.BattleMenu.gameObject.SetActive(false);
                DestroyCards();
                StartNewBattle(_CommonState.BattleState.LevelID);
            }
        }

        void UpdateMainMenuRecords()
        {
            MainMenu.MaxLevelClassic.text = _CommonState.BestLevelClassic.ToString();
            MainMenu.MaxLevelQuantity.text = _CommonState.BestLevelQuantity.ToString();
            
            var maxStars = _CommonState.Levels.Length;
            int currentStars = 0;
            foreach (var level in _CommonState.Levels)
            {
                if (level.Complete)
                    currentStars++;
            }

            MainMenu.StarsQuantity.text = currentStars.ToString() + "/" + maxStars.ToString();
            MainMenu.TrophiesQuantity.text = _CommonState.AchievementsTrophies.ToString();
            MainMenu.Notify.SetActive(IsHaveCompletedAchievements());
        } 
        
        void UpdateLevelsMenuStars()
        {
            var maxStars = _CommonState.Levels.Length;
            int currentStars = 0;
            foreach (var level in _CommonState.Levels)
            {
                if (level.Complete)
                    currentStars++;
            }

            LevelsMenu.StarsQuantity.text = currentStars.ToString() + "/" + maxStars.ToString();
        }

        void UpdateRequires((ColorType, int)[] collection)
        {
            int i = 0;
            foreach (var color in collection)
            {
                BattleUI.Requires[i].GemSprite.sprite = BattleUI.GetColorSprite(color.Item1);
                if (color.Item2 > 0)
                {
                    BattleUI.Requires[i].Quantity.text = color.Item2.ToString();
                    BattleUI.Requires[i].Quantity.gameObject.SetActive(true);
                    BattleUI.Requires[i].ToggleCheck.gameObject.SetActive(false);
                }
                else
                {
                    BattleUI.Requires[i].Quantity.gameObject.SetActive(false);
                    BattleUI.Requires[i].ToggleCheck.gameObject.SetActive(true);
                }
                BattleUI.Requires[i].gameObject.SetActive(true);
                i++;
            }
            
            for (; i < BattleUI.Requires.Length; i++)
            {
                BattleUI.Requires[i].gameObject.SetActive(false);
            }
        }

        void UpdateCompleteQuest((ColorType, int)[] collection)
        {
            int i = 0;
            var panel = BattleUI.QuestCompletePanel;
            foreach (var color in collection)
            {
                panel.Requires[i].GemSprite.sprite = BattleUI.GetColorSprite(color.Item1);
                if (color.Item2 > 0)
                {
                    panel.Requires[i].Quantity.text = color.Item2.ToString();
                    panel.Requires[i].Quantity.gameObject.SetActive(true);
                    panel.Requires[i].ToggleCheck.gameObject.SetActive(false);
                }
                else
                {
                    panel.Requires[i].Quantity.gameObject.SetActive(false);
                    panel.Requires[i].ToggleCheck.gameObject.SetActive(true);
                }
                panel.Requires[i].gameObject.SetActive(true);
                i++;
            }
            
            for (; i < panel.Requires.Length; i++)
            {
                panel.Requires[i].gameObject.SetActive(false);
            }
        }

        void ActiveBattleUI()
        {
            MainMenu.gameObject.SetActive(false);
            LevelsMenu.gameObject.SetActive(false);
            InfiniteLvlMenu.gameObject.SetActive(false);
            InfiniteLvlMenu.LevelsContent.SetActive(false);
            BattleUI.BattleMenu.gameObject.SetActive(false);
            BattleUI.QuestCompletePanel.gameObject.SetActive(false);
            BattleUI.LevelCompletePanel.gameObject.SetActive(false);
            BattleUI.NewLevelPanel.gameObject.SetActive(false);
            var tutor = _CommonState.BattleState.LevelID >= BattleState.CommonLevelID;
            BattleUI.BattleMenu.LevelMenu.gameObject.SetActive(tutor);
            BattleUI.BattleMenu.LevelAchievedPanel.SetActive(!tutor);
            BattleUI.TutorButton.gameObject.SetActive(!tutor);

            BattleUI.gameObject.SetActive(true);

            BattleUI.LevelNumber.text = _CommonState.BattleState.NumberLevel.ToString();
            BattleUI.LevelProgress.maxValue = _CommonState.BattleState.MaxLevelProgress;
            BattleUI.LevelProgress.SetValueWithoutNotify(_CommonState.BattleState.LevelProgress);
        }

        void GoToMenu()
        {
            EndBattle();
            UpdateMainMenuRecords();
            BattleUI.QuestCompletePanel.gameObject.SetActive(false);
            BattleUI.LevelCompletePanel.gameObject.SetActive(false);
            InfiniteLvlMenu.Continue.gameObject.SetActive(false);
            BattleUI.gameObject.SetActive(false);
            LevelsMenu.gameObject.SetActive(false);
            InfiniteLvlMenu.gameObject.SetActive(false);
            for (int i = 0; i < BattleUI.Requires.Length; i++)
            {
                BattleUI.Requires[i].gameObject.SetActive(false);
            }
            
            MainMenu.gameObject.SetActive(true);
        }

        public void OpenMenu(BattleMenu menu)
        {
            menu.MenuLable.gameObject.SetActive(true);
            menu.EndLable.gameObject.SetActive(false);
            menu.Image.sprite = menu.MenuSprite;
            menu.Close.gameObject.SetActive(true);
            menu.PlayAgain.GetComponent<ButtonSwaying>().StopSway();
            menu.PlayAgain.gameObject.SetActive(true);
            
            var levelID = _CommonState.BattleState.GetRealLevelID();
            if (levelID < _CommonState.Levels.Length - 1)
            {
                menu.NextLevel.gameObject.SetActive(_CommonState.Levels[levelID].Stars > 0);
            }
            else
                menu.NextLevel.gameObject.SetActive(false);
            
            BattleUI.BattleMenu.LevelAchievedNumber.text = _CommonState.BattleState.NumberLevel.ToString();
            
            menu.gameObject.SetActive(true);
            BattleUI.BattleMenu.Notify.SetActive(IsHaveCompletedAchievements());
            BattleUI.BattleMenu.TrophiesQuantity.text = _CommonState.AchievementsTrophies.ToString();
        }

        public void OpenDefeat(BattleMenu menu)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if(WithQuantity)
                Yandex.GetLeaderBoard(CLASSICMODE);
            else
                Yandex.GetLeaderBoard(ONLYWITHCOLOR);
#endif
            
            menu.MenuLable.gameObject.SetActive(false);
            menu.EndLable.gameObject.SetActive(true);
            menu.Image.sprite = menu.DefeatSprite;
            menu.Close.gameObject.SetActive(false);
            menu.PlayAgain.gameObject.SetActive(true);
            menu.PlayAgain.GetComponent<ButtonSwaying>().Swaying = true;
            //menu.PlayAgain.GetComponent<ButtonSwaying>().GoSway();
            
            var levelID = _CommonState.BattleState.GetRealLevelID();
            if (_CommonState.BattleState.GetRealLevelID() < _CommonState.Levels.Length - 1)
            {
                menu.NextLevel.gameObject.SetActive(_CommonState.Levels[levelID].Stars > 0);
            }
            else
            {
                menu.NextLevel.gameObject.SetActive(false);
            }
            BattleUI.BattleMenu.LevelAchievedNumber.text = _CommonState.BattleState.NumberLevel.ToString();
            
            menu.gameObject.SetActive(true);
        }

        public void OpenWin(BattleMenu menu)
        {
            menu.MenuLable.gameObject.SetActive(true);
            menu.EndLable.gameObject.SetActive(true);
            menu.Image.sprite = menu.TrophySprite;
            menu.Close.gameObject.SetActive(false);
            menu.PlayAgain.GetComponent<ButtonSwaying>().StopSway();
            menu.PlayAgain.gameObject.SetActive(true);
            
            TutorHandObj.SetActive(false);
            Highlight.gameObject.SetActive(false);

            var levelID = _CommonState.BattleState.GetRealLevelID();
            menu.NextLevel.gameObject.SetActive(levelID < _CommonState.Levels.Length - 1);
            BattleUI.BattleMenu.LevelAchievedNumber.text = _CommonState.BattleState.NumberLevel.ToString();
            
            menu.gameObject.SetActive(true);
        }

        void EndBattle()
        {
            TutorHandObj.SetActive(false);
            Highlight.gameObject.SetActive(false);
            StopAllCoroutines();
            DOTween.KillAll();
            Save();
            DestroyAndUnloadCards();
        }

        void PlayClickSound()
        {
            MenuAudioSource.clip = ClickSound;
            MenuAudioSource.Play();
        }

        //Yandex
        public void SetActiveRateButton(bool active)
        {
            BattleUI.BattleMenu.RateGame.gameObject.SetActive(active);
        }
    }
}