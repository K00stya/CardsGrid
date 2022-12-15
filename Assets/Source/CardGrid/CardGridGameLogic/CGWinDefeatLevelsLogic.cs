using System;
using System.Collections;
using DG.Tweening;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Leaderboard;
using InstantGamesBridge.Modules.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGrid
{
    public partial class CardGridGame
    {
        float LevelUpSeed = 0.7f;

        void CheckWinOrDefeat(CardState[,] cells, CardState[,] items)
        {
            bool enemiesExist = false;
            foreach (var cell in cells)
            {
                if (cell.ScrObj != null && cell.ScrObj.Type == TypeCard.Crystal && cell.Quantity > 0)
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

            _CommonState.GetCurrentLevel().Complete ++;
        }

        private bool authAsked;
        (CT, int)[] _AdRewards;
        int rewardsLeft;

        void Defeat()
        {
            Save();
            DebugSystem.DebugLog("Defeat", DebugSystem.Type.Battle);

            MenuAudioSource.clip = DefeateSound;
            MenuAudioSource.Play();

            if (rewardsLeft > 0)
            {
                BattleUI.EndItemsReward.gameObject.SetActive(true);
                var rewardsImages = BattleUI.EndItemsReward.Reward;
                _AdRewards = GenerateRandomRewards(5, 5);
                int j = 0;
                for (; j < _AdRewards.Length; j++)
                {
                    rewardsImages[j].sprite = GetCardSO(_AdRewards[j].Item1).Sprite;
                    rewardsImages[j].gameObject.SetActive(true);
                }

                for (; j < rewardsImages.Length; j++)
                {
                    rewardsImages[j].gameObject.SetActive(false);
                }
            }
            else
            {
                BattleUI.EndItemsReward.gameObject.SetActive(false);
                OpenDefeat();
            }

            _inputActive = false;
        }

        IEnumerator AddRewardedItems()
        {
            BattleUI.EndItemsReward.gameObject.SetActive(false);
            for (int i = 0; i < _AdRewards.Length; i++)
            {
                yield return AddItem(_AdRewards[i]);
            }

            _inputActive = true;
        }
        
        (CT, int)[] GenerateRandomRewards(int quantity = 0, int allDamage = 0)
        {
            (CT, int)[] Rewards;

            int numberRewards;
            switch (0)//_CommonState.BattleState.NumberLevel)
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
            if (quantity != 0)
                numberRewards += quantity;
            for (int i = 0; i < numberRewards && i < Rewards.Length; i++)
            {
                int power;
                if (allDamage == 0)
                    power = Random.Range(1, _startMaxCellQuantity + 1);
                else
                    power = allDamage;
                
                switch (Random.Range(0, 1f))
                {
                    case <= 0.1f:
                        Rewards[i] = (CT.SwordHor, power);
                        break;
                    case <= 0.2f:
                        Rewards[i] = (CT.SwordVer, power);
                        break;
                    case <= 0.3f:
                        Rewards[i] = (CT.SwordL_R, power);
                        break;
                    case <= 0.4f:
                        Rewards[i] = (CT.SwordR_L, power);
                        break;
                    case <= 0.5f:
                        Rewards[i] = (CT.Swords, power);
                        break;
                    case <= 0.6f:
                        Rewards[i] = (CT.Bomb, power);
                        break;
                    default:
                        Rewards[i] = (CT.Hammer, power);
                        break;
                }
            }

            return Rewards;
        }
        
        IEnumerator AddItem((CT type, int quantity) item)
        {
            _itemsRecession = true;
            var cardState = new CardState();
            var cardSo = GetCardSO(item.type);
            cardState.ScrObj = cardSo;
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