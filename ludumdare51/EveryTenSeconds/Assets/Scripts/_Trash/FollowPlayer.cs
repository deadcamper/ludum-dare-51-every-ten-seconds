using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Rigidbody2D body;

    public Transform playerTransform;

    public float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = Util.FetchPlayer();
        if (playerTransform == null)
        {
            Debug.LogWarning("Could not find player. Disabling to avoid exception throws.");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Follow();
    }

    // Push really hard to follow the player as much as possible
    void Follow()
    {
        Vector3 currentDisplacement = playerTransform.position - transform.position;

        if (currentDisplacement.sqrMagnitude < 0.1f)
        {
            body.velocity = Vector2.zero;
            transform.position = playerTransform.position;
        }
        else
        {
            if (maxSpeed > 0 && currentDisplacement.sqrMagnitude > maxSpeed*maxSpeed)
            {
                currentDisplacement = currentDisplacement.normalized * maxSpeed;
            }

            body.velocity = currentDisplacement;
        }
    }
}
