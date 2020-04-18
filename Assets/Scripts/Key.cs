using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public int keyNr;
    public Sprite keySprite;

    public void Interact(GameObject obj)
    {
        var player = obj.GetComponent<PlayerController>();
        
        if (player != null)
        {
            // TODO: maybe generic or a function to call event
            player.AddItem(new KeyItem(keyNr, keySprite));
            //player.items.Add(new KeyItem(keyNr, keySprite));
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public class KeyItem : Item
{
    public int keyNr;

    public KeyItem(int key, Sprite spr)
    {
        keyNr = key;
        sprite = spr;
    }
}
