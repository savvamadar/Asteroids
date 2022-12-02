using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerTarget[] p_targets;
    void Start()
    {
        bool new_targets = false;
        if(p_targets == null)
        {
            new_targets = true;
        }
        else
        {
            for(int i=0; i < p_targets.Length; i++)
            {
                if(p_targets[i] == null)
                {
                    new_targets = true;
                }
            }
        }

        if (new_targets)
        {
            p_targets = GameObject.FindObjectsOfType<PlayerTarget>();
        }

        target_dir = transform.up;
    }

    Vector3 target_dir = Vector3.zero;

    public Rigidbody rb;
    public float max_rotation_per_second = 0f;
    public float forward_speed = 1f;
    // Update is called once per frame
    void Update()
    {
        if (p_targets.Length > 0)
        {
            PlayerTarget closest_target = p_targets[0];
            if(p_targets.Length > 1)
            {
                for(int i=1; i < p_targets.Length; i++)
                {
                    if(Vector3.Distance(p_targets[i].transform.position, transform.position) < Vector3.Distance(closest_target.transform.position, transform.position))
                    {
                        closest_target = p_targets[i];
                    }
                }
            }

            target_dir = (closest_target.transform.position - transform.position).normalized;

        }

        float angle_diff = Vector3.SignedAngle(transform.up, target_dir, Vector3.forward);
        
        transform.Rotate(0f, 0f, Mathf.Clamp(angle_diff, -max_rotation_per_second, max_rotation_per_second) * Time.deltaTime, Space.Self);

        //Vector3 _d = transform.up;
        //_d.z = 0f;
        rb.velocity = transform.up * forward_speed;

    }
}
