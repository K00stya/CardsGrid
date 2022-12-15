using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InstantGamesBridge;

namespace CardGrid
{
    public partial class CardGridGame //UI
    {
        public MainMenu MainMenu;
        public TutorialMenu TutorialMenu;
        public VillageMenu VillageMenu;
        public ExpeditionMenu ExpeditionMenu;
        public BattleUI BattleUI;
        public Achievements AchievementsMenu;
        public SettingsMenu SettingsMenu;
        public float MenuChangeSpeed = 1f;
        public float _menusOffet;
        
        public Image Fade;
        Transform TutorButtons;
        Button[] InfiniteLevelsButtons;
        List<LevelCell> LevelsCells = new();
        
        public AudioClip ClickSound;
        bool _battleMenuOpen;
        
        void LoadUI()
        {
            LoadExpeditions();
            
            SettingsMenu.VolumeSlider.SetValueWithoutNotify(_CommonState.Volume);
            SettingsMenu.LanguageDropdown.SetValueWithoutNotify((int) _CommonState.Language);
            LoadAchievementsPanel();

            _menusOffet = Vector2.Distance(MainMenu.Memenus.GetChild(0).transform.localPosition,
                MainMenu.Memenus.GetChild(1).transform.localPosition);
        }

        void LoadExpeditions()
        {
            int i = 0;
            foreach (var level in _CommonState.Levels)
            {
                ExpeditionPanel panel = null;
                switch (level.Type)
                {
                    case LevelType.Tutor:
                        continue;
                    case LevelType.Exploration:
                        panel = Instantiate(ExpeditionMenu.PanelPrefab, ExpeditionMenu.ExplorationGroup);
                        break;
                    case LevelType.Extraction:
                        panel = Instantiate(ExpeditionMenu.PanelPrefab, ExpeditionMenu.ExtractionGroup);
                        break;
                    case LevelType.Battle:
                        panel = Instantiate(ExpeditionMenu.PanelPrefab, ExpeditionMenu.BattleGroup);
                        break;
                }

                SubLevelButton(panel, i);
                i++;
            }

            void SubLevelButton(ExpeditionPanel panel, int index)
            {
                panel.StartExpedition.onClick.AddListener(() =>
                {
                    StartNewBattle(index);
                });
            }
        }

        public void OpenMainMenu()
        {
        }
        
        void SubscribeOnButtons()
        {
            SetMemenuButtons();
            SetSettingsButtons();
            SetBattleUI();
            SetTutorialLevels();

            void SetSettingsButtons()
            {
                SettingsMenu.VolumeSlider.onValueChanged.AddListener(value =>
                {
                    PlayClickSound();
                    _CommonState.Volume = value;
                    MenuAudioSource.volume = value;
                    BattleAudioSource.volume = value;
                });
                SettingsMenu.LanguageDropdown.onValueChanged.AddListener(language =>
                {
                    PlayClickSound();
                    ChangeLanguage(language);
                });
            }

            void SetBattleUI()
            {
                BattleUI.RotateRight.onClick.AddListener(() =>
                {
                    if (_inputActive)
                    {
                        DisableHighlightInfo();
                        StartCoroutine(RotateRight());
                    }
                });
                BattleUI.RotateLeft.onClick.AddListener(() =>
                {
                    if (_inputActive)
                    {
                        DisableHighlightInfo();
                        StartCoroutine(RotateLeft());
                    }
                });

                BattleUI.GetAdItems.onClick.AddListener(() =>
                {
                    rewardsLeft--;
                    Bridge.advertisement.ShowRewarded(success =>
                    {
                        if (success)
                        {
                            StartCoroutine(AddRewardedItems());
                        }
                        else
                        {
                            NotRewardPlayer();
                        }
                    });
                });

                if (Bridge.social.isRateSupported)
                {
                    if (Bridge.platform.id == "yandex")
                        Yandex.SetActiveRateButton();
                    SettingsMenu.RateGame.onClick.AddListener(() => { Bridge.social.Rate(); });
                }
                else
                {
                    SettingsMenu.RateGame.gameObject.SetActive(false);
                }

                void PlayAgain()
                {
                    DestroyCards();
                    StartNewBattle(_CommonState.BattleState.LevelID);
                }
            }
        }
        void SetMemenuButtons()
        {
            MainMenu.OpenTutors.onClick.AddListener(() =>
            {
                PlayClickSound();
                OpenTutorialsMenu();
                MoveMainMenu(2);
                MoveFrame(MainMenu.OpenTutors.transform.localPosition.x);
            });
            MainMenu.OpenVillage.onClick.AddListener(() =>
            {
                MoveMainMenu(1);
                MoveFrame(MainMenu.OpenVillage.transform.localPosition.x);
            });
            MainMenu.OpenBattle.onClick.AddListener(() =>
            {
                MoveMainMenu(0);
                MoveFrame(MainMenu.OpenBattle.transform.localPosition.x);
            });
            MainMenu.OpenAhievements.onClick.AddListener(() =>
            {
                PlayClickSound();
                OpenAchievements();
                MoveMainMenu(-1);
                MoveFrame(MainMenu.OpenAhievements.transform.localPosition.x);
            });
            MainMenu.OpenSettings.onClick.AddListener(() =>
            {
                MoveMainMenu(-2);
                MoveFrame(MainMenu.OpenSettings.transform.localPosition.x);
            });

            void MoveFrame(float x)
            {
                MainMenu.Frame.DOLocalMoveX(x, MenuChangeSpeed);
            }
            
            void OpenTutorialsMenu()
            {
                UpdateTutorialStars();
                UpdateLevelButtonsStars();
                TutorialMenu.gameObject.SetActive(true);

                void UpdateTutorialStars()
                {
                    var maxStars = _CommonState.Levels.Length;
                    int currentStars = 0;
                    foreach (var level in _CommonState.Levels)
                    {
                        if (level.Type == LevelType.Tutor && level.Complete > 0)
                            currentStars++;
                    }

                    TutorialMenu.StarsQuantity.text = currentStars.ToString() + "/" + maxStars.ToString();
                }
                
                void UpdateLevelButtonsStars()
                {
                    for (int i = 0; i < _CommonState.Levels.Length; i++)
                    {
                        var level = _CommonState.Levels[i];
                        if (level.Type == LevelType.Tutor && level.Complete > 0)
                            LevelsCells[level.ScrObj.ArrayTypeIndex].Star.sprite = TutorialMenu.Star;
                    }
                }
            }
            
            void OpenAchievements()
            {
                UpdateAchievements();
                AchievementsMenu.gameObject.SetActive(true);
            }
        }

