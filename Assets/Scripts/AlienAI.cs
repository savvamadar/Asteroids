using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAI : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public Transform visual;
    public float max_vel = 0f;
    public float visual_rotation_speed = 5f;

    public Vector2 time_between_changes = Vector2.one;
    private float time_before_change = 0f;

    public ShootBullet sb;
    public Transform bulletPrefab;

    void Start()
    {
        time_before_shot = Random.Range(shotTimer.x, shotTimer.y);
        transform.position = new Vector3(transform.position.x, transform.position.y, 5f);
        UpdateDirection();
    }

    public Vector2 shotTimer = Vector2.one;
    private float time_before_shot = 0f;

    private void Update()
    {
        RandomShot();
        CollisionTimer();
        UpdateDirection();
        visual.Rotate(0f, 0f, visual_rotation_speed * Time.deltaTime, Space.World);
    }



    private void RandomShot()
    {
        if (time_before_shot >= 0f)
        {
            time_before_shot -= Time.deltaTime;
            if (time_before_shot <= 0f)
            {
                time_before_shot = Random.Range(shotTimer.x, shotTimer.y);
                sb.Shoot("bullet_alien", bulletPrefab, bulletPrefab.position, visual.right);
            }
        }
    }



    private void CollisionTimer()
    {
        if (col_time >= 0f)
        {
            col_time -= Time.deltaTime;
            col_force = og_col_force * (col_time / max_col_time);
            if (col_time <= 0f)
            {
                col_force = Vector3.zero;
            }
        }
    }

    private void FixedUpdate()
    {
        if (col_time > 0f)
        {
            rb.velocity = col_force;
            //rb.AddForce(col_force, ForceMode.Impulse);
        }
        else
        {
            rb.velocity = target_del.normalized * max_vel;
        }
    }

    Vector3 target_del = Vector3.zero;
    public void UpdateDirection()
    {
        time_before_change -= Time.deltaTime;
        if (time_before_change <= 0f)
        {
            time_before_change = Random.Range(time_between_changes.x, time_between_changes.y);
            target_del = Vector3.up * Random.Range(-1f, 1f) + Vector3.right * Random.Range(-1f, 1f);
            rb.velocity = target_del.normalized * max_vel;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollisionResolveAlien(collision);
    }

    float col_time = 0.0f;
    float max_col_time = 1.5f;
    Vector3 og_col_force = Vector3.zero;
    Vector3 col_force = Vector3.zero;
    private void CollisionResolveAlien(Collision collision)
    {
        col_time = max_col_time;

        col_force = (transform.position - collision.transform.position);
        col_force.z = 0f;
        col_force = col_force.normalized * max_vel * 3f;

        og_col_force = col_force;
    }
}
