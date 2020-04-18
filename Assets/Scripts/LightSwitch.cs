using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public bool lightsOn;
    public Sprite spriteOn;
    public Sprite spriteOff;

    public Light2D[] lights;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = lightsOn ? spriteOn : spriteOff;
            
        foreach (var light in lights)
        {
            light.enabled = lightsOn;
        }
    }


    public void Interact(GameObject obj)
    {
        lightsOn = !lightsOn;
        _renderer.sprite = lightsOn ? spriteOn : spriteOff;

        foreach (var light in lights)
        {
            light.enabled = lightsOn;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        foreach (var light in lights)
        {
            Gizmos.DrawLine(transform.position, light.transform.position);
        }
    }
}
