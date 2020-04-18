using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sofa : MonoBehaviour, IInteractable
{

    public void Interact(GameObject obj)
    {
        var player = obj.GetComponent<PlayerController>();
        if (player != null)
        {
            player.EnterSofa();
        }
    }
}
