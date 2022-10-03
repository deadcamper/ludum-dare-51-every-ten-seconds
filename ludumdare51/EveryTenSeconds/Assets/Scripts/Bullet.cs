using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int damage;
    public float speed = 5f;
    public float lifeSpan = 10f; // Time before cleaning

    public Collider2D trigger;
    public Animator animator;

    private GameObject owner;

    private Vector2 velocity;
    private float timeToDie;

    public bool playerOnly;

    public void Fire(GameObject owner, Vector2 direction)
    {
        this.owner = owner;
        velocity = direction.normalized * speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        timeToDie = Time.time + lifeSpan;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeToDie)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector3 delta = velocity * Time.fixedDeltaTime;
        transform.position += delta;
    }

    // TODO collisions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == owner)
        {
            return;
        }

        if (owner && collision.transform.IsChildOf(owner.transform))
        {
            return;
        }

        if (playerOnly)
        {
            Player playa = collision.GetComponent<Player>();
            if (!playa)
            {
                return;
            }
        }

        List<IHurtable> allTheHurt = new List<IHurtable>();
        IHurtable[] hurtMePlenty = collision.GetComponentsInParent<IHurtable>();

        allTheHurt.AddRange(hurtMePlenty);

        if (allTheHurt.Count > 0)
        {
            foreach (IHurtable hurtMe in allTheHurt)
            {
                hurtMe.Hurt(damage);
            }
        }

        Destroy(gameObject);
    }
}
