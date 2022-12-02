using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStarMovement : MonoBehaviour
{
    public float rotation_speed;

    private Vector3 target_dir = Vector3.one;
    public float max_vel = 1f;
    public Rigidbody rb;

    public void Start()
    {
        target_dir = Vector3.up * Random.Range(-1f, 1f) + Vector3.right * Random.Range(-1f, 1f);
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, rotation_speed * Time.deltaTime, Space.World);
    }

    private void FixedUpdate()
    {
        rb.velocity = target_dir.normalized * max_vel;
    }

}
