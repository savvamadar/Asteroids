using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PoolEntry
{
    public HashSet<PoolItem> active = new HashSet<PoolItem>();
    public HashSet<PoolItem> inactive = new HashSet<PoolItem>();
}

public class Pool : MonoBehaviour
{
    public static Dictionary<string, PoolEntry> pool_dict = new Dictionary<string, PoolEntry>();

    public static Pool instance = null;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static PoolItem RequestItem(string key, Transform prefab)
    {
        if (!pool_dict.ContainsKey(key))
        {
            pool_dict[key] = new PoolEntry();
        }
        if(pool_dict[key].inactive.Count <= 0)
        {
            Transform t = Instantiate(prefab, prefab.position, prefab.rotation);
            t.gameObject.SetActive(false);
            pool_dict[key].active.Remove(t.GetComponent<PoolItem>());
            pool_dict[key].inactive.Add(t.GetComponent<PoolItem>());
        }

        foreach(var x in pool_dict[key].inactive)
        {
            return x;
        }

        return null;
    }

    public static void SetActive(PoolItem pi)
    {
        pool_dict[pi.key].inactive.Remove(pi);
        pool_dict[pi.key].active.Add(pi);
    }

    public static void SetInactive(PoolItem pi)
    {
        pool_dict[pi.key].active.Remove(pi);
        pool_dict[pi.key].inactive.Add(pi);
    }
}
