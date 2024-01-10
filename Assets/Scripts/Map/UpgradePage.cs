using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePage : MonoBehaviour
{
    [System.Serializable]
    public struct Product
    {
        public string Title;
    }


    [SerializeField]
    private Text _titleText = default;
    [SerializeField]
    private Button _timeUpgradeButton = default;
    [SerializeField]
    private Text _timeUpgradeText = default;
    [SerializeField]
    private Button _rewardUpgradeButton = default;
    [SerializeField]
    private Text _rewardUpgradeText = default;
    
    private int _id = default;
    private int _timeUpgradePrice = default;
    private int _rewardUpgradePrice = default;

    [SerializeField]
    private Product[] _products = default;


    private void Awake()
    {
        _timeUpgradeButton.onClick.AddListener(UpgradeTime);
        _rewardUpgradeButton.onClick.AddListener(UpgradeReward);
    }


    public void Open(int id)
    {
        _id = id;

        _timeUpgradePrice = 1000;
        _rewardUpgradePrice = 1000;

        var product = _products[id];
        _titleText.text = product.Title;
        _timeUpgradeText.text = _timeUpgradePrice.ToString();
        _rewardUpgradeText.text = _rewardUpgradePrice.ToString();

        gameObject.SetActive(true);
    }


    private void UpgradeTime()
    {
        if (Wallet.Value >= _timeUpgradePrice)
        {
            Wallet.Value -= _timeUpgradePrice;
            ProductButton.SetTime(_id, (int)(ProductButton.GetTime(_id) * .9f));
            gameObject.SetActive(false);
        }
    }

    private void UpgradeReward()
    {
        if (Wallet.Value >= _rewardUpgradePrice)
        {
            Wallet.Value -= _rewardUpgradePrice;
            ProductButton.SetReward(_id, (int)(ProductButton.GetReward(_id) * 1.1f));
            gameObject.SetActive(false);
        }
    }
}
