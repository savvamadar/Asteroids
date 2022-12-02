using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWarp : MonoBehaviour
{
    public int max_warp_count = -1;
    public float radius = 0f;
    private void OnEnable()
    {
        ScreenWarper.Add(this);
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    public virtual void Warp()
    {
        return;
        //was warped
    }
}
