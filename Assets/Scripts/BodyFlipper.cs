using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Cache;
using UnityEngine;


public class BodyFlipper : MonoBehaviour
{
    public bool isPlayer = false;
    
    public bool facingLeft = true;

    [Header("Transforms")] 
    public Transform head;
    public Transform body;
    public Transform leftLeg;
    public Transform rightLeg;
    public Transform leftArm;
    public Transform rightArm;

    [Header("Left Side")]
    public Vector3 localHeadLeft;
    public Vector3 localBodyLeft;
    public Vector3 localLeftLegLeft;
    public Vector3 localRightLegLeft;
    public Vector3 localLeftArmLeft;
    public Vector3 localRightArmLeft;
    
    
    [Header("Right Side")]
    public Vector3 localHeadRight;
    public Vector3 localBodyRight;
    public Vector3 localLeftLegRight;
    public Vector3 localRightLegRight;
    public Vector3 localLeftArmRight;
    public Vector3 localRightArmRight;

    private float lastPosX;
    private SpriteRenderer _head;
    private SpriteRenderer _body;
    private SpriteRenderer _leftLeg;
    private SpriteRenderer _rightLeg;
    private SpriteRenderer _leftArm;
    private SpriteRenderer _rightArm;

    private void Start()
    {
        _head = head.GetComponent<SpriteRenderer>();
        _body = body.GetComponent<SpriteRenderer>();
        _leftLeg = leftLeg.GetComponent<SpriteRenderer>();
        _rightLeg = rightLeg.GetComponent<SpriteRenderer>();

        localHeadLeft = head.localPosition;
        localBodyLeft = body.localPosition;
        localLeftLegLeft = leftLeg.localPosition;
        localRightLegLeft = rightLeg.localPosition;
        
        if (isPlayer)
        {
            localLeftArmLeft = leftArm.localPosition;
            localRightArmLeft = rightArm.localPosition;
            _leftArm = leftArm.GetComponent<SpriteRenderer>();
            _rightArm = rightArm.GetComponent<SpriteRenderer>();
        }
        
        lastPosX = transform.position.x;
    }

    private void LateUpdate()
    {
        if (Math.Abs(lastPosX - transform.position.x) < 0.001f)
        {
            return;
        }

        if (lastPosX < transform.position.x)
        {
            //Debug.Log("moving right");
            if (facingLeft)
            {
                head.localPosition = localHeadRight;
                body.localPosition = localBodyRight;
                leftLeg.localPosition = localLeftLegRight; 
                rightLeg.localPosition = localRightLegRight;

                _head.flipX = true;
                _body.flipX = true;
                _leftLeg.flipX = true;
                _rightLeg.flipX = true;

                if (isPlayer)
                {
                    leftArm.localPosition = localLeftArmRight;
                    rightArm.localPosition = localRightArmRight;
                    _leftArm.flipX = true;
                    _rightArm.flipX = true;
                }

                facingLeft = false;
            }
            
        }
        else
        {
            //Debug.Log("moving left");
            if (!facingLeft)
            {
                head.localPosition = localHeadLeft;
                body.localPosition = localBodyLeft;
                leftLeg.localPosition = localLeftLegLeft;
                rightLeg.localPosition = localRightLegLeft;
            
                _head.flipX = false;
                _body.flipX = false;
                _leftLeg.flipX = false;
                _rightLeg.flipX = false;

                if (isPlayer)
                {
                    leftArm.localPosition = localLeftArmLeft;
                    rightArm.localPosition = localRightArmLeft;
                    _leftArm.flipX = false;
                    _rightArm.flipX = false;
                }
                
                facingLeft = true;
            }
        }

        lastPosX = transform.position.x;
    }
}
