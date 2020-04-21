using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 4f;
    public bool moving = false;
    
    public Sprite testSprite;
    
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider2D;
    private SpriteRenderer[] _renderers;
    private Animator _animator;

    private bool _vInput;
    private float _hInput;
    private String _interactableTag = "Interactable";
    private IInteractable _interactable;
    private bool _canMove = true;

    public List<Item> items;

    public bool carryingItem;
    public bool sitting; 

    public bool AddItem(Item item)
    {
        bool ret = false;
        if (item is KeyItem)
        {
            /*
             * TODO:
             * update gui
             * add animation
             */
            items.Add(item);
            ret = true;
            
            AudioManager.Instance.Play("keyPick");
            MenuManager.Instance.UpdatePlayerSpeakBox(TextManager.Instance.GetTookKeyString());
            AddItemToGui(item.sprite, ItemGui.ItemGuiType.Key, ((KeyItem) item).keyNr, item.color);
        }
        else
        {
            if (carryingItem)
            {
                AudioManager.Instance.Play("partFull");
                MenuManager.Instance.UpdatePlayerSpeakBox(TextManager.Instance.GetNotCarryingMoreString());
                ret = false;
            }
            else
            {
                // TODO: add animation
                items.Add(item);
                carryingItem = true;
                ret = true;
                
                AudioManager.Instance.Play("partPick");
                MenuManager.Instance.UpdatePlayerSpeakBox(TextManager.Instance.GetTookComputerPartString());
                AddItemToGui(item.sprite, ItemGui.ItemGuiType.Part, 0, Color.white);
            }
        }

        return ret;
    }

    public void RemoveItem(Item item)
    {
        ItemGui.ItemGuiType type = ItemGui.ItemGuiType.Part;
        int keyNr = 0;
        
        if (item is KeyItem)
        {
            /*
             * TODO:
             * remove item from the list
             * update gui
             */
            type = ItemGui.ItemGuiType.Key;
            keyNr = ((KeyItem) item).keyNr;
            Debug.Log("as keyitem");
        }
        else
        {
            Debug.Log("as normal item");
            carryingItem = false;
        }
        
        items.Remove(item);
        RemoveItemFromGui(type, keyNr);
    }
    
    public void EnterElevator()
    {
        _collider2D.enabled = false;
        SetRenderersActive(false);
        _canMove = false;
        _rigidbody.gravityScale = 0;
    }

    public void ExitElevator()
    {
        _collider2D.enabled = true;
        SetRenderersActive(true);
        _canMove = true;
        _rigidbody.gravityScale = 10;
    }

    public void EnterSofa()
    {
        AudioManager.Instance.Play("sit");
        _collider2D.enabled = false;
        _canMove = false;
        sitting = true;
        _rigidbody.gravityScale = 0;
        Debug.Log("sitting");
        MenuManager.Instance.UpdatePlayerSpeakBox(TextManager.Instance.GetHidingString());
    }
    
    public void ExitSofa()
    {
        AudioManager.Instance.Play("stand");
        _collider2D.enabled = true;
        _canMove = true;
        sitting = false;
        _rigidbody.gravityScale = 10;
        Debug.Log("not sitting");
        MenuManager.Instance.UpdatePlayerSpeakBox(TextManager.Instance.GetStoodUpString());
    }

    private void SetRenderersActive(bool val)
    {
        foreach (var spriteR in _renderers)
        {
            spriteR.enabled = val;
        }
    }

    public void AddItemToGui(Sprite sprite, ItemGui.ItemGuiType type, int keyNr, Color color)
    {
        MenuManager.Instance.AddItemToGui(sprite, type, keyNr, color);
    }
    
    public void RemoveItemFromGui(ItemGui.ItemGuiType type, int keyNr)
    {
        MenuManager.Instance.RemoveItemFromGui(type, keyNr);
    }
    
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<BoxCollider2D>();
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        
        items = new List<Item>();
    }

    private void Update()
    {
        _hInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            _interactable?.Interact(gameObject);    
        }

        if (Input.GetButtonUp("Jump") && sitting)
        {
            ExitSofa();
        }

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && _canMove)
        {
            if (carryingItem)
            {
                foreach (var item in items)
                {
                    if (!(item is KeyItem))
                    {
                        RemoveItem(item);
                        // TODO: event why you doing, stupid?
                        Debug.Log("item removed");
                        AudioManager.Instance.Play("partDrop");
                        break;
                    }
                }
            }
            else
            {
                //TODO: event that not carrying, ya stupid?
                Debug.Log("not carrying anything");
            }
        }
    }

    private void FixedUpdate()
    {
        if (Math.Abs(_hInput) > 0.01f && _canMove)
        {
            Vector2 move = _rigidbody.position;
            move.x += _hInput * movementSpeed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(move);
            moving = true;
            _animator.SetBool("moving", true);
        }
        else
        {
            moving = false;
            _animator.SetBool("moving", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_interactableTag))
        {
            _interactable = other.GetComponent<IInteractable>();
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            if (interactable.GetHashCode() == _interactable.GetHashCode())
                _interactable = null;
        }
        
    }
}
