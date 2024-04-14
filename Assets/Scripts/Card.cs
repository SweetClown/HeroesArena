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
    public UnityEngine.UI.Button button;//���ư��o
    private Vector3 initPos;
    private Tween tween;
    private bool isDraging;
    private bool ShowCharacter;
    public GameObject CharacterShowGo;//����չʾģ��
    private Camera cam;
    public Text cardText;//��������
    public GameObject magicCircleGo;//���gጷŹ����@ʾ�[��ģ��
    public Transform imgEnergyT;//�}ˮ�DƬ�@ʾUI�[��ģ��
    public int posID;
    public GameObject[] modelGos; //��ǰ����ʹ��ǰ, ��Ҫ���ɵ�ģ��ģ�M



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
            if (CharacterShowGo.transform.localScale.x <= 0) //�����@ʾ����
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
            if (button.transform.localScale.x <= 0) //�����@ʾģ��
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
        if (ShowCharacter) //�@ʾģ��
        {
            imgEnergyT.gameObject.SetActive(true);
            //�侀�z�y
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            //�z�y��ǰʹ�ÿ����Ƿ��ڿ�Ƭʹ�ù�����

            imgEnergyT.DOLocalMove(imgEnergyT.localPosition + new Vector3(0, 50, 0), 0.5f).OnComplete(() => { UseCurrentCard(hits);});
        }
        else //�@ʾ����
        {
            ReturnToInitPos();
        }
    }
    /// <summary>
    /// ��Ļ�����D�Q��������
    /// </summary>
    /// <param name="screenPoint">��Ļ�����ϵ��c</param>
    /// <param name="planeZ">���xcamera Zƽ��ľ��x</param>
    /// <returns></returns>
    private Vector3 ScreenPointToWorldPoint(Vector2 screenPoint, float planeZ)
    {
        return cam.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, planeZ));
    }

    /// <summary>
    /// ��ʹ�ÿ��Ʒ��س�ʼλ��
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
    /// ʹ�î�ǰ����
    /// </summary>
    private void UseCurrentCard(RaycastHit[] hits) 
    {
        //�}ˮ����
        GameController.Instance.DecreaseEnergyValue(id);
        //ѭ�h�侀�z�y���е���ײ���Л]�е���
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.collider != null&&hit.collider.tag =="Plane") 
            {
                Vector3 targetpos = hit.point;
                //����� ���Ɍ�������ID�Ĺ����λ
                GameController.Instance.CreateUnit(id ,targetpos);
                //�õ���ǰ������λ�Þ�� ��Ҫ�a�Ͽ���
                UIManager.Instance.UseCard(posID);
                //ʹ�ÿ���������m����
                UIManager.Instance.RemoveCardIDInList(id);
                Destroy(gameObject);
            }
        }


        //ʹ�ÿ���������m����

        //������������
    }

}

