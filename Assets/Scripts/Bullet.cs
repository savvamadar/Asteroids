using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int player_index = -1;
    public Transform start;
    public Transform end;
    private Transform owner_root;
    public PoolItem pi;
    private Vector3 prev_frame_pos = Vector3.zero;

    public void Init(Transform owner)
    {
        SetEndPos();
        owner_root = owner;
    }

    public void SetEndPos()
    {
        prev_frame_pos = end.position;
    }

    public void OnEnable()
    {
        SetEndPos();
        CollisionCheck();
    }

    public void Update()
    {
        if (!CollisionCheck())
        {
            SetEndPos();
            MoveForward();
        }
        else
        {
            gameObject.SetActive(false);
            Pool.SetInactive(pi);
        }
    }

    public float bullet_vel = 1f;

    public void MoveForward()
    {
        transform.position += transform.up * Time.deltaTime * bullet_vel;
    }

    public bool CollisionCheck()
    {
        RaycastHit rh;
        if(Physics.Linecast(prev_frame_pos, start.position, out rh))
        {
            if(rh.transform.root != owner_root)
            {
                Target t = rh.transform.root.GetComponent<Target>();
                if (t != null)
                {
                    t.Hit(player_index);
                }
                return true;
            }
        }
        return false;
    }
}
