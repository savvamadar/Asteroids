using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitTarget : Target
{
    public Transform[] split_pieces;

    public override void Hit(int player_index)
    {
        //Debug.Log("test0.5");
        base.Hit(player_index);
        //Debug.Log("test2");
        for (int i = 0; i < split_pieces.Length; i++)
        {
            Instantiate(split_pieces[i], transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
