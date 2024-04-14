using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int id;
    public UnityEngine.UI.Button button;//卡牌按鈕
    private Vector3 initPos;
    private Tween tween;
    private bool isDraging;
    private bool ShowCharacter;
    public GameObject CharacterShowGo;//人物展示模型
    private Camera cam;
    public Text cardText;//卡牌名字
    public GameObject magicCircleGo;//法術釋放範圍顯示遊戲模型
    public Transform imgEnergyT;//聖水圖片顯示UI遊戲模型
    public int posID;
    public GameObject[] modelGos; //當前卡牌使用前, 想要生成的模型模組



    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        for (int i = 0; i < modelGos.Length; i++) 
        {
            modelGos[i].SetActive(false);
        }
        if (id <= 8)
        {
            modelGos[id - 1].SetActive(true);
            magicCircleGo.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        button.interactable = GameController.Instance.CanUseCard(id);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable == false)
        {
            return;
        }
        if (!isDraging)
        {
            tween = transform.DOLocalMove(initPos + new Vector3(0, 50, 0), 0.1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable == false)
        {
            return;
        }
        if (!isDraging)
        {
            tween.Pause();
            transform.localPosition = initPos;
        }
    }

    public void SetInitPos()
    {
        initPos = transform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (button.interactable == false)
        {
            return;
        }
        tween.Pause();
        isDraging = true;

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (button.interactable == false)
        {
            return;
        }
        Vector2 cardPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out cardPos);
        transform.localPosition = cardPos;
        if (ShowCharacter)// Show Character
        {
            float scale = Mathf.Clamp(((transform.localPosition.y - initPos.y) - 100) / 100, 0, 1);
            CharacterShowGo.transform.position = ScreenPointToWorldPoint(transform.position, 14.46f);
            CharacterShowGo.transform.localScale = Vector3.one * scale;
            if (CharacterShowGo.transform.localScale.x <= 0) //即將顯示卡牌
            {
                ShowCharacter = false;
                button.gameObject.SetActive(true);
                CharacterShowGo.gameObject.SetActive(false);
                if (id > 8)
                {
                    magicCircleGo.SetActive(false);
                }
                cardText.gameObject.SetActive(false);

            }

        }
        else //Show Card
        {
            float scale = Mathf.Clamp((100 - (transform.localPosition.y - initPos.y)) / 100, 0, 1);
            button.transform.localScale = Vector3.one * scale;
            if (button.transform.localScale.x <= 0) //即將顯示模型
            {
                ShowCharacter = true;
                button.gameObject.SetActive(false);
                CharacterShowGo.gameObject.SetActive(true);
                if (id > 8)
                {
                    magicCircleGo.SetActive(true);
                }
                cardText.gameObject.SetActive(true);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (button.interactable == false)
        {
            return;
        }
        button.transform.localScale = Vector3.one;
        if (ShowCharacter) //顯示模型
        {
            imgEnergyT.gameObject.SetActive(true);
            //射線檢測
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            //檢測當前使用卡牌是否在卡片使用範圍內

            imgEnergyT.DOLocalMove(imgEnergyT.localPosition + new Vector3(0, 50, 0), 0.5f).OnComplete(() => { UseCurrentCard(hits);});
        }
        else //顯示卡牌
        {
            ReturnToInitPos();
        }
    }
    /// <summary>
    /// 屏幕座標轉換世界座標
    /// </summary>
    /// <param name="screenPoint">屏幕座標上的點</param>
    /// <param name="planeZ">距離camera Z平面的距離</param>
    /// <returns></returns>
    private Vector3 ScreenPointToWorldPoint(Vector2 screenPoint, float planeZ)
    {
        return cam.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, planeZ));
    }

    /// <summary>
    /// 不使用卡牌返回初始位置
    /// </summary>
    private void ReturnToInitPos()
    {
        button.gameObject.SetActive(true);
        CharacterShowGo.gameObject.SetActive(false);
        cardText.gameObject.SetActive(false);
        imgEnergyT.gameObject.SetActive(false);
        transform.DOLocalMove(initPos, 0.2f).OnComplete(() => { isDraging = false; });
    }

    /// <summary>
    /// 使用當前卡牌
    /// </summary>
    private void UseCurrentCard(RaycastHit[] hits) 
    {
        //聖水消耗
        GameController.Instance.DecreaseEnergyValue(id);
        //循環射線檢測所有的碰撞器有沒有地面
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.collider != null&&hit.collider.tag =="Plane") 
            {
                Vector3 targetpos = hit.point;
                //如果有 生成對應卡牌ID的怪物單位
                GameController.Instance.CreateUnit(id ,targetpos);
                //用掉當前卡牌後位置為空 需要補上卡牌
                UIManager.Instance.UseCard(posID);
                //使用卡牌後的後續工作
                UIManager.Instance.RemoveCardIDInList(id);
                Destroy(gameObject);
            }
        }


        //使用卡牌後的後續工作

        //例如消除卡牌
    }

}

