using System.Collections;
using System.Collections.Generic;
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

}
