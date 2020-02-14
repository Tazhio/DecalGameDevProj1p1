using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemySpawnInfo
{

    #region Editor Variables
   

    [SerializeField]
    [Tooltip("name of the enemy")]
    private string m_Name;

    public string EnemyName
    {
        get
        {

            return m_Name;

        }
    }


    [SerializeField]
    [Tooltip("the prefab this enemy will spawn")]
    private GameObject m_EnemyGO;

    public GameObject EnemyGO
    {

        get
        {

            return m_EnemyGO;

        }
    }



    [SerializeField]
    [Tooltip("number of seconds before next enemy is spawn")]
    private float m_TimeToNextSpawn;

    public float TimeToNextSpawn
    {

        get
        {
            return m_TimeToNextSpawn;

        }
    }



    [SerializeField]
    [Tooltip("the number of enemies to spawn, if set to 0, it will spawn endlessly")]
    private int m_NumberToSpawn;

    public int NumberToSpawn
    {

        get
        {

            return m_NumberToSpawn;

        }
    }

    #endregion

}
