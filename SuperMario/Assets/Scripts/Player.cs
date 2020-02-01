﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1;
    public float shift = 2;
    public float jumpSpeed = 2;
    public float velocity = 0;
    public float gravity = 1;
   

    private SpriteRenderer marioSprite;

    public Sprite[] smallWalk;
    public Sprite[] smallIdle;
    public Sprite[] bigWalk;
    public Sprite[] bigIdle;
    private Sprite[] currentAnim;
    private float directionX = 0;
    private bool isBig = true;
    private int animState = 0;
    private float animTime = 0;
    private int currentSprite;
    private bool onGround = false;
    private bool maxJump = false;
    private Rigidbody rigidBody;
    private RaycastHit raycastDown;
    
    // Start is called before the first frame update
    void Start()
    {
        marioSprite = this.GetComponentInChildren<SpriteRenderer>();
        /*bigWalk = smallWalk = new Sprite[3];
        bigIdle = smallIdle = new Sprite[1];*/
        SetAnim(0, true);
        currentAnim = bigIdle;
        currentSprite = 0;
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        directionX = Input.GetAxis("Horizontal");

        if (directionX < 0)
        {
            marioSprite.flipX = true;
            SetAnim(1, true);
        }
        else if(directionX > 0)
        {
            marioSprite.flipX = false;
            SetAnim(1, true);
        }
        else
        {
            SetAnim(0, true);
        }
        velocity = Mathf.Lerp(velocity, directionX, shift * Time.deltaTime);

        if(velocity < 0.000003f && velocity > -0.000003f)
        {
            velocity = 0;
        }

        Vector2 pos = this.transform.position;
        pos.x += (speed * velocity * Time.deltaTime);
        transform.position = pos;

        animTime += Time.deltaTime * (Mathf.Abs(velocity))*(speed*2);
        //Debug.Log((int)animTime);
        if((int)animTime >= 1)
        {
            //Debug.Log("SOY MAYOR");
            animTime = 0;
            currentSprite++;
        }

        if(currentSprite >= currentAnim.Length)
        {
            currentSprite = 0;
        }
        marioSprite.sprite = currentAnim[currentSprite];

        // Debug.Log("Anim TIme: " + animTime);


      
    }

    private void FixedUpdate()
    {

    }

   

    private void SetAnim(int state, bool big)
    {
        if (animState != state)
        {
            switch (state)
            {
                case 0:
                    if (big)
                        currentAnim = bigIdle;
                    else
                        currentAnim = smallIdle;
                    break;
                case 1:
                    if (big)
                        currentAnim = bigWalk;
                    else
                        currentAnim = smallWalk;
                    break;
            }
            animState = state;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<MoveBlock>())
        {
            collision.collider.GetComponent<MoveBlock>().Activate();
        }
    }
}