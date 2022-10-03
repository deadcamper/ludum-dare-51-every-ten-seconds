using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHurtable
{
    public float maxSpeed = 5f;

    public Rigidbody2D playerBody;

    public Collider2D playerCollider;

    public Animator animator;

    public AudioSource hurtSound;
    public AudioSource deathSound;

    public Bullet prefabBullet;

    private Vector2 playerFacing = Vector2.up;
    private bool isDead = false;
    private bool invulnerable = false;

    public static Player FetchPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("Could not find any player. Uh oh...");
            return null;
        }

        Player player = playerObj.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Found an object with tag 'Player' but with no Player component.");
        }
        return player;
    }

    public void Update()
    {
        if (!isDead)
        {
            UpdateMovement();
            UpdateFacing();
            UpdateFire();
        }
    }

    private void UpdateFire()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
        {
            Bullet bullet = Instantiate(prefabBullet, transform.position, Quaternion.identity);
            bullet.Fire(gameObject, playerFacing);
        }
    }

    private void UpdateMovement()
    {
        bool frozenPlayer = GameStateTracker.GetInstance().GetGameState().betweenLevels ||
            true == GameStateTracker.GetInstance().GetLevelState()?.freezePlayer;

        if (frozenPlayer)
        {
            playerBody.velocity = Vector2.zero;
            animator.SetBool("IsWalking", false);
        }
        else
        {
            float rawX = Input.GetAxis("Horizontal");
            float rawY = Input.GetAxis("Vertical");

            Vector2 rawXY = new Vector2(rawX, rawY);

            if (rawXY.sqrMagnitude < 0.1f)
            {
                playerBody.velocity = Vector2.zero;
                animator.SetBool("IsWalking", false);
            }
            else
            {
                if (rawXY.sqrMagnitude > 1)
                {
                    rawXY.Normalize();
                }
                playerBody.velocity = rawXY * maxSpeed;
                animator.SetBool("IsWalking", true);
            }

        }
    }

    private void UpdateFacing()
    {
        float rawX = Input.GetAxis("Horizontal");
        float rawY = Input.GetAxis("Vertical");


        float test = 0.05f;

        // Set facing direction based on movement;
        {
            float facingX, facingY;
            if (Mathf.Abs(rawX) < test)
            {
                facingX = 0;
            }
            else
            {
                facingX = Mathf.Sign(rawX);
            }

            if (Mathf.Abs(rawY) < test)
            {
                facingY = 0;
            }
            else
            {
                facingY = Mathf.Sign(rawY);
            }

            if (facingX != 0 || facingY != 0)
            {
                playerFacing = new Vector2(facingX, facingY);
            }
        }
    }

    public void Hurt(int damage)
    {
        GameState gs = GameStateTracker.GetInstance().GetGameState();

        if (gs.playerArmor > 0)
        {
            gs.playerArmor -= damage;
            if (gs.playerArmor < 0)
            {
                damage = -gs.playerArmor;
                gs.playerArmor = 0;
            }
        }

        gs.playerHearts -= damage;

        // TODO Check for death
        if (gs.playerHearts <= 0)
        {
            Die();
        }
        else
        {
            hurtSound?.Play();
            animator.SetBool("IsHurt", true);
            invulnerable = true;
        }
    }

    public void Die()
    {
        deathSound?.Play();
        animator.SetBool("IsDead", true);
        animator.SetBool("IsHurt", false);
        isDead = true;
        playerCollider.enabled = false;
        playerBody.velocity = Vector2.zero;
        Util.FetchMusicPlayer()?.Stop();
    }

    public void HurtFinish()
    {
        animator.SetBool("IsHurt", false);
        invulnerable = false;
    }

    public int GetHealth()
    {
        return GameStateTracker.GetInstance().GetGameState().playerHearts;
    }
}
