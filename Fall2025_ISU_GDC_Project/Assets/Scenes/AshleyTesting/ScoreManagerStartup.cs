using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class Startup : MonoBehaviour
{
    //variables 
    [SerializeField] public int numberOfPlayers;
    public TMP_Text ScoreLabel;

    List<int> score_list = new List<int>();



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

    void RenderScores()
    {
        ScoreLabel.text = "Score: \n";
        for (int count = 0; count < numberOfPlayers; count++)
        {
            ScoreLabel.text += "Player " + (count + 1) + ": " + score_list[count] + "\n";
        }
    }

    void ScoreKill(int killer, int killee)
    {
        score_list[killer] += 1;
        score_list[killee] += -1;
        RenderScores();
    }
}
