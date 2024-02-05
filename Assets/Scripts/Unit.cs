using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
/// <summary>
/// Unit Information
/// </summary>
public struct UnitInfo 
{
    public int id;
    public string unitName;
    public int cost;
    public int hp;
    public float attackArea;
    public float speed;
    public int attackValue;
    public bool canCreateAnywhere;
}

