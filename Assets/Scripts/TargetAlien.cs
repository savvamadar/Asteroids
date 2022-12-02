using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAlien : Target
{
    public override void Hit(int player_index)
    {
        base.Hit(player_index);
        Destroy(gameObject);
    }
}
