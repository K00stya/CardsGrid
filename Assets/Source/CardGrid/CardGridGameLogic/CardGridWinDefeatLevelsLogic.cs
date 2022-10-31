using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace CardGrid
{
    public partial class CardGridGame
    {
        float LevelUpSeed = 1f;
        
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
            OpenWin(BattleUI.BattleMenu);
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
        
        void UpdateLevel()
        {
            _CommonState.BattleState.LevelProgress += 1;
            if (_CommonState.BattleState.LevelProgress >= _CommonState.BattleState.MaxLevelProgress)
            {
                _nextLevels++;
                _CommonState.BattleState.MaxLevelProgress = (int) (_CommonState.BattleState.MaxLevelProgress * 1.25f);
                _CommonState.BattleState.LevelProgress = 0;
                _CommonState.BattleState.NumberLevel += 1;
            }

            BattleUI.LevelNumber.text = _CommonState.BattleState.NumberLevel.ToString();
            BattleUI.LevelProgress.SetValueWithoutNotify(_CommonState.BattleState.LevelProgress);
            BattleUI.LevelProgress.maxValue = _CommonState.BattleState.MaxLevelProgress;
            if (_CommonState.BattleState.LevelProgress > _CommonState.BestScore)
                _CommonState.BestScore = _CommonState.BattleState.LevelProgress;
        }

        IEnumerator NextLevel()
        {
            var rewards = BattleUI.NewLevelUp.Reward;

            LevelReward reward = LevelsReward.Rewards[^1];
            for (int i = 0; i < LevelsReward.Rewards.Length; i++)
            {
                if (_CommonState.BattleState.NumberLevel < LevelsReward.Rewards[i].InLevels)
                {
                    reward = LevelsReward.Rewards[i - 1];
                }
            }

            int j = 0;
            for (; j < reward.Rewards.Length; j++)
            {
                rewards[j].sprite = GetCardSO(reward.Rewards[j].Item1).Sprite;
                rewards[j].gameObject.SetActive(true);
            }
            for (; j < rewards.Length; j++)
            {
                rewards[j].gameObject.SetActive(false);
            }

            BattleUI.NewLevelUp.gameObject.SetActive(true);
            for (int i =0; i < reward.Rewards.Length; i++)
            {
                rewards[i].transform.localScale = Vector3.zero;
            }
            
            var panel = BattleUI.NewLevelUp.transform.GetChild(0);
            panel.localScale = Vector3.zero;
            panel.DOScale(Vector3.one, 0.5f);
            yield return new WaitForSeconds(0.5f);
            for (int i =0; i < reward.Rewards.Length; i++)
            {
                yield return rewards[i].transform.DOScale(Vector3.one, LevelUpSeed / reward.Rewards.Length);
                yield return new WaitForSeconds(LevelUpSeed / reward.Rewards.Length);
            }

            BattleUI.NewLevelUp.gameObject.SetActive(false);
            for (int i = 0; i < reward.Rewards.Length; i++)
            {
                yield return AddItem(reward.Rewards[i]);
            }
        }

        IEnumerator AddItem((CT type, int maxQuantity) item)
        {
            _itemsRecession = true;
            var cardState = new CardState();
            var cardSo = GetCardSO(item.type);
            cardState.CardSO = cardSo;
            cardState.Quantity = Random.Range(1, item.maxQuantity + 1);
            AddItemInInventory(cardState);
            
            foreach (var card in _CommonState.BattleState.Inventory.Items)
            {
                MoveCardToSelfPosition(card, BattleObjects.Inventory);
            }

            yield return new WaitForSeconds(SpeedRecession);
        }
    }
}