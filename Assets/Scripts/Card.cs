using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// Card UI
/// </summary>
public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int id;
    public Button button;//���ư��o
    private Vector3 initPos;
    private Tween tween;
    private bool isDraging;
    private bool ShowCharacter;
    public GameObject CharacterShowGo;//����չʾģ��
    private Camera cam;
    public Text cardText;//��������
    public GameObject magicCircleGo;//���gጷŹ����@ʾ�[��ģ��
    public Transform imgEnergyT;//�}ˮ�DƬ�@ʾUI�[��ģ��



    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
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
        ReturnToInitPos();
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

    private void ReturnToInitPos()
    {
        button.gameObject.SetActive(true);
        CharacterShowGo.gameObject.SetActive(false);
        cardText.gameObject.SetActive(false);
        imgEnergyT.gameObject.SetActive(false);
        transform.DOLocalMove(initPos, 0.2f).OnComplete(() => { isDraging = false; });
    }


}

