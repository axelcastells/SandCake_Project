﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour {

    #region variables
    private Vector2 contactNormal;
    private Rigidbody2D rBody;
    private Quaternion rotation;
    private int jumpCount, currentJumpCount;

    public float speed;
    public float maxSpeed;
    public float minSpeed;
    public float rotationLerpTime;
    public Vector2 jump;
    #endregion

    // Use this for initialization
    void Start () {
        jumpCount = 1;
        currentJumpCount = jumpCount;
        contactNormal = Vector2.zero;
        rBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (GameCore.Instance.gameState)
        {
            case GameState.AWAKE:
                {

                }
                break;
            case GameState.PAUSE:
                {

                }
                break;
            case GameState.PLAY:
                {
                    ClampSpeed();
                    Movement();
                    RotatePlayer();
                }
                break;
            default:
                break;
        }

    }

    #region events

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("GROUND!");
        currentJumpCount = jumpCount; 
    }

    void OnCollisionStay2D(Collision2D other)
    {
        foreach(ContactPoint2D contact in other.contacts)
        {
            contactNormal += contact.normal;
        }
        contactNormal = contactNormal.normalized;
    }

    #endregion

    #region functions

    void ClampSpeed()
    {
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
    }

    void RotatePlayer()
    {
        rotation = new Quaternion(0, 0, -Mathf.Sin(contactNormal.x) * 180 * Time.deltaTime * rotationLerpTime, 0);
        //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rotation, rotationLerpTime * Time.deltaTime);
    }

    void Movement()
    {
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        rBody.velocity = new Vector2(contactNormal.y * speed, rBody.velocity.y);
    }

    public void Jump()
    {
        if (currentJumpCount > 0)
        {
            currentJumpCount -= 1;
            rBody.AddForce(jump, ForceMode2D.Impulse);
            rBody.rotation = 0;
        }
    }

    #endregion
}