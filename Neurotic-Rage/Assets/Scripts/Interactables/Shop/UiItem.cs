using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        }
        else
        {
            GetComponent<Image>().sprite = null;
            GetComponent<Image>().color = Color.clear;
        }
    }
}
