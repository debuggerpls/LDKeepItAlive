using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour, IInteractable
{
    public ElevatorState state = ElevatorState.NotReady;
    public ElevatorController destination;
    public int floorsToDestination;
    public float busyTimePerFloor = 1f;

    public Sprite spriteNotReady;
    public Sprite spriteReady;
    public Sprite spriteBusy;

    private SpriteRenderer _renderer;

    private float _busyTime;

    public float BusyTime
    {
        get => _busyTime;
        set => _busyTime = value;
    }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (state == ElevatorState.Ready)
        {
            destination.SetState(ElevatorState.NotReady);
            destination.SetDestination(this);
            _busyTime = floorsToDestination * busyTimePerFloor;
            destination.BusyTime = _busyTime;
            _renderer.sprite = spriteReady;
        }
    }

    public void SetDestination(ElevatorController dest)
    {
        destination = dest;
    }
    
    public void SetState(ElevatorState newState)
    {
        state = newState;
        if (state == ElevatorState.Ready)
            _renderer.sprite = spriteReady;
        else if (state == ElevatorState.NotReady)
            _renderer.sprite = spriteNotReady;
        else
            _renderer.sprite = spriteBusy;
    }

    public IEnumerator SetDelayedState(ElevatorState newState, float time)
    {
        SetState(ElevatorState.Busy);
        yield return new WaitForSeconds(time);
        SetState(newState);
    }
    
    public IEnumerator PlayerEnteredElevator(PlayerController player, float time)
    {
        player.EnterElevator();
        yield return new WaitForSeconds(time);
        player.transform.position = destination.transform.position;
        player.ExitElevator();
    }

    public void Interact(GameObject obj)
    {
        AudioManager.Instance.Play("elevator");
        
        if (state == ElevatorState.Ready)
        {
            Debug.Log("Ready called");
            StartCoroutine(destination.SetDelayedState(ElevatorState.Ready, _busyTime));
            StartCoroutine(SetDelayedState(ElevatorState.NotReady, _busyTime));
            StartCoroutine(PlayerEnteredElevator(obj.GetComponent<PlayerController>(), _busyTime));
        }
        else if (state == ElevatorState.NotReady)
        {
            Debug.Log("NotReady called");
            StartCoroutine(destination.SetDelayedState(ElevatorState.NotReady, _busyTime));
            StartCoroutine(SetDelayedState(ElevatorState.Ready, _busyTime));
        }
        else
        {
            Debug.Log("Elevator is busy");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (state == ElevatorState.Ready)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, destination.transform.position);
        }
    }

    [Serializable]
    public enum ElevatorState
    {
        NotReady,
        Ready,
        Busy
    }
}
