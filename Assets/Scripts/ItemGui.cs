using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGui : MonoBehaviour
{
    public enum ItemGuiType
    {
        Key,
        Part
    }

    public int keyNr;
    public ItemGuiType type;

    private Image _image;

    public void SetItem(Sprite sprite, ItemGuiType t, int key, Color color)
    {
        type = t;
        keyNr = key;
        _image.sprite = sprite;
        _image.color = color;
    }
    
    public bool RemoveItem(ItemGuiType t, int key)
    {
        if (t == ItemGuiType.Part && type == ItemGuiType.Part)
        {
            Destroy(gameObject, 0.01f);
            return true;
        }
        else
        {
            if (key == keyNr)
            {
                Debug.Log("destroying key gui");
                Destroy(gameObject, 0.01f);
                return true; 
            }
            else
            {
                Debug.Log("not destroying key gui");
                return false;
            }
        }
    }
    
    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    
}
