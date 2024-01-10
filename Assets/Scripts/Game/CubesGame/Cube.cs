using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CubesGame
{
    public class Cube : MonoBehaviour
    {
        private const float SPEED = 15f;

        private Image image = null;
        private Image _image => image ??= GetComponent<Image>();
        private Button button = null;
        private Button _button => button ??= GetComponent<Button>();

        public int X = default;
        public int Y = default;
        public Sprite Sprite
        {
            get => _image.sprite;
            set => _image.sprite = value;
        }
        private Game _game = default;

        public static float Height = default;
        public Vector3? StartPosition = null;
        private static int _animationCount = 0;


        private void Awake()
        {
            _button.onClick.AddListener(Click);   
        }

        public void Init(int x, int y, Sprite sprite, Game game)
        {
            _game = game;
            X = x;
            Y = y;
            Sprite = sprite;
        }

        public void MoveToStart(bool isAnimated)
        {
            if (StartPosition == null)
            {
                StartPosition = transform.position;
            }

            if (isAnimated)
            {
                StartCoroutine(MovingToStart());
            }
            else
            {
                transform.position = StartPosition.Value;
            }
        }

        private void Click()
        {
            if (_animationCount > 0) { return; }

            _game?.RemoveCubes(this);
        }

        private IEnumerator MovingToStart()
        {
            _animationCount++;
            while (transform.position != StartPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, StartPosition.Value, SPEED * Time.deltaTime);
                yield return null;
            }
            yield return null;
            _animationCount--;
        }
    }
}