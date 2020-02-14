using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("max health for enemy")]
    private float m_MaxHealth;

    [SerializeField]
    [Tooltip("The speed of the enemy")]
    private float m_Speed;


    [SerializeField]
    [Tooltip("approximate damage per frame")]
    private float m_Damage;


    [SerializeField]
    [Tooltip("the explosion that occurs when a enemy dies")]
    private ParticleSystem m_DeathExplosion;



    [SerializeField]
    [Tooltip("the probability that that this enemy drops a health pill")]
    private float m_HealthPillDropRate;



    [SerializeField]
    [Tooltip("the type of health pill this enemy may drop")]
    private GameObject m_HealthPill;

    [SerializeField]
    [Tooltip("how many killing this enemi provides")]
    private int m_Score;


    #endregion
    //[SerializeField]
    //[Tooltip("current health for enemy")]
    private float p_CurHealth;



    



    #region Cached Components
    private Rigidbody cc_Rb;

    #endregion


    #region Cached References

    private Transform cr_Player;

    #endregion

    #region Initialization

    private void Awake()
    {

        p_CurHealth = m_MaxHealth;
        cc_Rb = GetComponent < Rigidbody > ();
   
    }

    private void Start()


    {

        //feeling like used the factory pattern
        cr_Player = FindObjectOfType<PlayerController>().transform;

    }

    #endregion

    #region Main Updates

    private void FixedUpdate()
    {
        Vector3 dir = cr_Player.position - transform.position;
        dir.Normalize();
        cc_Rb.MovePosition(cc_Rb.position + dir * m_Speed*Time.fixedDeltaTime);

    }


    #endregion


    #region Collision Methods

    private void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.collider.gameObject;

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DecreaseHealth(m_Damage);
            //DecreaseHealth(m_Damage);
            //this can make the player invicinble

        }

    }



    #endregion


    #region Health Method

    public void DecreaseHealth(float amount)
    {

        p_CurHealth -= amount;
        if (p_CurHealth <= 0)
        {
            ScoreManager.singleton.IncreaseScores(m_Score);
            if (Random.value < m_HealthPillDropRate)
            {
                
                Instantiate(m_HealthPill, transform.position, Quaternion.identity);
            }
            Instantiate(m_DeathExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
       
        
    }

    #endregion
}
