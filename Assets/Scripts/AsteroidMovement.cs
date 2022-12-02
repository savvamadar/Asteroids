using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public Transform visual;
    public float max_vel = 0f;
    Vector3 rnd_vel = Vector3.zero;
    private Vector3 rot_speed = Vector3.zero;
    public float visual_rotation_speed = 5f;
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 5f);
        rnd_vel = Vector3.up * Random.Range(-1f,1f) + Vector3.right * Random.Range(-1f, 1f);
        rb.velocity = rnd_vel.normalized * max_vel;
        rot_speed = (Vector3.up * Random.Range(-1f, 1f) + Vector3.right * Random.Range(-1f, 1f) + Vector3.up * Random.Range(-1f, 1f)) * visual_rotation_speed;
    }

    private void Update()
    {
        visual.Rotate(rot_speed * Time.deltaTime, Space.Self);
    }

    private void FixedUpdate()
    {
        rb.velocity = rnd_vel.normalized * max_vel;
    }
}
