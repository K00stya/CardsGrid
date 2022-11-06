using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGrid
{
    public partial class CardGridGame
    {
        float LevelUpSeed = 0.5f;

        void CheckWinOrDefeat(CardState[,] cells, CardState[,] items)
        {
            bool enemiesExist = false;
            foreach (var cell in cells)
            {
                if (cell.CardSO != null && cell.CardSO.Type == TypeCard.Enemy && cell.Quantity > 0)
                {
                    enemiesExist = true;
                    break;
                }
            }

            if (!enemiesExist)
            {
                Win();
                return;
            }

            bool haveItems = false;
            foreach (var item in items)
            {
                if (item.Quantity > 0)
                {
                    haveItems = true;
                    break;
                }
            }

            if (!haveItems)
            {
                Defeat();
            }
        }

        void Win()
        {
            DebugSystem.DebugLog("Win", DebugSystem.Type.Battle);
            StopAllCoroutines();
            DOTween.KillAll();

            MenuAudioSource.clip = WinSound;
            MenuAudioSource.Play();

            int id = _CommonState.BattleState.LevelID;
            if (_CommonState.BattleState.LevelID >= BattleState.CommonLevelID)
            {
                id = _CommonState.BattleState.LevelID - BattleState.CommonLevelID;
            }

            _CommonState.Levels[id].Complete = true;

            if (!_CommonState.Levels[id].Complete)
            {
                _CommonState.Levels[id].Complete = true;
                _CommonState.Levels[id].Stars++;
            }

            //OpenWin(BattleUI.BattleMenu);
            StartCoroutine(LevelComplete());
        }

        //TODO TRANSLATE
        IEnumerator LevelComplete()
        {
            _inputActive = false;
            BattleUI.LevelCompletePanel.gameObject.SetActive(true);
            var id = _CommonState.BattleState.GetRealLevelID() + 1;
            BattleUI.LevelNumberComplete.text = $"LEVEL {id} COMPLETE!";
            BattleUI.LevelCompletePanel.localScale = Vector3.zero;
            BattleUI.LevelCompletePanel.DOScale(Vector3.one, 1f);
            yield return new WaitForSeconds(4f);
            
            BattleUI.LevelCompletePanel.gameObject.SetActive(false);
            
            EndBattle();
            if (_CommonState.BattleState.GetRealLevelID() < _CommonState.Levels.Length - 1)
            {
                StartNewBattle(_CommonState.BattleState.LevelID + 1);
            }
            else
            {
                StartNewBattle(0);
            }
            
            _inputActive = true;
        }

        void Defeat()
        {
            DebugSystem.DebugLog("Defeat", DebugSystem.Type.Battle);

            MenuAudioSource.clip = DefeateSound;
            MenuAudioSource.Play();

            OpenDefeat(BattleUI.BattleMenu);
            _CommonState.InBattle = false;
            _inputActive = false;
        }

        void UpdateLevel(int addProgress = 1)
        {
            _CommonState.BattleState.LevelProgress += addProgress;
            if (_CommonState.BattleState.LevelProgress >= _CommonState.BattleState.MaxLevelProgress)
            {
                _nextLevels++;
                _CommonState.BattleState.MaxLevelProgress = (int) (_CommonState.BattleState.MaxLevelProgress * 1.15f);
                _CommonState.BattleState.LevelProgress = 0;
                _CommonState.BattleState.NumberLevel += 1;

                MenuAudioSource.clip = LevelUpSound;
                MenuAudioSource.Play();
            }

            BattleUI.LevelNumber.text = _CommonState.BattleState.NumberLevel.ToString();
            DOTween.To(() => BattleUI.LevelProgress.value,
                x => BattleUI.LevelProgress.value = x, _CommonState.BattleState.LevelProgress, 0.5f);
            BattleUI.LevelProgress.maxValue = _CommonState.BattleState.MaxLevelProgress;

            if (WithQuantity)
            {
                if (_CommonState.BattleState.LevelProgress > _CommonState.BestLevelQuantity)
                    _CommonState.BestLevelQuantity = _CommonState.BattleState.NumberLevel;
            }
            else
            {
                if (_CommonState.BattleState.LevelProgress > _CommonState.BestLevelClassic)
                    _CommonState.BestLevelClassic = _CommonState.BattleState.NumberLevel;
            }
        }
        
        void GenerateItemsTypes()
        {
            var v = Enum.GetValues (typeof (CT));
            for (int i = 0; i < v.Length; i++)
            {
                ColorTypes.Add( (ColorType) v.GetValue(i) );
            }
        }
        
        (CT, int)[] GenerateRandomRewards()
        {
            (CT, int)[] Rewards;

            int numberRewards;
            switch (_CommonState.BattleState.NumberLevel)
            {
                case <= 5:
                    numberRewards = 1;
                    break;
                case <= 10:
                    numberRewards = 2;
                    break;
                case <= 15:
                    numberRewards = 3;
                    break;
                case <= 20:
                    numberRewards = 4;
                    break;
                case <= 25:
                    numberRewards = 5;
                    break;
                default:
                    numberRewards = 5;
                    break;
            }
            Rewards = new (CT, int)[numberRewards];
            for (int i = 0; i < numberRewards; i++)
            {
                int quantity = Random.Range(1, _startMaxCellQuantity + 1);
                switch (Random.Range(0, 1f))
                {
                    case <= 0.1f:
                        Rewards[i] = (CT.SH, quantity);
                        break;
                    case <= 0.2f:
                        Rewards[i] = (CT.SV, quantity);
                        break;
                    case <= 0.3f:
                        Rewards[i] = (CT.SLR, quantity);
                        break;
                    case <= 0.4f:
                        Rewards[i] = (CT.SRL, quantity);
                        break;
                    case <= 0.5f:
                        Rewards[i] = (CT.Sw, quantity);
                        break;
                    case <= 0.6f:
                        Rewards[i] = (CT.Bo, quantity);
                        break;
                    default:
                        Rewards[i] = (CT.Ha, quantity);
                        break;
                }
            }

            return Rewards;
        }
        

        IEnumerator NextLevel()
        {
            var rewardsImages = BattleUI.NewLevelUp.Reward;

            (CT, int)[] rewards = GenerateRandomRewards();

            int j = 0;
            for (; j < rewards.Length; j++)
            {
                rewardsImages[j].sprite = GetCardSO(rewards[j].Item1).Sprite;
                rewardsImages[j].gameObject.SetActive(true);
            }

            for (; j < rewardsImages.Length; j++)
            {
                rewardsImages[j].gameObject.SetActive(false);
            }

            BattleUI.NewLevelUp.gameObject.SetActive(true);
            for (int i = 0; i < rewards.Length; i++)
            {
                rewardsImages[i].transform.localScale = Vector3.zero;
            }

            var panel = BattleUI.NewLevelUp.transform.GetChild(0);
            panel.localScale = Vector3.zero;
            panel.DOScale(Vector3.one, 0.5f);
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < rewards.Length; i++)
            {
                yield return rewardsImages[i].transform.DOScale(Vector3.one, LevelUpSeed / rewards.Length);
                yield return new WaitForSeconds(LevelUpSeed / rewards.Length);
            }
            yield return new WaitForSeconds(1f);

            BattleUI.NewLevelUp.gameObject.SetActive(false);
            for (int i = 0; i < rewards.Length; i++)
            {
                yield return AddItem(rewards[i]);
            }
        }


        void LoadRewards()
        {
            LevelReward reward = LevelsReward.Rewards[^1];
            for (int i = 0; i < LevelsReward.Rewards.Length; i++)
            {
                if (_CommonState.BattleState.NumberLevel < LevelsReward.Rewards[i].InLevels)
                {
                    reward = LevelsReward.Rewards[i - 1];
                }
            }
        }

        IEnumerator AddItem((CT type, int quantity) item)
        {
            _itemsRecession = true;
            var cardState = new CardState();
            var cardSo = GetCardSO(item.type);
            cardState.CardSO = cardSo;
            cardState.Quantity = item.quantity;
            AddItemInInventory(cardState);

            foreach (var card in _CommonState.BattleState.Inventory.Items)
            {
                MoveCardToSelfPosition(card, BattleObjects.Inventory);
            }

            yield return new WaitForSeconds(SpeedRecession);
        }
    }
}