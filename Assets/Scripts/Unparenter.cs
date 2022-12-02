using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unparenter : MonoBehaviour
{
    private void Awake()
    {
        for(int i=transform.childCount - 1; i >= 0; i--)
        {
            Transform t = transform.GetChild(i);
            t.parent = null;
            t.gameObject.SetActive(true);
        }
        Destroy(gameObject);
    }
}
