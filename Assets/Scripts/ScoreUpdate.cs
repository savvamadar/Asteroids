using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI score;
    public TextMeshProUGUI lives;
    public int pindex = -1;

    void Start()
    {
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        PlayerState ps = GameState.GetPlayerState(pindex);
        score.text = string.Format("P{0} Score: {1}", pindex+1, ps == null ? 0 : ps.score);
        lives.text = string.Format("Lives: {0}", ps == null ? 0 : ps.lives);
    }
}
