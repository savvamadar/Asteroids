using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWarpBullet : ScreenWarp
{
    public PoolItem pi;
    public Bullet b;
    public override void Kill()
    {
        gameObject.SetActive(false);
        Pool.SetInactive(pi);
    }

    public override void Warp()
    {
        b.SetEndPos();
    }
}
