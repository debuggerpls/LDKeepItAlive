using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerParts : MonoBehaviour, IInteractable
{
    public Sprite partSprite;

    public void Interact(GameObject obj)
    {
        var player = obj.GetComponent<PlayerController>();
        if (player != null)
        {
            bool added = player.AddItem(new ComputerPart(partSprite));
            Debug.Log(added? "part added" : "Part not added");
        }
    }
}
