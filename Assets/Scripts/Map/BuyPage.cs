using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyPage : MonoBehaviour
{
    [System.Serializable]
    public struct Product
    {
        public string Title;
        public string Description;
        public int Price;
    }

    [SerializeField]
    private Button _button = default;
    [SerializeField]
    private Text _priceText = default;
    private int _id = default;
    private int _price = default;

    [SerializeField]
    private Text _titleText = default;
    [SerializeField]
    private Text _descriptionText = default;

    [SerializeField]
    private Product[] _products = default;


    private void Awake()
    {
        _button.onClick.AddListener(Buy);
    }


    public void Open(int id)
    {
        _id = id;

        var product = _products[id];
        _price = product.Price;
        _priceText.text = _price.ToString();
        _titleText.text = product.Title;
        _descriptionText.text = product.Description;

        gameObject.SetActive(true);
    }


    private void Buy()
    {
        if (Wallet.Value >= _price)
        {
            Wallet.Value -= _price;
            ProductButton.SetIsOpen(_id, true);
            gameObject.SetActive(false);
        }
    }
}
