using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CubesGame
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private Cube[] _cubes = default;
        private List<Cube> _list = null;
        [SerializeField]
        private Sprite[] _enableCubeSprite = default;

        [SerializeField]
        private Timer _timer = default;

        [SerializeField]
        private GameObject _winObject = default;
        [SerializeField]
        private Text _rewardText = default;

        [SerializeField]
        private Text[] _rewardTexts = default;
        private int[] _rewards = default;


        private void Awake()
        {
            Cube.Height = _cubes[0].transform.position.y - _cubes[15].transform.position.y;
        }

        private void OnEnable()
        {
            StartGame();
        }

        private void OnDisable()
        {
            FinishGame();
        }

        private void FillList(Cube cube, List<Cube> list)
        {
            if (list.Contains(cube)) { return; }
            list.Add(cube);
            if (cube.X > 0)
            {
                var next = _cubes[cube.Y * 15 + (cube.X - 1)];
                if (next.Sprite == cube.Sprite)
                {
                    FillList(next, list);
                }
            }
            if (cube.X < 14)
            {
                var next = _cubes[cube.Y * 15 + (cube.X + 1)];
                if (next.Sprite == cube.Sprite)
                {
                    FillList(next, list);
                }
            }
            if (cube.Y > 0)
            {
                var next = _cubes[(cube.Y - 1) * 15 + cube.X];
                if (next.Sprite == cube.Sprite)
                {
                    FillList(next, list);
                }
            }
            if (cube.Y < 6)
            {
                var next = _cubes[(cube.Y + 1) * 15 + cube.X];
                if (next.Sprite == cube.Sprite)
                {
                    FillList(next, list);
                }
            }
        }

        private void UpdateRewards(Sprite sprite, int value)
        {
            int index = 0;
            for (int i = 0; i < _enableCubeSprite.Length; i++)
            {
                if (_enableCubeSprite[i] == sprite)
                {
                    index = i;
                }
            }
            _rewards[index] += value;

            for (int i = 0; i < _rewardTexts.Length; i++)
            {
                _rewardTexts[i].text = _rewards[i].ToString();
            }
        }

        private void CleanRewards()
        {
            _rewards = new int[] { 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < _rewardTexts.Length; i++)
            {
                _rewardTexts[i].text = _rewards[i].ToString();
            }
        }

        public void RemoveCubes(Cube currentCube)
        {
            FillList(currentCube, _list = new List<Cube>());
            if (_list.Count < 2) { return; }

            UpdateRewards(_list[0].Sprite, _list.Count);

            foreach (var cube in _list)
            {
                cube.Sprite = null;
            }

            for (int i = 0; i < 15; i++)
            {
                {
                    int count = 0;
                    for (int j = 0; j < 7; j++)
                    {
                        if (_cubes[j * 15 + i].Sprite == null)
                        {
                            count++;
                        }
                    }
                    for (int j = 0; j < count; j++)
                    {
                        _cubes[j * 15 + i].transform.position += Cube.Height * count * Vector3.up;
                    }
                    int qwe = 0;
                    for (int j = count; j < 7; j++)
                    {
                        for (int k = qwe; k < 7; k++)
                        {
                            if (_cubes[k * 15 + i].Sprite != null)
                            {
                                qwe = k + 1;
                                _cubes[j * 15 + i].transform.position = _cubes[k * 15 + i].StartPosition.Value;
                                break;
                            }
                        }
                    }
                }
                for (int j = 6; j >= 0; j--)
                {
                    var current = _cubes[j * 15 + i];
                    if (current.Sprite == null)
                    {
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (_cubes[k * 15 + i].Sprite != null)
                            {
                                (_cubes[k * 15 + i].Sprite, current.Sprite) = (current.Sprite, _cubes[k * 15 + i].Sprite);
                                break;
                            }
                        }
                    }
                }
            }

            foreach (var cube in _cubes)
            {
                if (cube.Sprite == null)
                {
                    cube.Sprite = GetRandomSprite();
                }
                cube.MoveToStart(true);
            }

            _list = null;
        }

        private void StartGame()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    _cubes[i * 15 + j].Init(j, i, GetRandomSprite(), this);
                    _cubes[i * 15 + j].MoveToStart(false);
                }
            }

            _timer.StartTimer();
            _timer.OnTimeOff.AddListener(Win);

            CleanRewards();
        }

        private void FinishGame()
        {
            _timer.StopTimer();
            _timer.OnTimeOff.RemoveListener(Win);
        }

        private void Win()
        {
            FinishGame();

            int reward = 12 * _rewards[0] + 8 * _rewards[1] + 5 * _rewards[2] + 3 * _rewards[3] + 2 * _rewards[4] + 1 * _rewards[5];
            _winObject.SetActive(true);
            _rewardText.text = reward.ToString();
            Wallet.Value += reward;
        }

        private Sprite GetRandomSprite()
        {
            return _enableCubeSprite[Random.Range(0, _enableCubeSprite.Length)];
        }
    }

}
