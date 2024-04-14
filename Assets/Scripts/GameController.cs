using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Game Controller
/// </summary>
public class GameController : MonoBehaviour
{

    public static GameController Instance;

    public float energyValue;
    public float leftTime;
    public List<UnitInfo> unitInfos;
    public GameObject[] unitGos; //�����A�u�w (�[�����w)���YԴ
    public Building[] PurpleBuilding;
    public Building[] OrangeBuilding;

    private void Awake()
    {
        Instance = this;
        energyValue = 1;
        leftTime = 180;
        unitInfos = new List<UnitInfo>()
        {
            new UnitInfo(){ id=1,unitName="���`������",cost=3,hp=10,attackArea=4,speed=1,attackValue=2},
            new UnitInfo(){ id=2,unitName="����ʹ",cost=4,hp=10,attackArea=3,speed=1,attackValue=1},
            new UnitInfo(){ id=3,unitName="���^��",cost=6,hp=30,attackArea=2,speed=1,attackValue=5},
            new UnitInfo(){ id=4,unitName="������ʹ",cost=6,hp=10,attackArea=2.5f,speed=2,attackValue=6},
            new UnitInfo(){ id=5,unitName="���ҾޫF",cost=8,hp=30,attackArea=2,speed=1,attackValue=4},
            new UnitInfo(){ id=6,unitName="���������ֵ�",cost=5,hp=10,attackArea=4,speed=1,attackValue=2},
            new UnitInfo(){ id=7,unitName="�b����Ⱥ",cost=7,hp=10,attackValue=8,attackArea=2,speed=4},
            new UnitInfo(){ id=8,unitName="����",cost=6,hp=10,attackValue=7,attackArea=3,speed=2},
            new UnitInfo(){ id=9,unitName="��������",cost=4,attackArea=1.5f,speed=1,attackValue=1,canCreateAnywhere=true},
            new UnitInfo(){ id=10,unitName="�����g",cost=4,attackArea=2f,attackValue=3,speed=18,canCreateAnywhere=true},
            new UnitInfo(){ id=11,unitName="С���t",cost=0,hp=2,attackArea=1.5f,speed=1,attackValue=1},
            new UnitInfo(){ id=12,unitName="�ί���h",cost=0,attackArea=2f,attackValue=-2,speed=18}
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (energyValue < 10) 
        {
            energyValue += Time.deltaTime;
            UIManager.Instance.SetEnergySliderValue();
        }
        DecreaseTime();
    }

    private void DecreaseTime()
    {
        leftTime -= Time.deltaTime;
        int min = (int)leftTime / 60;
        int sec = (int)leftTime % 60;
        UIManager.Instance.SetTimeValue(min, sec);
    }
    /// <summary>
    /// �Д࿨���Ƿ����}ˮ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool CanUseCard(int id)
    {
        return unitInfos[id-1].cost <= energyValue;
    }

    public void DecreaseEnergyValue(int id) 
    {
        int value = unitInfos[id-1].cost;
        energyValue -= value;
    }
    /// <summary>
    /// ���������λ
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pos">����λ��</param>
    /// <param name="isOrange">�Ƿ��춳�ɫ��</param>

    public void CreateUnit(int id, Vector3 pos, bool isOrange = true) 
    {
        GameObject go = Instantiate(unitGos[id - 1]);
        go.transform.position = pos;
        switch (id) 
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 8:
            case 11:
                Unit unit = go.GetComponent<Unit>();
                unit.isOrange = isOrange;
                unit.unitInfo = unitInfos[id - 1];
                break;
            case 6:
                for (int i = 0; i < go.transform.childCount ; i++)
                {
                    Unit u = go.transform.GetChild(i).GetComponent<Unit>();
                    u.isOrange = isOrange;
                    u.unitInfo = unitInfos[id - 1];
                }
                break;
            case 7:
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    Unit u = go.transform.GetChild(i).GetComponent<Unit>();
                    u.isOrange = isOrange;
                    u.unitInfo = unitInfos[id - 1];
                }
                break;
            default:
                break;

        }
    }

    /// <summary>
    /// ĳ����λ�@ȡĬ�J����Ŀ�˵ķ���
    /// </summary>
    /// <param name="unit">��ǰ��λ</param>
    /// <param name="isOrange">�Ƿ��춳�ɫ��</param>
    public void UnitGetTargetPos(Unit unit, bool isOrange) 
    {
        Building[] building = isOrange ? PurpleBuilding : OrangeBuilding ;
        //Building[] building;
        //if (isOrange)
        //{
        //    building = OrangeBuilding;
        //}
        //else 
        //{
        //    building = PurpleBuilding;
        //}
        if (!building[0]) 
        {
            //��������
            return;
        }
        //�����]������, ������ǰ��λx�����c���������˱��^,�Д�����·߀����·
        int index = unit.transform.position.x <= building[0].transform.position.x ? 1 : 2;
        //��ǰ�����������B�Ƿ�������
        if (building[index].isDead)
        {
            //�����������B���, ��ǰ��λ�ч������O�����A�O������
            unit.defaultTarget = building[0];
        }
        else 
        {
            //�����������B�]�����, ��ǰ��λ����·����������·�������O�����A�O������
            unit.defaultTarget = building[index];
        }
    }

}
