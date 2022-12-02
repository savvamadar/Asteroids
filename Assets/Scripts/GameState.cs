using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public int lives = 3;
    public int score = 0;
}

public class GameState : MonoBehaviour
{
    public Transform asteroids;
    public Transform alien;
    public Transform death_star;

    public static Dictionary<int, PlayerState> player_states = new Dictionary<int, PlayerState>();

    public Transform[] Player_Ships;
    public Transform[] Player_Scores;

    public GameObject PreStartUI;
    public GameObject LoseUI;

    private int state = 0;
    private int level = 0;
    private static int level_enemies = 0;

    public void StartGame(int players)
    {
        BackgroundMusic.StartMusic();

        PreStartUI.SetActive(false);

        player_states.Clear();

        bool even = (players % 2 == 0);
        float dist_between_ships = 10f;
        for (int i = 0; i < players; i++)
        {
            player_states[i] = new PlayerState();

            Player_Ships[i].position = ((((-players / 2f) + (i + (even ? 0.5f : 0f))) * dist_between_ships) + (even ? 0f : dist_between_ships / 2f)) * Vector3.right;
            Player_Ships[i].position += Vector3.forward * 5f;

            Player_Ships[i].gameObject.SetActive(true);
            Player_Scores[i].gameObject.SetActive(true);
        }

        level_enemies = 0;
        state = 1;
        level = 0;
    }

    public static void AddScore(int pindex, int score)
    {
        if (player_states.ContainsKey(pindex))
        {
            player_states[pindex].score += score;
        }
    }

    public static PlayerState GetPlayerState(int pindex)
    {
        if (player_states.ContainsKey(pindex))
        {
            return player_states[pindex];
        }
        return null;
    }

    public static void AddLives(int pindex, int lives)
    {
        if (player_states.ContainsKey(pindex))
        {
            player_states[pindex].lives += lives;
        }
    }


    public float time_between_wave = 1.5f;
    private float _cd = 0f;
    private void Update()
    {
        if(state == 3)
        {
            LoseUI.SetActive(((int)_cd % 2 != 0));
            _cd -= Time.deltaTime;
            if (_cd <= 1f)
            {
                for (int i = 0; i < player_states.Count; i++)
                {
                    if (EasyInput.Player(i).GetInputDown("fire"))
                    {
                        Application.LoadLevel(Application.loadedLevel);
                    }
                }
            }
        }
        if(state > 0 && state < 3)
        {
            int total_player_lives = 0;
            for(int i=0; i < player_states.Count; i++)
            {
                total_player_lives += player_states[i].lives;
            }
            if(total_player_lives <= 0)
            {
                _cd = 2f;
                state = 3;
                return;
            }
            //Debug.Log("Enemies: " + level_enemies);
        }
        if(state == 1)
        {
            level++;
            level_enemies = 0;
            SpawnLevelEnemies();
            _cd = time_between_wave;
            state = 2;
        }
        else if(state == 2 && level_enemies <= 0)
        {
            if(_cd >= 0)
            {
                _cd -= Time.deltaTime;
                if(_cd <= 0f)
                {
                    state = 1;
                }
            }
        }
    }

    public static void KillEnemy()
    {
        level_enemies -= 1;
    }

    private void SpawnLevelEnemies()
    {

        int num_asteroids = (int)(Mathf.Lerp(3f, 10f, level / 10f));
        int num_aliens = (int)(level >= 3 ? (Mathf.Lerp(1f, 3f, (level-3) / 10f)) : 0f);
        int num_death_stars = (int)(level >= 5 ? (Mathf.Lerp(1f, 3f, (level - 5) / 10f)) : 0f);

        for (int i=0; i < num_asteroids; i++)
        {
            Instantiate(asteroids, ScreenWarper.RandomOffScreenPosition(asteroids.GetComponent<ScreenWarp>().radius * 0.9f), asteroids.rotation);
            level_enemies += 7;//1 + 2 + 4;
        }

        for (int i = 0; i < num_aliens; i++)
        {
            Instantiate(alien, ScreenWarper.RandomOffScreenPosition(alien.GetComponent<ScreenWarp>().radius * 0.9f), alien.rotation);
            level_enemies += 1;
        }

        for (int i = 0; i < num_death_stars; i++)
        {
            Instantiate(death_star, ScreenWarper.RandomOffScreenPosition(death_star.GetComponent<ScreenWarp>().radius * 0.9f), death_star.rotation);
            level_enemies += 10;//1 + 3 + 6;
        }
    }


}
