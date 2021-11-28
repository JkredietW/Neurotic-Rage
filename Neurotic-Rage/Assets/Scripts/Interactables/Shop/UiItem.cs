using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiItem : MonoBehaviour
{
    public ShopItem heldItem;
    public void Setup(ShopItem _item)
    {
        heldItem = _item;
        if(heldItem != null)
        {
            GetComponent<Image>().sprite = heldItem.itemSprite;
            GetComponent<Image>().color = Color.white;
            GetComponentInChildren<TextMeshProUGUI>().text = heldItem.moneyValue.ToString();
        }
        else
        {
            GetComponent<Image>().sprite = null;
            GetComponent<Image>().color = Color.clear;
            GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
        }
    }
}
