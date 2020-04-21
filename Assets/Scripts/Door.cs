using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public SpriteRenderer knobRenderer;
    public Key requiredKey;
    private int _requiredKeyNr;
    private Color _color;

    private BoxCollider2D[] _colliders;
    

    private void Awake()
    {
        _colliders = GetComponents<BoxCollider2D>();
        _requiredKeyNr = gameObject.GetHashCode();
        requiredKey.keyNr = _requiredKeyNr;
        _color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        requiredKey.color = _color;
        requiredKey.SetColor(_color);
        knobRenderer.color = _color;
    }

    public void Interact(GameObject obj)
    {
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
                        DisableColliders();
                        player.RemoveItem(item);
                        MenuManager.Instance.UpdatePlayerSpeakBox(TextManager.Instance.GetDoorOpenedString());
                        AudioManager.Instance.Play("unlocked");
                        this.enabled = false;
                        Destroy(gameObject);
                        return;
                    }
                }
            }
        
            MenuManager.Instance.UpdatePlayerSpeakBox(TextManager.Instance.GetDoorLockedString());
            AudioManager.Instance.Play("locked");
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
