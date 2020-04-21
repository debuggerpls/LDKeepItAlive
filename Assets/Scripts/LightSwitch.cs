using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public bool lightsOn;
    public Color onColor = Color.white;
    public Color offColor = Color.red;

    public Light2D[] lights;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = lightsOn ? onColor : offColor;
            
        foreach (var light in lights)
        {
            light.enabled = lightsOn;
        }
    }


    public void Interact(GameObject obj)
    {
        lightsOn = !lightsOn;
        _renderer.color = lightsOn ? onColor : offColor;

        foreach (var light in lights)
        {
            light.enabled = lightsOn;
        }

        AudioManager.Instance.Play("light");
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
