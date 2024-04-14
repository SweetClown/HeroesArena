using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.UIElements.Experimental;

/// <summary>
/// UI Manager
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text energyText;
    public Slider energySlider;
    public Text LeftTimeText;
    private List<int> CardIDList = new List<int>() ;
    public GameObject cardGameObject;
    public Sprite[] cardSprites;
    public Sprite[] cardDisSprites;
    private int maxContentNum = 4;
    private int currentBoardNum;
    public Transform nextCardT;
    public Transform[] BoardCardT;
    public Transform boardTrans;

    private void Awake()
    {
        Instance = this;
        CreateNewCard();
    }

    void Update()
    {
        
    }

    public void SetEnergySliderValue() 
    {
        energyText.text = ((int)GameController.Instance.energyValue).ToString();
        energySlider.value = GameController.Instance.energyValue / 10;

    }

    public void SetTimeValue(int min , int sec) 
    {
        LeftTimeText.text = min.ToString() + ":" + sec.ToString();
    }

    private void CreateNewCard() 
    {
        if (currentBoardNum > maxContentNum) 
        {
            return;
        }

        GameObject gameObject = Instantiate(cardGameObject , nextCardT );
        gameObject.transform.localPosition = Vector3.zero;

        int randomNum = Random.Range(1 , 11);
        while (CardIDList.Contains(randomNum)) //Identity the List is that include Random Card Number
        {
            randomNum = Random.Range(1, 11);
            //If include, Random generator the Card ID
        }
        CardIDList.Add(randomNum);
        Image image = gameObject.transform.GetChild(0).GetComponent<Image>();
        //設置卡牌樣式
        image.sprite = cardSprites[randomNum - 1] ;
        //設置不可使用的情況下卡牌樣式
        gameObject.GetComponent<Card>().id = randomNum;
        Button button = gameObject.transform.GetChild(0).GetComponent<Button>();
        SpriteState ss = button.spriteState;
        ss.disabledSprite = cardDisSprites[randomNum - 1];
        button.spriteState = ss;
        if (currentBoardNum < maxContentNum) 
        {
            MoveCardToBoard(currentBoardNum);
        }
    }
    /// <summary>
    /// Move Card to Card Board
    /// </summary>
    /// <param name="posID">Card Board ID</param>
    private void MoveCardToBoard(int posID)
    {
        Transform t = nextCardT.GetChild(0);
        t.SetParent(boardTrans);
        t.DOScale(Vector3.one, 0.2f);
        t.GetComponent<Card>().posID = posID;
        t.DOLocalMove(BoardCardT[posID].localPosition , 0.2f).OnComplete(() => { CompeleteMoveTween(t); });//匿名函數
        
    }
    /// <summary>
    /// Card Move Animation finish Method
    /// </summary>
    private void CompeleteMoveTween(Transform t) 
    {
        currentBoardNum++;
        CreateNewCard();
        t.GetComponent<Card>().SetInitPos();
    }

    /// <summary>
    /// 用掉卡牌後補充卡牌
    /// </summary>
    /// <param name="posID"></param>
    public void UseCard(int posID) 
    {
        currentBoardNum--;
        MoveCardToBoard(posID);
    }

    /// <summary>
    /// 將當前使用卡牌的id從列表中移除
    /// </summary>
    /// <param name="id"></param>
    public void RemoveCardIDInList(int id) 
    {
        CardIDList.Remove(id);
    }


}
