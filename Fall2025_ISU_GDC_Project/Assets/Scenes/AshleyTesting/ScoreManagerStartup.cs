using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

//by Ashley Simone

public class Startup : MonoBehaviour
{
    //variables 
    [SerializeField] public int numberOfPlayers;
    public TMP_Text ScoreLabel;
    List<int> score_list = new List<int>();
    int kill_reward = 1; //number of points granted to a player on kill
    int death_penalty = 1; //number of points lost when a player dies 



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreLabel = ScoreLabel.GetComponent<TMP_Text>();

        for (int count = 0; count < numberOfPlayers; count++)
        {
            score_list.Add(0);
        }

        RenderScores();

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Sets ScoreLabel to display players and their scores 

    public void RenderScores()
    {
        ScoreLabel.text = "Score: \n";
        for (int count = 0; count < numberOfPlayers; count++)
        {
            ScoreLabel.text += "Player " + (count + 1) + ": " + score_list[count] + "\n";
        }
    }

    // Adds point to the killer and removes point from the killed charecter 

    public void ScoreKill(int killer, int killee) //killer value of -1 indicates no killer 
    {
        if (killer != -1) { score_list[killer] += kill_reward; }
        score_list[killee] += death_penalty;
        RenderScores();
    }
}
