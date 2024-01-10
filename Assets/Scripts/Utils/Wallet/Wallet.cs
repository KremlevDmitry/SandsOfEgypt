using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Wallet
{
    public static UnityEvent<int> OnSet = new UnityEvent<int>();

    public static int Value
    {
        get
        {
            if (!PlayerPrefs.HasKey("WalletValue"))
            {
                Value = 0;
            }
            return PlayerPrefs.GetInt("WalletValue");
        }
        set
        {
            PlayerPrefs.SetInt("WalletValue", value);
            OnSet.Invoke(value);
        }
    }
}
