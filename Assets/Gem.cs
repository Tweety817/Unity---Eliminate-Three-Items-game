using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GemSpace
{
    public class Gem : MonoBehaviour
    {
        public GameCtrl gameCtrl;
        internal int rowNum = 0;
        internal int columnNum = 0;
        internal int offset = 20;

        internal bool isSelected;
            
        // Use this for initialization
        void Start()
        {
            gameCtrl = GameObject.Find("GameCtrl").GetComponent<GameCtrl>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void OnMouseDown()//點擊選擇寶石
        {
            gameCtrl.Select(this.gameObject);//跟控制器說我點了這個寶石(遊戲物件)
        }

        //調整寶石位置
        public void UpdatePos(int _rowIndex, int _columnIndex)
        {
            rowNum = _rowIndex;
            columnNum = _columnIndex;
            offset = 80;
            this.GetComponent<Image>().rectTransform.position = new Vector3(rowNum * 40f +40, columnNum * 40f+ offset , 0);
        }

    }
}
  
