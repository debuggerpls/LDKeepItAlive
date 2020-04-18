using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Computer : MonoBehaviour, IInteractable
{

    public bool requiresPart;
    public bool isWorking;
    public Sprite working;
    public Sprite notWorking;

    private SpriteRenderer _renderer;
    private BoxCollider2D _collider2D;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<BoxCollider2D>();
        
        ComputerRepaired(isWorking);
    }

    public void ComputerRepaired(bool value)
    {
        isWorking = value;
        _renderer.sprite = value ? working : notWorking;
        _collider2D.enabled = !value;
        this.enabled = !value;
    }

    public void Interact(GameObject obj)
    {
        Debug.Log("Computer triggered");
        if (!requiresPart)
        {
            // TODO: interaction start for player
            ComputerRepaired(true);
            return;
        }
        else
        {
            var player = obj.GetComponent<PlayerController>();
            if (player != null)
            {
                foreach (var item in player.items)
                {
                    if (item is ComputerPart)
                    {
                        /*
                         * change to working sprite
                         * disable collider
                         * remove item from player
                         * turn off component
                         * start interaction
                         */
                        
                        player.RemoveItem(item);
                        ComputerRepaired(true);

                        return;
                    }
                }
            }
        }
    }
}

[Serializable]
public class ComputerPart : Item
{
    public ComputerPart(Sprite spr)
    {
        sprite = spr;
    }
}
