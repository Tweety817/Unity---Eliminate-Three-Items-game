using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GemSpace{
    public class GameCtrl : MonoBehaviour
    {
        public Gem gem;
        public int TotalRow = 6;
        public int TotalColumn = 10;
        public int offset = 20;
        public ArrayList gemList;
        private int gemType;


        // Use this for initialization
        void Start()
        {
            gemList = new ArrayList();
            matchedGemList = new ArrayList();
            Map();//生成地圖
            //如果生成完就有連續3個以上的，就消除寶石
            if (CheckHorizontalMatch() || CheckVerticalMatch())
            {
                RemoveMatch();
            }
        }

        //生成地圖
        private void Map()
        {
             gemList = new ArrayList();
            for (int i = 0; i < TotalRow; i++)
            {
                ArrayList temp = new ArrayList();
                for (int j = 0; j < TotalColumn; j++)
                {
                    GameObject c = AddGem(i, j);
                    temp.Add(c);
                }
                gemList.Add(temp);
            }
        }

        //生成地圖上的遊戲物件(寶石)
        public GameObject AddGem(int _rowIndex, int _columnIndex)
        {
            gemType = Random.Range(1, 9);
            GameObject g = Instantiate(Resources.Load("prefabs/" + gemType)) as GameObject;
            g.GetComponent<RectTransform>().SetParent(this.transform);
            //g.transform.parent = this.transform;
            g.GetComponent<Gem>().UpdatePos(_rowIndex, _columnIndex);
            return g;
        }

        //點擊寶石(遊戲物件)後，要讓兩個寶石位置交換，先確認玩家點擊哪個寶石
        private GameObject currentGem;//紀錄目前點的寶石
        public void Select(GameObject g)
        {
            if (currentGem == null)//如果還沒點
            {
                currentGem = g;
                currentGem.GetComponent<Gem>().isSelected = true;
                return;
            }
            else //如果已經點了，最多只能點兩個，而且只能點第一個被點的寶石附近一格的上下左右
            {  //下面這行是設置限制只能點上下左右一個
                if (Mathf.Abs(currentGem.GetComponent<Gem>().rowNum - g.GetComponent<Gem>().rowNum) + Mathf.Abs(currentGem.GetComponent<Gem>().columnNum - g.GetComponent<Gem>().columnNum) == 1)
                {
                    //延遲幾秒交換跟比對
                    StartCoroutine(ExangeAndMatches(currentGem, g));
                }
                else
                {
                    //如果點超出範圍的，播放音效
                }
                //限制只能點兩個(點第1個是true，點第2個是false，開關的概念)
                currentGem.GetComponent<Gem>().isSelected = false;
                currentGem = null;//限制只能點兩個(點了寶石之後不是null了，再點一次變成null)
            }
        }

          //透過行跟列，取得寶石位置，再設置新的寶石位置
         public GameObject GetGem(int rowIndex, int columnIndex)
        {
            ArrayList temp = gemList[rowIndex] as ArrayList; //先設置暫時的陣列，取得那整行寶石list
            GameObject g = temp[columnIndex] as GameObject; //再透過那整行寶石list，取得那個列上的寶石(遊戲物件)
            return g;
        }
         public void SetGem(int rowIndex, int columnIndex, GameObject g)
            {
                ArrayList temp = gemList[rowIndex] as ArrayList;//先設置暫時的陣列，取得那整行寶石list
                temp[columnIndex] = g;//再透過那整行寶石list，取得那個列上的位置，將寶石賦值給那個位置)
            }

        IEnumerator ExangeAndMatches( GameObject g1,GameObject g2)
        {
            Exchange(g1, g2); //交換位置
            yield return new WaitForSeconds(0.5f);
            if (CheckHorizontalMatch() || CheckVerticalMatch())
            {
                RemoveMatch();
            }
            else
            {
                Debug.Log("沒有檢測到一樣的寶石，交換回來");
                Exchange(g1, g2);//不能寫成Exchange(g2, g1);，因為原本的g1變成g2、g2變成g1
            }
        }

        //交換位置函式
        private void Exchange(GameObject g1, GameObject g2)
        {
            //播放交換音效，並設置新的交換位置(g2放在g1位置，g1放在g2位置)
            SetGem(g1.GetComponent<Gem>().rowNum, g1.GetComponent<Gem>().columnNum, g2);
            SetGem(g2.GetComponent<Gem>().rowNum, g2.GetComponent<Gem>().columnNum, g1);

            int g1_r = g1.GetComponent<Gem>().rowNum;
            int g2_r = g2.GetComponent<Gem>().rowNum;
            int g1_c = g1.GetComponent<Gem>().columnNum;
            int g2_c = g2.GetComponent<Gem>().columnNum;

            g1_r = g1_r ^ g2_r;
            g2_r = g2_r ^ g1_r;
            g1_r = g1_r ^ g2_r;

            g1_c = g1_c ^ g2_c;
            g2_c = g2_c ^ g1_c;
            g1_c = g1_c ^ g2_c;

        }

        //檢測匹配函式，看是否有3個以上連在一起
        bool CheckHorizontalMatch() //水平檢測
        {
            bool isMatch = false;
            for (int rowNum = 0; rowNum < TotalRow; rowNum++)//在再個垂直行內，查找一橫列上個位置是否有連續三個同樣的寶石
            {
                for (int columnNum = 0; columnNum < TotalColumn - 2; columnNum++)
                {      //利用遊戲物件上的tag來比較寶石種類是否相同
                    if( GetGem(rowNum,columnNum).tag == GetGem(rowNum, columnNum+1).tag && GetGem(rowNum, columnNum).tag == GetGem(rowNum, columnNum + 2).tag)
                    {
                        Debug.Log("水平檢驗，發現至少3個相同寶石");
                        isMatch = true; //找到後把這些寶石加到配對清單，這個清單內紀錄即將要消除的寶石們
                        AddMatchGem(GetGem(rowNum, columnNum ));
                        AddMatchGem( GetGem(rowNum, columnNum +1) );
                        AddMatchGem(GetGem(rowNum, columnNum + 2));
                        //for迴圈會一一檢驗每個寶石，所以假設是連續4個寶石也會被加入清單、等待消除
                    }
                }
            }
            return isMatch;
         }

        bool CheckVerticalMatch() //垂直檢測
        {
            bool isMatch = false;
            for (int columnNum = 0; columnNum < TotalColumn; columnNum++)
            {
                for (int rowNum = 0; rowNum < TotalRow - 2; rowNum++)
                {      //利用遊戲物件上的tag來比較寶石種類是否相同
                    if (GetGem(rowNum, columnNum).tag == GetGem(rowNum + 1, columnNum ).tag && GetGem(rowNum, columnNum).tag == GetGem(rowNum + 2, columnNum ).tag)
                    {
                        Debug.Log("垂直檢驗，發現至少3個相同寶石");
                        isMatch = true; //找到後把這些寶石加到配對清單，這個清單內紀錄即將要消除的寶石們
                        AddMatchGem( GetGem(rowNum, columnNum) );
                        AddMatchGem( GetGem(rowNum + 1, columnNum) );
                        AddMatchGem( GetGem(rowNum + 2, columnNum) );
                        //for迴圈會一一檢驗每個寶石，所以假設是連續4個寶石也會被加入清單、等待消除
                    }
                }
            }
            return isMatch;
        }

        private ArrayList matchedGemList;
        private void AddMatchGem(GameObject g) //把比對到的寶石加入清單
        {
            if (matchedGemList == null)
            {
                matchedGemList = new ArrayList();
            }
            //檢測該寶石是否已經加入清單，沒有的話就會是null，有的話indexOf會返回寶石是清單中第幾個
            int index = matchedGemList.IndexOf(g);
            if (index == null)
            {
                matchedGemList.Add(g);
            }
        }

        private void RemoveMatch() //消除清單內的寶石
        {
            for (int i = 0; i < matchedGemList.Count; i++)
            {
                GameObject g = matchedGemList[i] as GameObject;
                RemoveGem(g);
                //消除清單內的寶石，並等待下次比對
                //之所以要等待比對，是因為降下來的寶石有可能也配對到，不要一次消除完
            }
            matchedGemList = new ArrayList();
            StartCoroutine(WaitForCheckMatchAgain());
        }

        IEnumerator WaitForCheckMatchAgain()
        {
            yield return new WaitForSeconds(0.5f);
            if (CheckHorizontalMatch() || CheckVerticalMatch())
            {
                RemoveMatch();
            }
        }
        private void RemoveGem(GameObject g) //消除寶石遊戲物件
        {
            GameObject temp = new GameObject();
            temp.AddComponent<Gem>().rowNum = g.GetComponent<Gem>().rowNum;
            temp.AddComponent<Gem>().columnNum = g.GetComponent<Gem>().columnNum;

            Debug.Log("消除寶石囉!");
            Destroy(g);
            //播放消除音效
            //刪除完寶石後，要讓上方的寶石降下來
            for (int i = temp.GetComponent<Gem>().rowNum +1 ; i < TotalRow; i++) //查找所點寶石上方，一直到最上方
            {   //先取得點擊寶石的上方寶石的位置，讓他的位置降下來
                GameObject tempGem = GetGem( i , temp.GetComponent<Gem>().columnNum);
                tempGem.GetComponent<Gem>().rowNum--;
                SetGem( tempGem.GetComponent<Gem>().rowNum, tempGem.GetComponent<Gem>().columnNum, tempGem);
                //記得降的位置，重新設置間距
                tempGem.GetComponent<Gem>().UpdatePos( tempGem.GetComponent<Gem>().rowNum , tempGem.GetComponent<Gem>().columnNum);
                //使用ITween讓下降更滑順
            }

            //刪除寶石後，有空缺了，要生成新的寶石
            GameObject NewGem = AddGem(TotalRow, temp.GetComponent<Gem>().columnNum);
            NewGem.GetComponent<Gem>().rowNum--;
            SetGem(NewGem.GetComponent<Gem>().rowNum, NewGem.GetComponent<Gem>().columnNum, NewGem);
            //重新更新位置間距
            NewGem.GetComponent<Gem>().UpdatePos(NewGem.GetComponent<Gem>().rowNum, NewGem.GetComponent<Gem>().columnNum);
            //使用ITween讓下降更滑順

           

        }


    }
}

