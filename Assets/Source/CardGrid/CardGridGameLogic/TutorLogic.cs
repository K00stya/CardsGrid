using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace CardGrid
{
    public partial class CardGridGame
    {
        public GameObject TutorHandObj;
        public GameObject Highlight;
        
        void TutorHand()
        {
            if(_CommonState.BattleState.LevelID < BattleState.CommonLevelID) return;
            
            if (!_inputActive || _CommonState.BattleState.CurrentTutorial.Count == 0)
            {
                TutorHandObj.SetActive(false);
                Highlight.gameObject.SetActive(false);
                _tutorActive = false;
                TutorHandObj.transform.DOKill();
                return;
            }

            if (_CommonState.BattleState.CurrentTutorial.Count > 0)
            {
                var tutor = _CommonState.BattleState.CurrentTutorial.First();
                if (tutor.RotateLeft)
                {
                    _tutorActive = true;
                    Highlight.gameObject.SetActive(true);
                    Highlight.transform.position = Tutorials.LeftRotate.position;
                    return;
                }
                else if (tutor.RotateRight)
                {
                    _tutorActive = true;
                    Highlight.gameObject.SetActive(true);
                    Highlight.transform.position = Tutorials.RightRotate.position;
                    return;
                }

                var firstPos = BattleObjects.Inventory.GetCellSpacePosition(tutor.ItemPosition);
                var secondPos = BattleObjects.Field.GetCellSpacePosition(tutor.FieldPosition);
                if (!_tutorActive)
                {
                    TutorHandObj.SetActive(true);
                    Highlight.gameObject.SetActive(true);
                    _tutorActive = true;
                    MoveTutorHandBetween(firstPos, secondPos);
                }

                if (_selectedCard != null)
                {
                    Highlight.gameObject.transform.position = secondPos + new Vector3(0, 0.5f,0);
                }
                else
                {
                    Highlight.gameObject.transform.position = firstPos + new Vector3(0, 0.5f,0);
                }
            }
        }

        void GreyItems()
        {
            if (_CommonState.BattleState.Inventory.Items != null)
                if (!_inputActive)
                {
                    foreach (var item in _CommonState.BattleState.Inventory.Items)
                    {
                        item.GameObject.Sprite.color = Color.gray;
                    }
                }
                else
                {
                    foreach (var item in _CommonState.BattleState.Inventory.Items)
                    {
                        item.GameObject.Sprite.color = Color.white;
                    }
                }
        }

        void MoveTutorHandBetween(Vector3 firstPos, Vector3 secondPos)
        {
            TutorHandObj.transform.position = firstPos + new Vector3(-0.7f,1f,0.5f);
            TutorHandObj.transform.DOMove(secondPos + new Vector3(-0.7f,1f,0.5f), 2f)
                .SetDelay(1f)
                .OnComplete(() => MoveTutorHandBetween(firstPos, secondPos));
        }

        List<GameObject> tutors = new List<GameObject>(5);
        void ActivateTextTutor()
        {
            _inputActive = false;
            PlayerClick += NextTextTutor;
            if(WithQuantity)
                tutors.AddRange(Tutorials.QuantityTutors);
            tutors.AddRange(Tutorials.ClassicTutors);
            tutors[0].SetActive(true);
        }

        void NextTextTutor()
        {
            tutors[0].SetActive(false);
            tutors.RemoveAt(0);
            if(tutors.Count > 0)
            {
                tutors[0].SetActive(true);
            }
            else
            {
                PlayerClick -= NextTextTutor;
                
                _inputActive = true;
            }
        }
    }
}