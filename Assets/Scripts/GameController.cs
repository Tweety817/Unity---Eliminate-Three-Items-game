using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public ScoreMng _ScoreMng; //掌管分數、輸贏轉場、計時
    public Gemstone gemstone;
	public int rowNum = 10; //y，橫列
    public int columnNum = 8;  //x，直欄
	public ArrayList gemstoneList;
    public bool MapIsReady;
	public AudioClip match3Clip;
	public AudioClip swapClip;
	public AudioClip errorClip;
    public FindMatch _FindMatch;

    private Gemstone currentGemstone;
	private ArrayList matchesGemstone;
    private bool FirstMove;

    // Use this for initialization
    void Start () {
        ResetGame();

    }

    //重置遊戲
    public void ResetGame()
    {  
        MapIsReady = false;
        Debug.Log("重置遊戲");
        Map();//生成地圖
        FirstMove = false;//玩家還沒動
        matchesGemstone = new ArrayList();
        //查看橫排直列有沒有匹配的，如果一生成就有需要取消除的寶石就消除
        if (CheckHorizontalMatches() || CheckVerticalMatches())
        {
            RemoveMatches();
        }
        if (_FindMatch.CanMatch() == false)
        {
            UpdateMap();
        }
    }

    //生成寶石地圖
    private void Map()
    {
        gemstoneList = new ArrayList();
        for (int rowIndex = 0; rowIndex < rowNum ; rowIndex++)
        {
            ArrayList temp = new ArrayList();
            for (int columnIndex = 0; columnIndex < columnNum; columnIndex++)
            {
                Gemstone c = AddGemstone(rowIndex, columnIndex);
                temp.Add(c);
            }
            gemstoneList.Add(temp);
        }
        MapIsReady = true;
    }
    //重刷寶石地圖
    private void UpdateMap()
    {
        //把每個寶石換圖
        foreach (ArrayList temp in gemstoneList)
        {
            foreach (Gemstone c in temp)
            {
                c.RandomCreateGemstoneBg();
                Debug.Log("重刷地圖");
            }
        }
        FirstMove = false;//玩家還沒動
        //如果換圖後又沒有可以配對的，就再換圖
        if (_FindMatch.CanMatch() == false)
        {
            UpdateMap();
        }
    }

    //加入寶石
    public Gemstone AddGemstone(int rowIndex,int columnIndex){ 
		Gemstone c = Instantiate (gemstone) as Gemstone;
		c.transform.parent = this.transform;
		c.GetComponent<Gemstone>().RandomCreateGemstoneBg();
		c.GetComponent<Gemstone>().UpdatePosition (rowIndex,columnIndex);
		return c;
	}

    //通過行列，取得寶石
    public Gemstone GetGemstone(int rowIndex,int columnIndex){    
		ArrayList temp = gemstoneList [rowIndex] as ArrayList;
		Gemstone c = temp [columnIndex] as Gemstone;
		return c;
	}

    //設置寶石的位置
    public void SetGemstone(int rowIndex,int columnIndex, Gemstone c){  
		ArrayList temp = gemstoneList [rowIndex] as ArrayList;
		temp [columnIndex] = c;
	}

    //選擇到寶石
	public void Select(Gemstone c){
		if (currentGemstone == null) {
			currentGemstone = c;
			currentGemstone.isSelected = true;
            Debug.Log("選了第1個");
			return;
		}else{
			if( Mathf.Abs(currentGemstone.rowIndex - c.rowIndex) + Mathf.Abs(currentGemstone.columnIndex - c.columnIndex) == 1 ){
                Debug.Log("選了第2個");
                StartCoroutine (ExangeAndMatches(currentGemstone,c));
			}else{
				GetComponent<AudioSource>().PlayOneShot(errorClip);
                Debug.Log("選了第2個，但超出範圍");
            }
			currentGemstone.isSelected = false;
			currentGemstone = null;
		}
	}
    //寶石交換跟檢驗是否有至少三個一樣的寶石
	IEnumerator ExangeAndMatches(Gemstone c1,Gemstone c2){ 
		Exchange(c1,c2);
        FirstMove = true;
		yield return new WaitForSeconds (0.5f);
        if (L_Match(c1) || L_Match(c2)) //L配對
        {
            Debug.Log("是L配對");
            _ScoreMng.GetScore(80);
        }
        else if(T_Match(c1) || T_Match(c2)) //T配對
        {
            Debug.Log("是T配對");
            _ScoreMng.GetScore(80);
        }
        else if(CheckHorizontalMatches() || CheckVerticalMatches() ){//檢測開始!
            Debug.Log("是一般配對");
            if(matchesGemstone.Count == 3)
            {
                _ScoreMng.GetScore(10);
            }
            else if (matchesGemstone.Count == 4)
            {
                _ScoreMng.GetScore(30);
            }
            else if(matchesGemstone.Count >= 5)
            {
                _ScoreMng.GetScore(50);
            }
            RemoveMatches();
        }
		else{
			Debug.Log ("沒檢測到，把寶石交換回來");
			Exchange(c1,c2);
        }
        
	}

    public Sprite Rainbow_sprite;
    //換成彩虹寶石
    private void RainbowGem(int y, int x)
    {
        Gemstone temp = GetGemstone(y, x);
        temp.name = "RainbowGem";
        Debug.Log(temp.name + "  X=" + temp.columnIndex + "|  Y=" + temp.rowIndex);
        temp.gemstoneType = 7;//第8種寶石背景是虹 
        temp.GetComponentInChildren<SpriteRenderer>().sprite = Rainbow_sprite;
        Debug.Log("換彩虹寶石~");
    }


    public Sprite Black_sprite;
    //換成黑寶石
    private void BlackGem(int y,int x)
    {
       Gemstone temp =  GetGemstone(y, x);
        temp.name = "BlackGem";
        Debug.Log(temp.name + "  X=" + temp.columnIndex + "|  Y=" + temp.rowIndex);
        temp.gemstoneType = 8;//第9種寶石背景是黑
        temp.GetComponentInChildren<SpriteRenderer>().sprite = Black_sprite;
        Debug.Log("換成黑色寶石~");
    }

    //L檢測
    bool L_Match( Gemstone c)
    {
        //c、第1種L型，開口向右上
        if (c.rowIndex < 8 && c.columnIndex < 6)
        {
            if (GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex + 1, c.columnIndex).gemstoneType &&
              GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex + 2, c.columnIndex).gemstoneType &&
               GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex + 1).gemstoneType &&
                GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex + 2).gemstoneType)
            {
                for (int i = c.rowIndex; i < c.rowIndex + 3 && i < rowNum ; i++)
                {
                    for (int j = c.columnIndex; j < c.columnIndex + 3 && j < columnNum ; j++)
                    {
                        AddMatches(GetGemstone(i, j));
                        Debug.Log("消除了L配對 c、第1種L型");
                    }
                }
                Gemstone temp = GetGemstone(c.rowIndex + 1, c.columnIndex + 1);
                L_RemoveMatches(temp);
                return true;
            }
        }
        //c、第2種L型，開口向左上
        if (c.rowIndex < 7 && 1 < c.columnIndex)
        {
            if (GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex + 1, c.columnIndex).gemstoneType &&
            GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex + 2, c.columnIndex).gemstoneType &&
             GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex - 1).gemstoneType &&
              GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex - 2).gemstoneType)
            {
                for (int i = c.rowIndex; i < c.rowIndex + 3 && i < rowNum ; i++)
                {
                    for (int j = c.columnIndex; j > c.columnIndex - 3 && j > -1; j--)
                    {
                        AddMatches(GetGemstone(i, j));
                        Debug.Log("消除了L配對 c、第2種L型");
                    }
                }
                Gemstone temp = GetGemstone(c.rowIndex + 1, c.columnIndex - 1);
                L_RemoveMatches(temp);
                return true;
            }
        }

        //c、第3種L型 ，開口向左下
        if (1 < c.rowIndex && 1 < c.columnIndex)
        {
            if (GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex - 1, c.columnIndex).gemstoneType &&
            GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex - 2, c.columnIndex).gemstoneType &&
             GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex - 1).gemstoneType &&
              GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex - 2).gemstoneType)
            {
                for (int i = c.rowIndex; i > c.rowIndex - 3 && i > -1; i--)
                {
                    for (int j = c.columnIndex; j > c.columnIndex - 3 && j > -1; j--)
                    {
                        AddMatches(GetGemstone(i, j));
                        Debug.Log("消除了L配對 c、第3種L型");
                    }
                }
                Gemstone temp = GetGemstone(c.rowIndex - 1, c.columnIndex - 1);
                L_RemoveMatches(temp);
                return true;
            }
        }
        //c、第4種L型，開口向右下
        if (1 < c.rowIndex && c.columnIndex < 6)
        {
            if (GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex - 1, c.columnIndex).gemstoneType &&
            GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex - 2, c.columnIndex).gemstoneType &&
             GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex + 1).gemstoneType &&
              GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex + 2).gemstoneType)
            {
                for (int i = c.rowIndex; i > c.rowIndex - 3 && i > -1; i--)
                {
                    for (int j = c.columnIndex; j < c.columnIndex + 3 && j<columnNum; j++)
                    {
                        AddMatches(GetGemstone(i, j));
                        Debug.Log("消除了L配對 c、第4種L型");
                    }
                }
                Gemstone temp = GetGemstone(c.rowIndex - 1, c.columnIndex + 1);
                L_RemoveMatches(temp);
                return true;
            }
        }
        //如果都不是，那就不是L字型配對
        return false;
    }

    //清除整個列跟欄，T檢測會用到
    private void RemoveCross(Gemstone c)
    {
        int y = c.rowIndex;
        int x = c.columnIndex;
        for (int i = 0; i < rowNum; i++)
        {
            AddMatches(GetGemstone(i, x));
        }
        for (int j = 0; j < columnNum; j++)
        {
            AddMatches(GetGemstone(y, j));
        }
        T_RemoveMatches(y,x);
    }

    //T檢測
    bool T_Match(Gemstone c)
    {
        //c、第1種T型，腳向左
        if (c.rowIndex < rowNum -1 && c.rowIndex > 0 && c.columnIndex > 1)
        {
            if (GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex + 1, c.columnIndex).gemstoneType &&
            GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex - 1, c.columnIndex).gemstoneType &&
             GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex - 1).gemstoneType &&
              GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex - 2).gemstoneType)
            {
                RemoveCross(c);
                Debug.Log("第1種T型，腳向左");
                return true;
            }
        }
        //c、第2種T型，腳向右
        if (c.rowIndex < rowNum - 1 && c.rowIndex > 0 &&  c.columnIndex<6)
        {
            if (GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex + 1, c.columnIndex).gemstoneType &&
            GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex - 1, c.columnIndex).gemstoneType &&
             GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex + 1).gemstoneType &&
              GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex + 2).gemstoneType)
            {
                RemoveCross(c);
                Debug.Log("第2種T型，腳向右");
                return true;
            }
        }
        //c、第3種T型，正T
        if (c.rowIndex > 1 && c.columnIndex > 0 && c.columnIndex < columnNum - 1)
        {
            if (GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex - 1, c.columnIndex).gemstoneType &&
            GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex - 2, c.columnIndex).gemstoneType &&
             GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex - 1).gemstoneType &&
              GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex + 1).gemstoneType)
            {
                RemoveCross(c);
                Debug.Log("第3種T型，正T");
                return true;
            }
        }
        //c、第4種T型，腳朝上
        if (rowNum - 2 > c.rowIndex && c.columnIndex > 0 && c.columnIndex < columnNum - 1)
        {
            if (GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex + 1, c.columnIndex).gemstoneType &&
                       GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex + 2, c.columnIndex).gemstoneType &&
                        GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex - 1).gemstoneType &&
                         GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == GetGemstone(c.rowIndex, c.columnIndex + 1).gemstoneType)
            {
                RemoveCross(c);
                Debug.Log("第4種T型，腳朝上");
                return true;
            }
        }
        //如果都不是，那就不是T字型配對
        return false;
    }

    //水平檢測
    bool CheckHorizontalMatches(){  
		bool isMatches = false;
		for (int rowIndex = 0; rowIndex < rowNum; rowIndex++) {
			for(int columnIndex =0; columnIndex < columnNum - 2; columnIndex++){
				if((GetGemstone (rowIndex,columnIndex).gemstoneType == GetGemstone (rowIndex,columnIndex + 1).gemstoneType ) && (GetGemstone (rowIndex,columnIndex).gemstoneType == GetGemstone (rowIndex,columnIndex + 2).gemstoneType )){
					Debug.Log ("發現同列的寶石");
					AddMatches (GetGemstone (rowIndex,columnIndex));
					AddMatches (GetGemstone (rowIndex,columnIndex + 1));
					AddMatches (GetGemstone (rowIndex,columnIndex + 2));
					isMatches = true;
				}
			}	
		}
		return isMatches;
	
	}

    //垂直檢測
	bool CheckVerticalMatches(){   
		bool isMatches = false;
		for(int columnIndex = 0; columnIndex < columnNum; columnIndex++){
			for(int rowIndex = 0; rowIndex < rowNum -2 ;rowIndex++){
				if((GetGemstone(rowIndex,columnIndex).gemstoneType == GetGemstone (rowIndex + 1,columnIndex).gemstoneType ) & (GetGemstone(rowIndex,columnIndex).gemstoneType == GetGemstone (rowIndex + 2,columnIndex).gemstoneType )  ){
					Debug.Log ("發現行相同的寶石");
					AddMatches (GetGemstone (rowIndex,columnIndex));
					AddMatches (GetGemstone (rowIndex +1,columnIndex));
					AddMatches (GetGemstone (rowIndex +2,columnIndex));
					isMatches = true;
				}
			}

		}
		return isMatches;
	}

	void AddMatches(Gemstone c){
		if(matchesGemstone == null)
			matchesGemstone = new ArrayList();
		int index = matchesGemstone.IndexOf (c);  //檢測寶石是否已經加入匹配list
		if(index == -1){
			matchesGemstone.Add (c);
            foreach (Gemstone item in matchesGemstone)
            {
                Debug.Log("X=" + item.columnIndex+" | Y=" +item.rowIndex);
            }
		}
        else
        {
            Debug.Log("這寶石已經加入過匹配清單了");
        }
	}

    //删除匹配的寶石
    void RemoveMatches(){    
		for(int i=0; i< matchesGemstone.Count; i++){
			Gemstone c = matchesGemstone[i] as Gemstone;
			RemoveGemstone(c);
		}
		matchesGemstone = new ArrayList ();
		StartCoroutine (WaitForCheckMatchesAgain ());
	}

    //删除L匹配的寶石
    void L_RemoveMatches(Gemstone L)
    {
        for (int i = 0; i < matchesGemstone.Count; i++)
        {
            Gemstone c = matchesGemstone[i] as Gemstone;
            RemoveGemstone(c);
        }
        Debug.Log("消除L寶石！");
        matchesGemstone = new ArrayList();
        int y = L.rowIndex;
        int x = L.columnIndex;
        RainbowGem(y,x);
        StartCoroutine(WaitForCheckMatchesAgain());
    }

    //删除T匹配的寶石
    void T_RemoveMatches(int y,int x)
    {
        for (int i = 0; i < matchesGemstone.Count; i++)
        {
            Gemstone c = matchesGemstone[i] as Gemstone;
            RemoveGemstone(c);
        }
        Debug.Log("消除T寶石！");
        matchesGemstone = new ArrayList();
        BlackGem(y,x);
        StartCoroutine(WaitForCheckMatchesAgain());
        
    }

    // 等待下一次配對，因為有可能配對一次消除後，重新生成又有配對到的
    IEnumerator WaitForCheckMatchesAgain(){
        
        yield return new WaitForSeconds (0.5f);
        if (FirstMove) //玩家動了
        {
            if (CheckHorizontalMatches() || CheckVerticalMatches())
            {
                if (matchesGemstone.Count == 3)
                {
                    _ScoreMng.GetScore(10);
                }
                else if (matchesGemstone.Count == 4)
                {
                    _ScoreMng.GetScore(30);
                }
                else if (matchesGemstone.Count >= 5)
                {
                    _ScoreMng.GetScore(50);
                }
                RemoveMatches();
            }
        }
        else
        {
            if (CheckHorizontalMatches() || CheckVerticalMatches())
            {
                //玩家沒動第一步，代表是生成地圖時，去掃配對，所以不須加分
                RemoveMatches();
            }
        }
        if (!_FindMatch.CanMatch())
        {
            UpdateMap();
        }
    }


	void RemoveGemstone(Gemstone c){  //消除寶石
		Debug.Log ("消除寶石！");
		c.Dispose ();
		GetComponent<AudioSource>().PlayOneShot (match3Clip);
		for (int i=c.rowIndex +1; i<rowNum; i++) {
			Gemstone temGemstone = GetGemstone (i,c.columnIndex);
			temGemstone.rowIndex --;
			SetGemstone(temGemstone.rowIndex,temGemstone.columnIndex,temGemstone);
			//temGemstone.UpdatePosition (temGemstone.rowIndex,temGemstone.columnIndex);
			temGemstone.TweenToPostion (temGemstone.rowIndex,temGemstone.columnIndex);
		}

		Gemstone newGemstone = AddGemstone (rowNum, c.columnIndex);
		newGemstone.rowIndex--;
		SetGemstone (newGemstone.rowIndex, newGemstone.columnIndex, newGemstone);
		//newGemstone.UpdatePosition (newGemstone.rowIndex, newGemstone.columnIndex);
		newGemstone.TweenToPostion (newGemstone.rowIndex, newGemstone.columnIndex);
	}

	public void Exchange(Gemstone c1,Gemstone c2){  //寶石交換位置
        GetComponent<AudioSource>().PlayOneShot (swapClip);
		SetGemstone (c1.rowIndex, c1.columnIndex, c2);
		SetGemstone (c2.rowIndex, c2.columnIndex, c1);

		//交换c1,c2的行
		int tempRowIndex;
		tempRowIndex = c1.rowIndex;
		c1.rowIndex = c2.rowIndex;
		c2.rowIndex = tempRowIndex;

		//交换c1,c2的列
		int tempColumnIndex;
		tempColumnIndex = c1.columnIndex;
		c1.columnIndex = c2.columnIndex;
		c2.columnIndex = tempColumnIndex;

		//c1.UpdatePosition (c1.rowIndex, c1.columnIndex);
		//c2.UpdatePosition (c2.rowIndex, c2.columnIndex);
		c1.TweenToPostion (c1.rowIndex, c1.columnIndex);
		c2.TweenToPostion (c2.rowIndex, c2.columnIndex);
        Debug.Log("交換寶石位置囉~");
    }


}
