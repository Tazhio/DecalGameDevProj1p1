using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPill : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The amount of health that pill restores")]
      
    private int m_HealthGain;

    public int HealthGain
    {

        get
        {


            return m_HealthGain;
        }
    }




    #endregion
}
