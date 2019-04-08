using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatch : MonoBehaviour
{
    public GameController _GameController;
    int TotalRow;
    int TotalColumn;
    private bool _MapIsReady;
    // Use this for initialization
    void Start()
    {

        TotalRow = _GameController.rowNum;
        TotalColumn = _GameController.columnNum;
    }
    //掃場上所有有那些格式的寶石
    public bool CanMatch()
    {
        ArrayList _gemstoneList = _GameController.gemstoneList;

        foreach (ArrayList g in _gemstoneList)
        {
            foreach (Gemstone c in g)
            {
                //吸管型第1種，_____/
                if (c.rowIndex < TotalRow - 1 && c.rowIndex > -1 && c.columnIndex > -1 && c.columnIndex < TotalColumn - 2)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex, c.columnIndex + 1).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex + 1, c.columnIndex + 2).gemstoneType)
                    {
                        Debug.Log("還有得消，吸管型第1種，_____/");
                        return true;//有得消就不刷新
                    }
                }
                //吸管型第2種，\_____
                if (c.rowIndex < TotalRow && c.rowIndex > 0 && c.columnIndex > -1 && c.columnIndex < TotalColumn - 2)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 1, c.columnIndex + 1).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 1, c.columnIndex + 2).gemstoneType)
                    {
                        Debug.Log("還有得消，吸管型第2種，\'____");
                        return true;//有得消就不刷新
                    }
                }
                //吸管型第3種，￣￣\
                if (c.rowIndex < TotalRow && c.rowIndex > 0 && c.columnIndex > -1 && c.columnIndex < TotalColumn - 2)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex, c.columnIndex + 1).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 1, c.columnIndex + 2).gemstoneType)
                    {
                        Debug.Log("還有得消，吸管型第3種，￣￣\'");
                        return true;//有得消就不刷新
                    }
                }
                //吸管型第4種，/￣￣
                if (c.rowIndex < TotalRow - 1 && c.rowIndex > -1 && c.columnIndex > -1 && c.columnIndex < TotalColumn - 2)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex + 1, c.columnIndex + 1).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex + 1, c.columnIndex + 2).gemstoneType)
                    {
                        Debug.Log("還有得消，吸管型第4種，/￣￣");
                        return true;//有得消就不刷新
                    }
                }
                //吸管型第5種，`∣
                if (c.rowIndex < TotalRow && c.rowIndex > 1 && c.columnIndex > -1 && c.columnIndex < TotalColumn - 1)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 1, c.columnIndex + 1).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 2, c.columnIndex + 1).gemstoneType)
                    {
                        Debug.Log("還有得消，吸管型第5種，`∣ ");

                        return true;//有得消就不刷新
                    }
                }
                //吸管型第6種，∣’
                if (c.rowIndex < TotalRow && c.rowIndex > 1 && c.columnIndex > 0 && c.columnIndex < TotalColumn)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 1, c.columnIndex - 1).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 2, c.columnIndex - 1).gemstoneType)
                    {
                        Debug.Log("還有得消，吸管型第6種，∣’ ");
                        return true;//有得消就不刷新
                    }
                }
                //吸管型第7種，,∣
                if (c.rowIndex < TotalRow && c.rowIndex > 1 && c.columnIndex > 0 && c.columnIndex < TotalColumn)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 1, c.columnIndex).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 2, c.columnIndex - 1).gemstoneType)
                    {
                        Debug.Log("還有得消，吸管型第7種，,∣ ");
                        return true;//有得消就不刷新
                    }
                }
                //吸管型第8種，︱﹑
                if (c.rowIndex < TotalRow && c.rowIndex > 1 && c.columnIndex > -1 && c.columnIndex < TotalColumn - 1)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 1, c.columnIndex).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 2, c.columnIndex + 1).gemstoneType)
                    {
                        Debug.Log("還有得消，吸管型第8種，︱﹑ ");
                        return true;//有得消就不刷新
                    }
                }
                //ㄑ型第1種，>
                if (c.rowIndex < TotalRow - 1 && c.rowIndex > 0 && c.columnIndex > 0 && c.columnIndex < TotalColumn)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex + 1, c.columnIndex - 1).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 1, c.columnIndex - 1).gemstoneType)
                    {
                        Debug.Log("還有得消，ㄑ型第1種，> ");
                        return true;//有得消就不刷新
                    }
                }
                //ㄑ型第2種，﹀
                if (c.rowIndex < TotalRow - 1 && c.rowIndex > -1 && c.columnIndex > 0 && c.columnIndex < TotalColumn - 1)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex + 1, c.columnIndex - 1).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex + 1, c.columnIndex + 1).gemstoneType)
                    {
                        Debug.Log("還有得消，ㄑ型第2種，﹀ ");
                        return true;//有得消就不刷新
                    }
                }
                //ㄑ型第3種，ㄑ
                if (c.rowIndex < TotalRow - 1 && c.rowIndex > 0 && c.columnIndex > -1 && c.columnIndex < TotalColumn - 1)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex + 1, c.columnIndex + 1).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 1, c.columnIndex + 1).gemstoneType)
                    {
                        Debug.Log("還有得消，ㄑ型第3種，ㄑ ");
                        return true;//有得消就不刷新
                    }
                }
                //ㄑ型第4種，^
                if (c.rowIndex < TotalRow && c.rowIndex > 0 && c.columnIndex > 0 && c.columnIndex < TotalColumn - 1)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 1, c.columnIndex + 1).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 1, c.columnIndex - 1).gemstoneType)
                    {
                        Debug.Log("還有得消，ㄑ型第4種，^ ");
                        return true;//有得消就不刷新
                    }
                }
                //直線第1種，O   OO
                if (c.rowIndex < TotalRow && c.rowIndex > -1 && c.columnIndex > -1 && c.columnIndex < TotalColumn - 3)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex, c.columnIndex + 2).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex, c.columnIndex + 3).gemstoneType)
                    {
                        Debug.Log("還有得消，直線第1種，O   OO");
                        return true;//有得消就不刷新
                    }
                }
                //直線第2種，上空下下
                if (c.rowIndex < TotalRow && c.rowIndex > 2 && c.columnIndex > -1 && c.columnIndex < TotalColumn)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 2, c.columnIndex).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 3, c.columnIndex).gemstoneType)
                    {
                        Debug.Log("還有得消，直線第2種，上空下下");
                        return true;//有得消就不刷新
                    }
                }
                //直線第3種，OO O
                if (c.rowIndex < TotalRow && c.rowIndex > -1 && c.columnIndex > -1 && c.columnIndex < TotalColumn - 3)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex, c.columnIndex + 1).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex, c.columnIndex + 3).gemstoneType)
                    {
                        Debug.Log("還有得消，直線第3種，OO O");
                        return true;//有得消就不刷新
                    }
                }
                //直線第4種，上上空下
                if (c.rowIndex < TotalRow && c.rowIndex > 2 && c.columnIndex > -1 && c.columnIndex < TotalColumn)
                {
                    if (_GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 1, c.columnIndex).gemstoneType &&
                         _GameController.GetGemstone(c.rowIndex, c.columnIndex).gemstoneType == _GameController.GetGemstone(c.rowIndex - 3, c.columnIndex).gemstoneType)
                    {
                        Debug.Log("還有得消，直線第4種，上上空下");
                        return true;//有得消就不刷新
                    }
                }
                //沒得消就刷新

            }
        }
        return false;
    }



    //void Update () {
    //    _MapIsReady = _GameController.MapIsReady;
    //    if (_MapIsReady) //確定地圖生成好後
    //    {
    //        CanMatch();//一直去掃場上所有有那些格式的寶石，有那種格式代表還有得消
    //    }
    //    if ( == false) //如果沒得刷新了，就請控制器重置遊戲
    //    {
    //        Debug.Log("沒得消了，刷新地圖");
    //        if (_GameController.gemstoneList != null)
    //        {
    //            foreach (ArrayList temp in _GameController.gemstoneList)
    //            {
    //                foreach (Gemstone item in temp)
    //                {
    //                    temp.Remove(item);
    //                    _GameController.gemstoneList.Remove(temp);
    //                }
    //            }
    //            _GameController.gemstoneList = null;
    //        }
    //        _GameController.ResetGame();
    //    }
    //}
}
