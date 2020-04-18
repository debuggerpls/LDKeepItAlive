using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Key requiredKey;
    private int _requiredKeyNr;
    
    private BoxCollider2D[] _colliders;
    

    private void Awake()
    {
        _colliders = GetComponents<BoxCollider2D>();
        _requiredKeyNr = gameObject.GetHashCode();
        requiredKey.keyNr = _requiredKeyNr;
    }

    public void Interact(GameObject obj)
    {
        //Debug.Log("Door trigger");
        var player = obj.GetComponent<PlayerController>();
        if (player != null)
        {
            foreach (var item in player.items)
            {
                if (item is KeyItem)
                {
                    int keyNr = ((KeyItem) item).keyNr;
                    if (keyNr == _requiredKeyNr)
                    {
                        /*
                         * change to open sprite
                         * disable colliders
                         * remove item from player
                         */
                        DisableColliders();
                        player.RemoveItem(item);
                        //player.items.Remove(item);
                        this.enabled = false;
                        return;
                    }
                }
            }
        }

    }

    private void DisableColliders()
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (requiredKey != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, requiredKey.transform.position);
        }
    }
}
