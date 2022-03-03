using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float GravityWeak = 2f;
    public float GravityStrong = 3.5f;
    public float JumpForce = 2f;           // Max jump force (for adaptable jumps)
    public float MaxSpeed = 500f;           // Horizontal speed cap
    public float MaxFallSpeed = 900f;       // Vertical (falling) speed cap
    public float MoveSmoothing = 0.1f;
    public float CoyoteTime = 0.1f;         // Time to jump while in the air
    public Transform GroundTester;          // Child Transform where to test for ground
    public LayerMask GroundLayer;           // Ground layer used to test for ground

    private float Direction;                // Direction of the horizontal movement
    private float AirTimer = 0f;            // Used for Coyote time
    private float Gravity = 98f; 
    private Vector3 Velocity = Vector3.zero;

    // States of the "jump" Button
    private bool Jump = false;              // Pressed
    private bool JumpRelease = false;       // Released
    private bool Attack = false;
    private bool OnGround;

    //Specific states of the game
    private bool DoMove = false;
    private bool HasSword = false;
    private int MaxJump = 1;
    private int JumpNb = 0;

    private GameObject RespawnPos;

    private Collider2D PlayerCollider;
    private Rigidbody2D PlayerBody;
    private Animator PlayerAnimator;
    private SpriteRenderer PlayerSprite;

    public SpriteRenderer SwordSprite;
    public SpriteRenderer CloudSprite;



    // Start is called before the first frame update
    void Start() {
        PlayerCollider = gameObject.GetComponent<Collider2D>();
        PlayerBody = gameObject.GetComponent<Rigidbody2D>();
        PlayerAnimator = gameObject.GetComponent<Animator>();
        PlayerSprite = gameObject.GetComponent<SpriteRenderer>();

        RespawnPos = GameObject.FindWithTag("Respawn");

        PlayerBody.gravityScale = GravityStrong;
    }
    // Input here
    private void Update() {
        if (!GameManager.Instance.in3D)
        {
            if (Input.GetButtonDown("Cancel")) GameManager.Instance.ChangePlayer(false);
            
            Direction = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump")) Jump = true;        // Store the state of the key to be used
            if (Input.GetButtonUp("Jump")) JumpRelease = true;   // during FixedUpdate
            if (Input.GetButtonDown("Interract")) Attack = true;
        }
    }
    private void FixedUpdate()
    {
        if(DoMove)
        {
            // Apply "move" back to player velocity and reset "Jump" state
            PlayerBody.velocity = MoveWithVector(PlayerBody.velocity);
            SetAnimations();
            Jump = false;
            JumpRelease = false;
            Attack = false;
        }
        else
        {
            PlayerBody.velocity = Vector2.zero;
        }
    }
    // Movement here
    private Vector2 MoveWithVector(Vector2 move)
    {
        // Detects if the player is grounded
        OnGround = false;
        Collider2D collider = Physics2D.OverlapCircle(GroundTester.position, 0.2f, GroundLayer);
        OnGround = (collider != null);

        /*
        Take the current velocity of the player and store it in "move".
        Make calculations on "move", first the movement on the Y axis, then on the X axis.
        Finally, apply "move" to the player velocity.
        */

        // Vertical movement
        if (OnGround)
        {
            AirTimer = 0f;
            JumpNb = 0;
        }
        else
        {
            AirTimer += Time.fixedDeltaTime; // Increase the timer when off the ground

            // Used to cap the falling speed when off the ground
            if (move.y < (-MaxFallSpeed * Time.fixedDeltaTime))
            {
                move.y = (-MaxFallSpeed * Time.fixedDeltaTime);
            }
        }
        if(JumpRelease || move.y < 0f)
        {
            PlayerBody.gravityScale = GravityStrong;
        }

        if (Jump && (AirTimer < CoyoteTime || JumpNb < MaxJump))
        {
            AirTimer = CoyoteTime;
            move.y = JumpForce;
            JumpNb += 1;

            PlayerBody.gravityScale = GravityWeak;
            SoundManager.Instance.PlaySound(SOUND_ID.Jump);
        }

        if (Attack && HasSword)
        {
            PlayerAnimator.SetTrigger("Attack");
            SoundManager.Instance.PlaySound(SOUND_ID.SwordSwing);

            Vector2 actPos = new Vector2(transform.position.x + (PlayerSprite.flipX ? -1f : 1f), transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(actPos, new Vector2((PlayerSprite.flipX ? actPos.x - 0.5f : -actPos.x + 0.5f), actPos.y), 0.9f);

            if (hit.collider != null) {
                if (hit.collider.tag == "Ennemy") Destroy(hit.transform.gameObject);
                SoundManager.Instance.PlaySound(SOUND_ID.PlayerHitsEnemy);
            }
        }

        // Horizontal movement
        Vector3 targetVelocity = new Vector2(Direction * MaxSpeed * Time.fixedDeltaTime, move.y);
        return Vector3.SmoothDamp(move, targetVelocity, ref Velocity, MoveSmoothing);
    }
    private void SetAnimations()
    {
        
        PlayerAnimator.SetBool("Running", (Mathf.Abs(Direction) > Mathf.Epsilon));
        PlayerAnimator.SetBool("Falling", (PlayerBody.velocity.y < Mathf.Epsilon && !OnGround));
        PlayerAnimator.SetBool("Jumping", (PlayerBody.velocity.y >= Mathf.Epsilon && !OnGround));
        if (Direction < 0f) 
        { 
            PlayerSprite.flipX = true;
            SwordSprite.flipX = true;
        }
        else if (Direction > 0f) PlayerSprite.flipX = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Pickable2D":
                Item collected_item = collision.GetComponent<Pickable2DItem>().item;
                GameManager.Instance.AddItem(collected_item);

                Destroy(collision.gameObject);
                break;
            case "Ennemy":
                SoundManager.Instance.PlaySound(SOUND_ID.EnemyHitsPlayer);
                Respawn();
                break;
            case "Respawn":
                RespawnPos.GetComponent<Animator>().SetBool("Up", false);
                RespawnPos = collision.gameObject;
                RespawnPos.GetComponent<Animator>().SetBool("Up", true);
                break;
            case "Narrator":
                collision.gameObject.GetComponent<NarratorTrigger>().PlaySound();
                collision.GetComponent<Subtitles>().ShowSubtitles();
                AnimationTrigger anim = collision.gameObject.GetComponent<AnimationTrigger>();
                if (anim != null) anim.TriggerAnim();
                collision.GetComponent<CircleCollider2D>().enabled = false;
                break;
        }
    }

    public void UnlockMove()
    {
        DoMove = true;
    }
    public void LockMove()
    {
        DoMove = false;
    }

    public void GetSword()
    {
        HasSword = true;
        SwordSprite.enabled = true;
    }
    public void GetCloud()
    {
        MaxJump = 2;
    }
    public void Respawn()
    {
        this.transform.position = (RespawnPos.transform.position + new Vector3(0, 1, 0));
    }
}
