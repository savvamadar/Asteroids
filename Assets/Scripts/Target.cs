using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int score = 0;
    public virtual void Hit(int player_index)
    {
        GameState.AddScore(player_index, score);
        if(score > 0)
        {
            GameState.KillEnemy();
        }
    }
}
