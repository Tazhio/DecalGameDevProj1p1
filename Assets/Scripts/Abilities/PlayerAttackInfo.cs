using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerAttackInfo
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("a name for the attack")]
    private string m_Name;

    public string AttackName
    {
        get
        {
        return m_Name;
        }
    }


    [SerializeField]
    [Tooltip("The button that pressed that will use this attck, This button must be in input settings")]
    private string m_Button;

    public string Button
    {
        get
        {

            return m_Button;
        }
    }



    [SerializeField]
    [Tooltip("trigger string used to activate this attack in animator")]
    private string m_TriggerName;

    public string TriggerName
    {
        get
        {


            return m_TriggerName;

        }
    }



    [SerializeField]
    [Tooltip("The prefab that represent the game ability")]
    private GameObject m_AbilityGo;

    public GameObject AbilityGo
    {
        get
        {

            return m_AbilityGo;
        }
    }



    [SerializeField]
    [Tooltip("Where to spawn the ability game object respect to the player")]
    private Vector3 m_offset;
    public  Vector3 Offset
    {
        get
        {
            return m_offset;

        }
    }




    [SerializeField]
    [Tooltip("How long it should take before the attack it's activated after the button is pressed")]
    //攻击前摇
    private float m_WindUpTime;

    public float WindUpTime
    {
        get
        {
            return m_WindUpTime;

        }
    }


    [SerializeField]
    [Tooltip("How long it should take before the player can do anything")]
    //攻击后的僵直时间
    private float m_FronzenTime;

    public float FronzenTime
    {
        get
        {
            return m_FronzenTime;

        }
    }


    [SerializeField]
    [Tooltip("How long the player has to wait before the ability can be used again")]
    //攻击后的僵直时间
    private float m_CoolDown;



    [SerializeField]
    [Tooltip("The amount of health this ability cost")]
  
    private float m_HealthCost;

    public float HealthCost
    {
        get
        {
            return m_HealthCost;

        }
    }



    [SerializeField]
    [Tooltip("The color to change when use this ability")]
    
    private Color m_Color;

    public Color AbilityColor
    {
        get
        {

            return m_Color;
           
        }
    }





    #endregion

    #region Public Variables

    public float CoolDown
    {
        get;

        set;
    }

    #endregion


    #region CoolDown Methods
    public void ResetCoolDown()
    {
        CoolDown = m_CoolDown;
        
    }

    public bool IsReady()
    {


        return CoolDown >= 0;

    }


    #endregion

}