        private bool menuMove;
        void MoveMainMenu(int offset)
        {
            Move(MainMenu.Memenus);

            void Move(Transform t)
            {
                menuMove = true;
                t.DOKill();
                t.DOLocalMoveX(_menusOffet * offset, MenuChangeSpeed)
                    .OnComplete(() =>
                    {
                        menuMove = false;
                    });
            }
        }

        void UpdateBattlePos()
        {
            if (menuMove)
            {
                var pos = BattleObjects.transform.position;
                BattleObjects.transform.position = 
                    new Vector3(BattleUI.transform.position.x,pos.y,pos.z);
            }
        }

        void SetTutorialLevels()
        {
            var levels = _CommonState.Levels;

            for (int i = 0; i < levels.Length; i++)
            {
                var levelCell = Instantiate(TutorialMenu.LevelButton, TutorButtons);
                LevelsCells.Add(levelCell); 
                var levelID = i;
                levelCell.Number.text = (levelID + 1).ToString();
                levelCell.Button.onClick.AddListener(() =>
                {
                    PlayClickSound();
                    StartNewBattle(levelID);
                });

                levelCell.Shading.SetActive(_CommonState.Levels[i].IsOpen);

                if(_CommonState.Levels[i].Complete > 0)
                    levelCell.Star.sprite = TutorialMenu.Star;
            }
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

        void ActiveBattleUI()
        {
            _battleMenuOpen = false;
        }

        void GoToMenu()
        {
            EndBattle();
            BattleUI.gameObject.SetActive(false);
            TutorialMenu.gameObject.SetActive(false);
            for (int i = 0; i < BattleUI.Requires.Length; i++)
            {
                BattleUI.Requires[i].gameObject.SetActive(false);
            }
            
            Save(false);
        }

        public void OpenDefeat()
        {
        }

        public void OpenWin()
        {
        }

        void EndBattle()
        {
            if(_CommonState.BattleState.LevelID == 0)
                Bridge.advertisement.ShowInterstitial();
            _selectedCard = null;
            TutorHandObj.SetActive(false);
            Highlight.gameObject.SetActive(false);
            StopAllCoroutines();
            DOTween.KillAll();
            DestroyAndUnloadCards();
        }

        void PlayClickSound()
        {
            MenuAudioSource.clip = ClickSound;
            MenuAudioSource.Play();
        }

        //Yandex
        public void SetActiveRateButton()
        {
            SettingsMenu.RateGame.gameObject.SetActive(true);
        }
        public void SetDeActiveRateButton()
        {
            SettingsMenu.RateGame.gameObject.SetActive(false);
        }
    }
}