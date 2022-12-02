using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : Target
{
    public PlayerController pc;
    public float invincibility_timer = 2f;
    private float current_invincibility = 0f;
    public override void Hit(int player_index)
    {
        if (pc.ShieldUp())
        {
            return;
        }

        if ((Time.timeSinceLevelLoad - current_invincibility) < invincibility_timer)
        {
            return;
        }
        else
        {
            current_invincibility = Time.timeSinceLevelLoad;
            GameState.AddLives(pc.player_index, -1);
            PlayerState _ps = GameState.GetPlayerState(pc.player_index);
            if (_ps != null && _ps.lives <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
