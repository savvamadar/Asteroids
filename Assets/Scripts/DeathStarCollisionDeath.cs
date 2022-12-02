using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStarCollisionDeath : MonoBehaviour
{
    public Target hit;
    private void OnCollisionEnter(Collision collision)
    {
        CollisionResolve(collision);
    }

    private void CollisionResolve(Collision collision)
    {
        hit.Hit(-1);
    }
}
