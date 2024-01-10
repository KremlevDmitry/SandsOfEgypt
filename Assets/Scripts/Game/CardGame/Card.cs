using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class Card : MonoBehaviour
    {
        private Image image = null;
        private Image _image => image ??= GetComponent<Image>();
        public Sprite Sprite => _image.sprite;
        private Button button = null;
        private Button _button => button ??= GetComponent<Button>();

        private Game _game = default;

        private Sprite _closeSprite = default;
        private Sprite _openSprite = default;

        private bool _isAnimating = false;


        private void Awake()
        {
            _button.onClick.AddListener(Click);
        }

        public void Open()
        {
            StartCoroutine(Opening(true));
        }

        public void Close()
        {
            StartCoroutine(Opening(false));
        }

        public void Init(Game game, Sprite openSprite, Sprite closeSprite)
        {
            _game = game;

            _openSprite = openSprite;
            _closeSprite = closeSprite;

            SetSprite(_closeSprite);
        }

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }


        private void Click()
        {
            if (Sprite != _closeSprite || _isAnimating || !_game.ReadyToClick) { return; }
            if (_image.sprite == _closeSprite)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        private IEnumerator Opening(bool toOpen)
        {
            _isAnimating = true;
            if (!toOpen)
            {
                yield return new WaitForSeconds(.15f);
            }
            var scale = Vector3.one;
            float time = .15f;
            for (float t = 0; t < time; t += Time.deltaTime)
            {
                SetScale(1 - t / time);
                yield return null;
            }
            SetScale(0);
            yield return null;

            SetSprite(toOpen ? _openSprite : _closeSprite);

            for (float t = 0; t < time; t += Time.deltaTime)
            {
                SetScale(t / time);
                yield return null;
            }
            SetScale(1);

            _isAnimating = false;
            if (toOpen)
            {
                _game.OnOpen(this);
            }



            void SetScale(float width)
            {
                scale.x = width;
                transform.localScale = scale;
            }
        }
    }
}
