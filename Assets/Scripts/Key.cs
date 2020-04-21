using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public int keyNr;
    public Sprite keySprite;
    public Color color;

    public void SetColor(Color c)
    {
        color = c;
        GetComponent<SpriteRenderer>().color = c;
    }

    public void Interact(GameObject obj)
    {
        var player = obj.GetComponent<PlayerController>();
        
        if (player != null)
        {
            // TODO: maybe generic or a function to call event
            player.AddItem(new KeyItem(keyNr, keySprite, color));
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public class KeyItem : Item
{
    public int keyNr;

    public KeyItem(int key, Sprite spr, Color c)
    {
        keyNr = key;
        sprite = spr;
        color = c;
    }
}
