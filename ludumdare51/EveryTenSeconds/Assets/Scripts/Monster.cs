using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour, IHurtable
{
    public Rigidbody2D monsterBody;
    public Animator animator;

    public Bullet bulletTemplate;

    public int health = 1;

    public float movementSpeed;

    public float farthestToLookForPlayer = 10f;

    public float brainDelay = 0.25f;

    public float distanceToAttack = 0.5f;

    public bool stillMovesOnAttack = false;


    // AI-like flags

    public bool nonStopPursuit = false;
    public bool hatesPlayer = false;
    public bool doesPatrols = false;

    private bool hasSeenPlayer = false;
    private bool seesPlayer = false;
    private bool busyAttacking = false;

    private Transform playerTransform;

    public UnityEvent onDeath;

    void OnEnable()
    {
        playerTransform = Util.FetchPlayer();
        if (playerTransform == null)
        {
            Debug.LogWarning("Could not find player. Disabling to avoid exception throws.");
            this.enabled = false;
            return;
        }

        StartCoroutine(DoMonsterChores());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {



    }

    IEnumerator DoMonsterChores()
    {
        while (true) // Disables externally.
        {
            yield return new WaitForEndOfFrame();
            if (busyAttacking && !stillMovesOnAttack)
            {
                continue;
            }

            if (hatesPlayer)
            {
                if (playerTransform.GetComponent<Player>().GetHealth() == 0)
                {
                    hatesPlayer = false;
                    continue;
                }

                if (seesPlayer || (nonStopPursuit && hasSeenPlayer))
                {
                    if (!busyAttacking && CheckIfCloseToPlayer(distanceToAttack))
                    {
                        Attack();
                        yield return new WaitForEndOfFrame(); // TODO wait for end of animation?
                    }

                    if (!busyAttacking || stillMovesOnAttack)
                    {
                        MoveTowardPlayer();
                        yield return new WaitForEndOfFrame();
                    }
                }
            }

            bool lastSeenPlayer = seesPlayer;
            seesPlayer = SearchForPlayer();

            if (lastSeenPlayer && !seesPlayer)
            {
                yield return new WaitForSeconds(brainDelay);
                StopMoving();
            }
        }

    }

    IEnumerator Die()
    {
        animator.SetBool("IsDead", true);

        monsterBody.velocity = Vector2.zero;

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();

        foreach(Collider2D col in colliders)
        {
            col.enabled = false;
        }

        // TODO Animation? Play sound effect?

        yield return new WaitForSeconds(0.1f);

        onDeath.Invoke();

        Destroy(gameObject);
    }

    bool SearchForPlayer()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(monsterBody.transform.position, playerTransform.position - monsterBody.transform.position, farthestToLookForPlayer);

        bool seePlayer = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform == monsterBody.transform)
                continue;

            if (hit.transform == transform)
                continue;

            if (hit.transform == playerTransform)
            {
                seePlayer = true;
                break;
            }

            if (hit.collider.isTrigger)
                continue;

            if (hit.collider.GetComponent<IHurtable>() != null)
            {
                continue;
            }



            break;
        }

        hasSeenPlayer |= seePlayer;
        return seePlayer;
    }

    bool CheckIfCloseToPlayer(float distNeeded)
    {
        float distNeededSquared = distNeeded * distNeeded;

        float distToCheckSquared = (playerTransform.position - monsterBody.transform.position).sqrMagnitude;

        return distToCheckSquared <= distNeededSquared;
    }

    public void Hurt(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }

    private void MoveTowardPlayer()
    {
        animator.SetBool("IsMoving", true);
        Vector3 direction = (playerTransform.position - monsterBody.transform.position).normalized;
        monsterBody.velocity = direction* movementSpeed;
    }

    private void StopMoving()
    {
        animator.SetBool("IsMoving", false);
        monsterBody.velocity = Vector2.zero;
    }

    private void Attack()
    {
        animator.SetBool("IsAttacking", true);
        animator.SetBool("IsMoving", false);

        if (!stillMovesOnAttack)
        {
            monsterBody.velocity = Vector2.zero;
        }

        busyAttacking = true;
        if (!animator.enabled)
        {
            AttackTriggerHurt();
            FinishAttack();
        }
    }

    private void AttackTriggerHurt()
    {
        if (bulletTemplate)
        {
            Bullet newBullet = Instantiate(bulletTemplate, monsterBody.transform.position, Quaternion.identity);
            newBullet.Fire(gameObject, playerTransform.position - monsterBody.transform.position);
        }
        
    }

    private void FinishAttack()
    {
        busyAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    public int GetHealth()
    {
        return health;
    }
}
