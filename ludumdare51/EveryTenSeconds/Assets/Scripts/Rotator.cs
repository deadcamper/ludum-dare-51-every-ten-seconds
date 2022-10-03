using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed;
    public Vector3 axis = Vector3.forward;

    // Update is called once per frame
    void Update()
    {
        float angle = speed * Time.deltaTime;
        transform.Rotate(axis, angle);
    }
}
