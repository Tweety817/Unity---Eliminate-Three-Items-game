using UnityEngine;
using System.Collections;

public class Gemstone : MonoBehaviour {
    public float xOffset =400.0f;  //x方向的偏移
    public float yOffset = -600.0f;  //y方向的偏移
    public int rowIndex = 0;       //行
    public int columnIndex = 0;    //列

	public GameObject[] gemstoneBgs;  //寶石的背景
	public int gemstoneType;  //寶石的類型，用來比對匹配
    private GameController gameController;
    public SpriteRenderer spriteRenderer;
	public bool isSelected{
		set{
			if(value){
				spriteRenderer.color = Color.red;
			}else{
				spriteRenderer.color = Color.white;
			}
		}
	}

	private GameObject gemstoneBg;
	

	void Start () {
        
        gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		spriteRenderer = gemstoneBg.GetComponent<SpriteRenderer> ();
	}
	
    //只有二維向量的話寶石依然不知道生成位置，調整位置間距
	public void UpdatePosition(int _rowIndex,int _columnIndex){  
		rowIndex = _rowIndex;
		columnIndex = _columnIndex;
		this.transform.position = new Vector3 (columnIndex*1.3f -4.5f , rowIndex*1.3f-6f , 0);
	}

    //滑順移動位置
	public void TweenToPostion(int _rowIndex,int _columnIndex){
		rowIndex = _rowIndex;
		columnIndex = _columnIndex;
		iTween.MoveTo (this.gameObject, iTween.Hash ("x",columnIndex * 1.3f-4.5f , "y",rowIndex*1.3f -6f,"time",0.3f));
    }

    //隨機生成寶石類型
    public void RandomCreateGemstoneBg(){ 
		if (gemstoneBg != null)
			return;
		gemstoneType = Random.Range (0, gemstoneBgs.Length-2); //只有0~6，共有9種，最後兩種是特殊寶石
		gemstoneBg = Instantiate(gemstoneBgs[gemstoneType]) as GameObject;
		gemstoneBg.transform.parent = this.transform;
	}
    //滑鼠點下，跟控制器說已選擇這個
	public void OnMouseDown(){
		gameController.Select (this);
	}

    //消除寶石
	public void Dispose(){
		Destroy (this.gameObject);
		Destroy (gemstoneBg.gameObject);
		gameController = null;
	}
}
