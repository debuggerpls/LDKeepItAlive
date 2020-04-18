using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 4f;
    
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider2D;
    private SpriteRenderer _renderer;

    private bool _vInput;
    private float _hInput;
    private String _interactableTag = "Interactable";
    private IInteractable _interactable;
    private bool _canMove = true;

    public List<Item> items;
    public int itemCount;

    public bool carryingItem;
    public bool sitting; 

    public bool AddItem(Item item)
    {
        bool ret = false;
        if (item is KeyItem)
        {
            /*
             * TODO:
             * add item to the list
             * update gui
             */
            items.Add(item);
            ret = true;
        }
        else
        {
            if (carryingItem)
            {
                /*
                 * TODO:
                 * call event that already carrying
                 */
                ret = false;
            }
            else
            {
                /*
                 * TODO: update gui
                 * add item
                 * carryingItem is true
                 */
                items.Add(item);
                carryingItem = true;
                ret = true;
            }
        }

        return ret;
    }

    public void RemoveItem(Item item)
    {

        if (item is KeyItem)
        {
            /*
             * TODO:
             * remove item from the list
             * update gui
             */
            Debug.Log("as keyitem");
        }
        else
        {
            Debug.Log("as normal item");
            carryingItem = false;
        }
        
        items.Remove(item);
    }
    
    public void EnterElevator()
    {
        _collider2D.enabled = false;
        _renderer.enabled = false;
        _canMove = false;
        _rigidbody.gravityScale = 0;
    }

    public void ExitElevator()
    {
        _collider2D.enabled = true;
        _renderer.enabled = true;
        _canMove = true;
        _rigidbody.gravityScale = 10;
    }

    public void EnterSofa()
    {
        _collider2D.enabled = false;
        _canMove = false;
        sitting = true;
        _rigidbody.gravityScale = 0;
        Debug.Log("sitting");
    }
    
    public void ExitSofa()
    {
        _collider2D.enabled = true;
        _canMove = true;
        sitting = false;
        _rigidbody.gravityScale = 10;
        Debug.Log("not sitting");
    }
    
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<BoxCollider2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        
        items = new List<Item>();
    }

    private void Update()
    {
        itemCount = items.Count;
        
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
        _interactable = null;
    }
}
