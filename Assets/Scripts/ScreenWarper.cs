using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWarper : MonoBehaviour
{
    public static Vector3 screen_size = Vector3.zero;
    public static Vector2 min_bounds = Vector2.zero;
    public static Vector2 max_bounds = Vector2.zero;
    public static List<ScreenWarp> warpable_objects = new List<ScreenWarp>();
    public static Camera levelCam;
    public static void Add(ScreenWarp sw)
    {
        warpable_objects.Add(sw);
    }

    public static void Remove(ScreenWarp sw)
    {
        warpable_objects.Remove(sw);
    }

    public static ScreenWarper instance = null;
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            screen_size.x = Screen.width;
            screen_size.y = Screen.height;

            levelCam = FindObjectOfType<Camera>();

            min_bounds = levelCam.ScreenToWorldPoint(Vector3.zero);//bottom left
            max_bounds = levelCam.ScreenToWorldPoint(screen_size);//top right
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static Vector3 RandomOffScreenPosition(float _r)
    {
        Vector3 p = min_bounds;
        p.x -= _r;
        p.y -= _r;
        p.z = 5f;


        bool rnd_width = false;
        if(Random.Range(0f,1f) < 0.5f)//50 - 50 check if we are going to be hieght based or width
        {
            p.x = (Random.Range(0f, 1f) < 0.5f) ? min_bounds.x - _r : max_bounds.x + _r;
        }
        else
        {
            p.y = (Random.Range(0f, 1f) < 0.5f) ? min_bounds.y - _r : max_bounds.y + _r;
            rnd_width = true;
        }

        if (rnd_width)
        {
            p.x = Random.Range(min_bounds.x + _r, max_bounds.x - _r);
        }
        else
        {
            p.y = Random.Range(min_bounds.y + _r, max_bounds.y - _r);
        }

        return p;
    }

    public void Update()
    {
        for (int i = warpable_objects.Count - 1; i >= 0; i--)
        {
            if (warpable_objects[i] == null)
            {
                warpable_objects.RemoveAt(i);
            }
            else
            {
                Vector3 warpobj_pos = warpable_objects[i].transform.position;
                float _r = warpable_objects[i].radius;
                Vector2 warp_dir = Vector2.zero;
                Vector2 xy = Vector2.zero;
                if(warpobj_pos.x + _r < min_bounds.x)
                {
                    xy.x = 1;
                    warp_dir.x = max_bounds.x + _r;//to right side
                }
                else if(warpobj_pos.x - _r > max_bounds.x)
                {
                    xy.x = 1;
                    warp_dir.x = min_bounds.x - _r;//to left side
                }

                if (warpobj_pos.y + _r < min_bounds.y)
                {
                    xy.y = 1;
                    warp_dir.y = max_bounds.y + _r;
                }
                else if (warpobj_pos.y - _r > max_bounds.y)
                {
                    xy.y = 1;
                    warp_dir.y = min_bounds.y - _r;
                }

                if(warp_dir != Vector2.zero)
                {
                    if (warpable_objects[i].max_warp_count == 0)
                    {
                        warpable_objects[i].Kill();
                        warpable_objects.RemoveAt(i);
                    }
                    else
                    {
                        warpable_objects[i].Warp();
                        warpable_objects[i].transform.position = new Vector3(xy.x > 0 ? warp_dir.x : warpobj_pos.x, xy.y > 0 ? warp_dir.y : warpobj_pos.y, warpable_objects[i].transform.position.z);
                        warpable_objects[i].max_warp_count--;
                    }
                }
            }
        }
    }
}
