using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfor : MonoBehaviour
{
    public static PlayerInfor instance = null;
   
    [SerializeField]
    private int _id;
    [SerializeField]
    private int _GameCoin;
    [SerializeField]
    private int _RealCoin;
    [SerializeField]
    private int _stars;
    [SerializeField]
    private int _score;
    [SerializeField]
    private int _rank;
    [SerializeField]
    private string _rankName;

    public bool gameEffect = true;
    public bool gameBgm = true;
    public int IDInfo { get { return _id; } set { _id = value; } }
    public int DCoinInfo { get { return _RealCoin; } set { _RealCoin = value; } }
    public int GCoinInfo { get { return _GameCoin; } set { _GameCoin = value; } }
    public int StarsInfo { get { return _stars; } set { _stars = value; } }
    public int ScoreInfo { get { return _score; } set { _score = value; } }
    public int RankInfo { get { return _rank; } set { _rank = value; } }
    public string RankNameInfo { get { return _rankName; } set { _rankName = value; } }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }


}
