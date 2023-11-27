using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KeepScore : MonoBehaviour
{   
    public Text copsScoreText;
    public Text robsScoreText;
    private int copScore = 0;
    private int robScore = 0;
    public static KeepScore Scorekeeper;
    private void Awake(){
        Scorekeeper = this;
    }
    void Start(){
        copsScoreText.text = "";
        robsScoreText.text = "";
    }
    public void CopScore(){
        copsScoreText.text = null;
        copScore += 1;
        copsScoreText.text = copScore.ToString();
    }
        
    public void RobScore(){
        robScore += 1;
        robsScoreText.text ="";
        robsScoreText.text = robScore.ToString();
        
     }
}
