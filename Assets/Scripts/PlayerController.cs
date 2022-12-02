using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int player_index = 0;

    public Rigidbody rb;
    public Transform rotation_helper;
    public float max_velocity = 5f;
    public float thrust_force = 1f;
    public float rotation_speed = 1f;

    private float thrust_amt = 0f;
    private float rotation_goal_delta = 0f;
    private Quaternion visual_rotation_target = Quaternion.identity;
    private bool shield_up = false;

    public bool ShieldUp() {
        return shield_up;
    }


    public Transform ship_visual;
    public MeshRenderer shield_visual;
    public Transform trails_visual;
    public float shield_time = 12f;
    public float shield_rotation_speed = 1f;

    public Transform bullet_spawn;
    public Transform player_bullet;
    string bullet_key = "";

    void Awake()
    {
        //adjust player color
        Color pColor = Color.white;
        if (player_index == 0)
        {
            pColor = Color.red;
        }
        else
        {
            pColor = Color.blue;
        }
        MeshRenderer[] mrs = gameObject.GetComponentsInChildren<MeshRenderer>(true);
        foreach(var mr in mrs)
        {
            string t_name = mr.transform.name.ToLower();
            if (t_name.Contains("shield"))
            {
                mr.material.SetColor("_EmissionColor", pColor * 3f);
            }
            else if (t_name.Contains("trail"))
            {
                mr.material.SetColor("_EmissionColor", pColor * 4f);
            }
            else if (t_name.Contains("bullet"))
            {
                mr.material.SetColor("_EmissionColor", pColor * 5f);
            }
            else
            {
                mr.material.SetColor("_EmissionColor", pColor *  0.7f);
            }
            
        }

        player_bullet.GetComponent<Bullet>().player_index = player_index;
        player_bullet.GetComponent<PoolItem>().key += "_p" + player_index;
        bullet_key = player_bullet.GetComponent<PoolItem>().key;

        rotation_helper.parent = null;
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        thrust_amt = (EasyInput.Player(player_index).GetInput("fwd") ? Mathf.InverseLerp(1f, 0f, col_time / max_col_time) : 0f);
        //thrust visual
        Vector3 thrust_length = Vector3.one;
        thrust_length.y = thrust_amt > 0f ? (rb.velocity.magnitude/30f) * Mathf.InverseLerp(1f, 0f, col_time / max_col_time) : 0f;
        trails_visual.localScale = Vector3.Lerp(trails_visual.localScale, thrust_length, 30f * Time.deltaTime);


        //real rotation logic
        rotation_goal_delta += -((EasyInput.Player(player_index).GetInput("rht") ? 1f : 0f) - (EasyInput.Player(player_index).GetInput("lft") ? 1f : 0f));
        rotation_goal_delta *= rotation_speed * Time.deltaTime;
        rotation_helper.Rotate(0f, 0f, rotation_goal_delta, Space.World);

        //visual rotation logic
        Quaternion prev_rot = ship_visual.localRotation;
        ship_visual.localRotation = Quaternion.identity;
        ship_visual.Rotate(0f, 0f, ((rotation_goal_delta/rotation_speed) * Mathf.Min(Mathf.Max(5f, rb.velocity.magnitude*1.5f), 50f)) / Time.deltaTime, Space.Self);
        visual_rotation_target = ship_visual.localRotation;
        ship_visual.localRotation = Quaternion.Lerp(prev_rot, ship_visual.localRotation, 15f * Time.deltaTime);
        rotation_goal_delta = 0f;

        //shield logic
        shield_up = shield_time > 0f && (EasyInput.Player(player_index).GetInput("shld"));
        shield_time -= shield_up ? Time.deltaTime : 0;
        shield_visual.enabled = shield_up;
        shield_visual.transform.Rotate(0f, shield_rotation_speed * Time.deltaTime, 0f, Space.Self);

        //shoot logic
        if (EasyInput.Player(player_index).GetInputDown("fire")){
            sb.Shoot(bullet_key, player_bullet, bullet_spawn.position, transform.right);
        }

        //remove collision force over time
        col_time -= Time.deltaTime;
        col_force = Vector3.Lerp(Vector3.zero, og_col_force, col_time/ max_col_time);


    }

    public ShootBullet sb;


    private void FixedUpdate()
    {
        rb.AddForce(rb.transform.forward * thrust_amt * thrust_force, ForceMode.VelocityChange);
        rb.MoveRotation(rotation_helper.rotation);
        if(rb.velocity.magnitude > max_velocity)
        {
            rb.velocity = Vector3.Lerp(rb.velocity.normalized * max_velocity, col_force, (col_time / max_col_time));
        }
        rb.AddForce(col_force, ForceMode.Impulse);
        
    }

    float col_time = 0.0f;
    float max_col_time = 1.5f;
    Vector3 og_col_force = Vector3.zero;
    Vector3 col_force = Vector3.zero;
    private void OnCollisionEnter(Collision collision)
    {
        CollisionResolvePlayer(collision);
    }

    public PlayerTarget p_LifeSystem;
    private void CollisionResolvePlayer(Collision collision)
    {
        col_time = max_col_time;

        col_force = (transform.position - collision.transform.position);
        col_force.z = 0f;
        col_force = col_force.normalized * max_velocity * 0.5f;

        og_col_force = col_force;

        p_LifeSystem.Hit(player_index);
    }
}
