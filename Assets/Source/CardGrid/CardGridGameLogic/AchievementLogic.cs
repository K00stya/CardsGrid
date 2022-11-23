using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardGrid
{
    
    public partial class CardGridGame
    {
        public Achievements AchievementsPanel;
        public AchieveSO[] AchievementsSO;
        List<Achivement> _achievements;
        bool _rewarding;

        void AddProgressAchieve(string key, int quantity)
        {
            foreach (var acive in _CommonState.Achievements)
            {
                if (acive.Key == key)
                {
                    acive.Progress += quantity;
                }
            }
        }

        void SetProgressAchieve(string key, int quantity)
        {
            foreach (var acive in _CommonState.Achievements)
            {
                if (acive.Key == key)
                {
                    if(acive.Progress < quantity)
                        acive.Progress = quantity;
                }
            }
        }

        void CollectGemsAchive(CardState card)
        {
            switch (card.CardSO.CardType)
            {
                case CT.B1:
                    AddProgressAchieve("GetBlue", 1);
                    break;
                case CT.G2:
                    AddProgressAchieve("GetGreen", 1);
                    break;
                case CT.P3:
                    AddProgressAchieve("GetPurple", 1);
                    break;
                case CT.R4:
                    AddProgressAchieve("GetRed", 1);
                    break;
                case CT.Y5:
                    AddProgressAchieve("GetYellow", 1);
                    break;
            }
        }

        void CombinationAchieve(CardState[] cards)
        {
            CT current = CT.Em;
            int combo = 0;
            foreach (var card in cards)
            {
                if (card.CardSO.CardType == current)
                {
                    combo++;
                }
                else
                {
                    combo = 1;
                    current = card.CardSO.CardType;
                    SetCombAchieve(card, combo);
                }
            }
            
            SetCombAchieve(cards.Last(), combo);

            void SetCombAchieve(CardState card, int combo)
            {
                if (combo > 2)
                    switch (card.CardSO.CardType)
                    {
                        case CT.B1:
                            AddProgressAchieve("CombinationBlue", 1);
                            break;
                        case CT.G2:
                            AddProgressAchieve("CombinationGreen", 1);
                            break;
                        case CT.P3:
                            AddProgressAchieve("CombinationPurple", 1);
                            break;
                        case CT.R4:
                            SetProgressAchieve("CombinationRed", combo);
                            break;
                        case CT.Y5:
                            AddProgressAchieve("CombinationYellow", 1);
                            break;
                    }
            }

        }
        
        void LoadAchievementsStates()
        {
            _CommonState.Achievements = new AchieveState[AchievementsSO.Length];
            for (int i = 0; i < _CommonState.Achievements.Length; i++)
            {
                _CommonState.Achievements[i] = new AchieveState()
                {
                    Key = AchievementsSO[i].name,
                    Level = 0,
                    MaxProgress = AchievementsSO[i].Levels[0].MaxProgress,
                    Reward = AchievementsSO[i].Levels[0].Reward,
                };
            }
        }
        
        void LoadAchievementsPanel()
        {
            _achievements = new List<Achivement>(_CommonState.Achievements.Length);
            foreach (var achieveState in _CommonState.Achievements)
            {
                var achieveGO = Instantiate(AchievementsPanel.Ahievement, AchievementsPanel.Content);
                achieveState.AchiveGO = achieveGO;
                _achievements.Add(achieveGO);
                achieveGO.Description.text = string.Format(GetAchiveDescription(achieveState.Key), achieveState.MaxProgress);
                achieveGO.CheckBox.onClick.AddListener(() => CompleteAchievement(achieveState));
                
                UpdateAchive(achieveGO, achieveState);
            }
        }

        void OpenAchievements()
        {
            UpdateAchievements();
            AchievementsPanel.gameObject.SetActive(true);
        }

        void CloseAchievements()
        {
            AchievementsPanel.gameObject.SetActive(false);
            MainMenu.Notify.SetActive(IsHaveCompletedAchievements());
            BattleUI.BattleMenu.Notify.SetActive(IsHaveCompletedAchievements());
            BattleUI.BattleMenu.TrophiesQuantity.text = _CommonState.AchievementsTrophies.ToString();
            Save();
        }

        void UpdateAchievements()
        {
            AchievementsPanel.Trophies.text = _CommonState.AchievementsTrophies.ToString();
            for (int i = 0; i < _CommonState.Achievements.Length; i++)
            {
                UpdateAchive(_achievements[i], _CommonState.Achievements[i]);
            }
        }

        void UpdateAchive(Achivement achiveGO, AchieveState achieveState)
        {
            achiveGO.Reward.text = achieveState.Reward.ToString();
            if (achieveState.Progress >= achieveState.MaxProgress)
            {
                achiveGO.CheckBox.gameObject.SetActive(true);
                if (achieveState.Complete)
                {
                    achiveGO.Check.SetActive(true);
                    achiveGO.transform.SetSiblingIndex(AchievementsPanel.Content.childCount - 1);
                }
                else
                {
                    achiveGO.Check.SetActive(false);
                    achiveGO.transform.SetSiblingIndex(0);
                }
                
                achiveGO.Progress.gameObject.SetActive(false);
            }
            else
            {
                achiveGO.Progress.gameObject.SetActive(true);
                achiveGO.Progress.text = $"{achieveState.Progress}/{achieveState.MaxProgress}";
                
                achiveGO.CheckBox.gameObject.SetActive(false);
            }
        }

        void CompleteAchievement(AchieveState achieve)
        {
            if(_rewarding) return;
            
            _CommonState.AchievementsTrophies += achieve.Reward;
            AchievementsPanel.Trophies.text = _CommonState.AchievementsTrophies.ToString();

            var achiveSO = GetAchiveSO(achieve.Key);
            achieve.Level++;
            if (achieve.Level < achiveSO.Levels.Length)
            {
                achieve.Reward = achiveSO.Levels[achieve.Level].Reward;
                achieve.MaxProgress = achiveSO.Levels[achieve.Level].MaxProgress;
            }
            else
            {
                achieve.Complete = true;
            }

            achieve.AchiveGO.Check.SetActive(true);
            achieve.AchiveGO.Progress.gameObject.SetActive(false);
            _rewarding = true;
            StartCoroutine(Reward(achieve, achiveSO));
            
            UpdateAchievements();
        }

        IEnumerator Reward(AchieveState achieve, AchieveSO achiveSO)
        {
            MenuAudioSource.clip = CompleteAchievementSound;
            MenuAudioSource.Play();

            yield return new WaitForSeconds(1f);

            if (achieve.Level < achiveSO.Levels.Length)
            {
                achieve.AchiveGO.Description.text = string.Format(GetLoc(achiveSO), achieve.MaxProgress);
                
                if (achieve.Progress >= achieve.MaxProgress)
                {
                    achieve.AchiveGO.Check.SetActive(false);
                }
                else
                {
                    achieve.AchiveGO.CheckBox.gameObject.SetActive(false);
                    achieve.AchiveGO.Progress.text = $"{achieve.Progress}/{achieve.MaxProgress}";
                    achieve.AchiveGO.Progress.gameObject.SetActive(true);
                }
            }
            else
            {
                achieve.AchiveGO.transform.SetSiblingIndex(AchievementsPanel.Content.childCount - 1);
            }
            
            _rewarding = false;
        }

        string GetAchiveDescription(string key)
        {
            foreach (var achive in AchievementsSO)
            {
                if (achive.name == key)
                {
                    return GetLoc(achive);
                }
            }
            return "";
        }

        string GetLoc(AchieveSO achive)
        {
            foreach (var loc in achive.Localizations)
            {
                if (loc.Language == _CommonState.Language)
                {
                    return loc.Text;
                }
            }

            return achive.Localizations[0].Text;
        }

        AchieveSO GetAchiveSO(string key)
        {
            foreach (var achive in AchievementsSO)
            {
                if (achive.name == key)
                {
                    return achive;
                }
            }
            
            DebugSystem.DebugLog($"Not exist Achive {key}", DebugSystem.Type.Error);
            return null;
        }

        void UpdateAchievementsLanguage()
        {
            for (int i = 0; i < _CommonState.Achievements.Length; i++)
            {
                _achievements[i].Description.text = 
                    string.Format(GetAchiveDescription(_CommonState.Achievements[i].Key),
                    _CommonState.Achievements[i].MaxProgress);
            }
        }

        bool IsHaveCompletedAchievements()
        {
            foreach (var achieve in _CommonState.Achievements)
            {
                if (achieve.Progress >= achieve.MaxProgress && !achieve.Complete)
                {
                    return true;
                }
            }

            return false;
        }
    }
}