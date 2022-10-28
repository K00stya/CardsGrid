﻿using System.Collections.Generic;
using DG.Tweening;
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
        
        public AudioClip ClickSound;

        public void OpenMainMenu()
        {
            UpdateMainMenuStars();
            MainMenu.gameObject.SetActive(true);
            
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
            });
            
            MainMenu.OpenLevels.onClick.AddListener(() =>
            {
                PlayClickSound();
                OpenLevelsMenu();
            });
            MainMenu.OpenInfiniteMenu.onClick.AddListener(()=>
            {
                //OpenInfiniteMenu();
                PlayClickSound();
                StartNewBattle(0);
            });
            
            BattleUI.BattleMenu.VolumeSlider.onValueChanged.AddListener(value =>
            {
                PlayClickSound();
                _CommonState.Volume = value;
                MenuAudioSource.volume = value;
                BattleAudioSource.volume = value;
            });
            MainMenu.VolumeSlider.onValueChanged.AddListener(value =>
            {
                PlayClickSound();
                _CommonState.Volume = value;
                MenuAudioSource.volume = value;
                BattleAudioSource.volume = value;
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

        void LoadUI()
        {
            BattleUI.BattleMenu.VolumeSlider.SetValueWithoutNotify(_CommonState.Volume);
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
            BattleUI.OpenMenu.onClick.AddListener(() =>
            {
                PlayClickSound();
                OpenMenu(BattleUI.BattleMenu);
            });
            BattleUI.PlayAgain.onClick.AddListener(() =>
            {
                PlayClickSound();
                PlayAgain();
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
            });
            BattleUI.BattleMenu.Close.onClick.AddListener(() =>
            {
                PlayClickSound();
                BattleUI.BattleMenu.gameObject.SetActive(false);
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

            void PlayAgain()
            {
                BattleUI.BattleMenu.gameObject.SetActive(false);
                DestroyCards();
                StartNewBattle(_CommonState.BattleState.LevelID);
            }
        }

        void UpdateMainMenuStars()
        {
            MainMenu.MaxScore.text = _CommonState.BestScore.ToString();
            
            var maxStars = _CommonState.Levels.Length;
            int currentStars = 0;
            foreach (var level in _CommonState.Levels)
            {
                if (level.Complete)
                    currentStars++;
            }

            MainMenu.StarsQuantity.text = currentStars.ToString() + "/" + maxStars.ToString();
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

        void ActiveBattleUI()
        {
            MainMenu.gameObject.SetActive(false);
            LevelsMenu.gameObject.SetActive(false);
            InfiniteLvlMenu.gameObject.SetActive(false);
            InfiniteLvlMenu.LevelsContent.SetActive(false);
            BattleUI.BattleMenu.gameObject.SetActive(false);
            BattleUI.BattleMenu.LevelMenu.gameObject.SetActive(
                _CommonState.BattleState.LevelID >= BattleState.CommonLevelID);

            BattleUI.gameObject.SetActive(true);
            BattleUI.Score.text = _CommonState.BattleState.Score.ToString();
        }

        void GoToMenu()
        {
            EndBattle();
            UpdateMainMenuStars();
            InfiniteLvlMenu.Continue.gameObject.SetActive(false);
            BattleUI.gameObject.SetActive(false);
            LevelsMenu.gameObject.SetActive(false);
            InfiniteLvlMenu.gameObject.SetActive(false);
            InfiniteLvlMenu.BestScore.text = _CommonState.BestScore.ToString();
            for (int i = 0; i < BattleUI.Requires.Length; i++)
            {
                BattleUI.Requires[i].gameObject.SetActive(false);
            }
            
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

        void EndBattle()
        {
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
    }
}