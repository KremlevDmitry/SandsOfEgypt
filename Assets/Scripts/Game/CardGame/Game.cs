using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private Card[] _cards = default;
        [SerializeField]
        private Sprite _closeSprite = default;
        [SerializeField]
        private Sprite[] _openSprites = default;
        [SerializeField]
        private Timer _timer = default;

        public bool ReadyToClick => !(_openCards.Item1 != null && _openCards.Item2 != null);
        private (Card, Card) _openCards = (null, null);

        private int _openCounter = default;

        [SerializeField]
        private GameObject _winObject = default;
        [SerializeField]
        private Text _rewardText = default;
        [SerializeField]
        private GameObject _loseObject = default;


        private void OnEnable()
        {
            StartGame();
        }

        private void OnDisable()
        {
            FinishGame();
        }


        public void OnOpen(Card card)
        {
            if (_openCards.Item1 == null)
            {
                _openCards.Item1 = card;
            }
            else if (_openCards.Item2 == null)
            {
                _openCards.Item2 = card;

                if (_openCards.Item1.Sprite == _openCards.Item2.Sprite)
                {
                    _openCards = (null, null);

                    _openCounter += 2;

                    if (_openCounter == _cards.Length)
                    {
                        Win();
                    }
                }
                else
                {
                    _openCards.Item1.Close();
                    _openCards.Item2.Close();

                    _openCards = (null, null);
                }
            }
        }

        private void StartGame()
        {
            _openCounter = 0;
            FillCards();
            _timer.StartTimer();
            _timer.OnTimeOff.AddListener(Lose);
        }

        private void FinishGame()
        {
            _timer.StopTimer();
            _timer.OnTimeOff.RemoveListener(Lose);
        }

        private void Win()
        {
            FinishGame();

            _rewardText.text = "500";
            Wallet.Value += 500;
            _winObject.SetActive(true);
        }

        private void Lose()
        {
            FinishGame();

            _loseObject.SetActive(true);
        }

        private void FillCards()
        {
            var cards = new List<Card>(_cards);
            cards.Shuffle();
            var sprites = new List<Sprite>(_openSprites);
            sprites.Shuffle();

            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].Init(this, sprites[i / 2], _closeSprite);
            }
        }
    }
}
