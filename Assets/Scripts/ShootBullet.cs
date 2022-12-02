using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public void Shoot(string key, Transform prefab, Vector3 start_point, Vector3 right)
    {
        PoolItem _b = Pool.RequestItem(key, prefab);
        _b.transform.position = start_point;
        _b.transform.right = right;
        _b.gameObject.GetComponent<Bullet>().Init(transform.root);
        _b.GetComponent<ScreenWarpBullet>().max_warp_count = prefab.GetComponent<ScreenWarpBullet>().max_warp_count;
        _b.transform.localScale = prefab.lossyScale;
        _b.gameObject.SetActive(true);
        Pool.SetActive(_b);
    }
}
