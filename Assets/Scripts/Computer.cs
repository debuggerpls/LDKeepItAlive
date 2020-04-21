using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Computer : MonoBehaviour, IInteractable
{
    public enum Owner
    {
        None,
        Delegator,
        Manager
    }

    public Owner ownedBy = Owner.None;
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
        
        //ComputerRepaired(isWorking);
        if (isWorking)
        {
            _renderer.sprite = isWorking ? working : notWorking;
            _collider2D.enabled = false;
            this.enabled = false;
        }
        else
        {
            ComputerRepaired(false, ownedBy, requiresPart);
        }
    }

    public void ComputerRepaired(bool value, Owner owner, bool reqPart)
    {
        isWorking = value;
        ownedBy = owner;
        requiresPart = reqPart;
        _renderer.sprite = value ? working : notWorking;
        _collider2D.enabled = !value;
        
        // TODO: send owner type
        if (value)
        {
            GameManager.Instance?.ComputerFix(owner);
        }
        else
        {
            GameManager.Instance?.ComputerBroke(owner);
        }
        
        this.enabled = !value;
    }

    public void Interact(GameObject obj)
    {
        //Debug.Log("Computer triggered");
        if (!requiresPart)
        {
            // TODO: interaction start for player
            ComputerRepaired(true, ownedBy, requiresPart);
            AudioManager.Instance.Play("fixed");
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
                        player.RemoveItem(item);
                        ComputerRepaired(true, ownedBy, true);
                        AudioManager.Instance.Play("fixed");
                        return;
                    }
                }
                AudioManager.Instance.Play("partReq");
                MenuManager.Instance.UpdatePlayerSpeakBox(TextManager.Instance.GetRequiresPart());
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
