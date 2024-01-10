using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class ProductButton : MonoBehaviour
{
    [SerializeField]
    private int _id = default;
    [SerializeField]
    private Button _openButton = default;
    [SerializeField]
    private Button _closeButton = default;

    [SerializeField]
    private BuyPage _buyPage = default;
    [SerializeField]
    private UpgradePage _upgradePage = default;

    public static UnityEvent<int> OnIsOpenSet = new UnityEvent<int>(); //id

    [SerializeField]
    private int _defaultReward = default;
    [SerializeField]
    private int _defaultTime = default;

    private static int[] _defaultRewards = new int[] { 0, 0, 0 };
    private static int[] _defaultTimes = new int[] { 0, 0, 0 };

    private void Awake()
    {
        _openButton.onClick.AddListener(OpenUpgrade);
        _closeButton.onClick.AddListener(OpenBuy);

        SetButton();
        OnIsOpenSet.AddListener((id) => { if (id == _id) { SetButton(); } });

        _defaultRewards[_id] = _defaultReward;
        _defaultTimes[_id] = _defaultTime;
    }


    public static bool GetIsOpen(int id)
    {
        return PlayerPrefs.GetInt($"Product{id}IsOpen", 0) == 1;
    }

    public static void SetIsOpen(int id, bool value)
    {
        PlayerPrefs.SetInt($"Product{id}IsOpen", value ? 1 : 0);
        OnIsOpenSet.Invoke(id);
    }

    public static int GetReward(int id)
    {
        return PlayerPrefs.GetInt($"Product{id}Reward", _defaultRewards[id]);
    }

    public static void SetReward(int id, int value)
    {
        PlayerPrefs.SetInt($"Product{id}Reward", value);
    }

    public static int GetTime(int id)
    {
        return PlayerPrefs.GetInt($"Product{id}Time", _defaultTimes[id]);
    }

    public static void SetTime(int id, int value)
    {
        PlayerPrefs.SetInt($"Product{id}Time", value);
    }


    private void OpenBuy()
    {
        _buyPage.Open(_id);
    }

    private void OpenUpgrade()
    {
        if (_remainSeconds <= 0)
        {
            Wallet.Value += GetReward(_id);
            switch (_id)
            {
                case 0:
                    FishingHouseTimeBonus.GetBonus();
                    break;
                case 1:
                    CreekTimeBonus.GetBonus();
                    break;
                case 2:
                    FieldsTimeBonus.GetBonus();
                    break;
            }
        }
        else
        {
            _upgradePage.Open(_id);
        }
    }

    private void SetButton()
    {
        bool isOpen = GetIsOpen(_id);
        _openButton.gameObject.SetActive(isOpen);
        _closeButton.gameObject.SetActive(!isOpen);
    }

    [SerializeField]
    private Text _timerText = default;

    private int _remainSeconds = default;
    private IEnumerator _settingTimer = default;


    private void OnEnable()
    {
        StartCoroutine(_settingTimer = SettingTimer());

        switch (_id)
        {
            case 0:
                FishingHouseTimeBonus.OnGet.AddListener(UpdateRemainSeconds);
                break;
            case 1:
                CreekTimeBonus.OnGet.AddListener(UpdateRemainSeconds);
                break;
            case 2:
                FieldsTimeBonus.OnGet.AddListener(UpdateRemainSeconds);
                break;
        }
    }

    private void OnDisable()
    {
        if (_settingTimer != null)
        {
            StopCoroutine(_settingTimer);
            _settingTimer = null;
        }
    }

    private void UpdateRemainSeconds()
    {
        TimeSpan remainTime = (DailyBonus.LastOpenTime.AddDays(1) - DateTime.Now);
        switch (_id)
        {
            case 0:
                remainTime = (FishingHouseTimeBonus.LastOpenTime.AddSeconds(GetTime(0)) - DateTime.Now);
                break;
            case 1:
                remainTime = (CreekTimeBonus.LastOpenTime.AddSeconds(GetTime(1)) - DateTime.Now);
                break;
            case 2:
                remainTime = (FieldsTimeBonus.LastOpenTime.AddSeconds(GetTime(2)) - DateTime.Now);
                break;
        }
        _remainSeconds = (int)remainTime.TotalSeconds;

        UpdateGraphic();
    }

    private IEnumerator SettingTimer()
    {
        var waitSecond = new WaitForSecondsRealtime(1f);
        UpdateRemainSeconds();
        while (true)
        {
            UpdateGraphic();
            yield return waitSecond;
            _remainSeconds--;
        }
    }

    private void UpdateGraphic()
    {
        if (_remainSeconds > 0)
        {
            int hours = _remainSeconds / 60 / 60;
            int minutes = _remainSeconds / 60 % 60;
            int seconds = _remainSeconds % 60;

            _timerText.text =
                //$"{(hours > 9 ? "" : "0")}{hours}:" +
                $"{(minutes > 9 ? "" : "0")}{minutes}:" +
                $"{(seconds > 9 ? "" : "0")}{seconds}";
        }
        else
        {
            _timerText.text = "TAKE";
        }
    }
}
