using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMng : MonoBehaviour {
    public GameController _GameController;
    private int Score;
    private Text ScoreText;
    private Text LeftTimeText;
    private int TimePass;
    private int TotalTime = 300;
    // Use this for initialization
    void Start () {
        ScoreText = GameObject.Find("Score").GetComponent<Text>();
        LeftTimeText = GameObject.Find("Left_Time").GetComponent<Text>();
        TimePass = 0;
        Score = 0;
    }
    void Update()
    {
        //if (Score >= 500)
        //{
        //    //其中一方滿500，判斷輸贏
        //}  
    }
    private void FixedUpdate()
    {
        TimeCountDown();
    }

    //遊戲時間倒數，倒數為0就結算成績
    private void TimeCountDown()
    {
        TimePass = Mathf.RoundToInt( Time.realtimeSinceStartup - 1/2);
        LeftTimeText.text = (TotalTime-TimePass).ToString();
        if (TotalTime == TimePass)
        {
            Debug.Log("時間到，遊戲結束，開始結算");
        }
    }

    //得分，並顯示分數改變
    public void GetScore(int Point)
    {
        Score += Point;
        ScoreText.text = Score.ToString();//顯示分數的變動
        Debug.Log(Score + "/" + ScoreText.ToString());
    }


}
