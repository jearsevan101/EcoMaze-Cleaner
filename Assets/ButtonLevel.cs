using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private TextMeshProUGUI timeRemaining;


    public void updateScore(int score, int time){
        bestScore.text= "Best Score:" + score.ToString();
        timeRemaining.text= "Remaining:" + time.ToString();
    }
}
