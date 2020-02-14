using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{


    #region Editor Variabkes
    [SerializeField]
    [Tooltip("How fast the player should move when running around")]
    private float m_speed;

    [SerializeField]
    [Tooltip("The transform of the camera attached to the player")]
    private Transform m_CameraTransform;


    [SerializeField]
    [Tooltip("a list of all attacks and information about them")]
    private PlayerAttackInfo[] m_Attacks;


    [SerializeField]
    [Tooltip("The max health a player can has")]
    private float m_MaxHealth;


    [SerializeField]
    [Tooltip("The hud controller")]
    private HUDController m_HUD;


    


    #endregion

    #region Cached References

    private Animator cr_Anim;
    private Renderer cr_Renderer;




    #endregion
    #region Cached Components

    private Rigidbody cc_Rb;

    #endregion


    #region Private Variables
    //the current move of the player, does not include magnitude
    private Vector2 p_Velocity;

    private float p_FronzenTimer;


    //the default color of the player, cached so we can switch between
    private Color p_DefaultColor;


    //the current health that the player has
    private float p_CurHealth;






    #endregion


    #region Initialization

    private void Awake()
    {
        p_Velocity = Vector2.zero;
        cc_Rb = GetComponent<Rigidbody>();
        cr_Anim = GetComponent<Animator>();
        cr_Renderer = GetComponentInChildren<Renderer>();
        p_DefaultColor = cr_Renderer.material.color;
        p_CurHealth = m_MaxHealth;




        p_FronzenTimer = 0;
        for (int i = 0; i < m_Attacks.Length; i++)
        {

            PlayerAttackInfo attack = m_Attacks[i];
            attack.CoolDown = 0;
            if (attack.WindUpTime>attack.FronzenTime)
            {
                Debug.LogError(attack.AttackName + "has a wind up time that is larger than the fronzen time");

            }
        }

     
        
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

    }

    #endregion


    #region Main Updates

    private void Update()
    {

        if (p_FronzenTimer > 0)
        {
            p_Velocity = Vector2.zero;
            p_FronzenTimer -= Time.deltaTime;
            return;


        }
        else
        {
            p_FronzenTimer = 0;
        }

        // for ability use
        for (int i = 0; i < m_Attacks.Length; i++)
        {
            PlayerAttackInfo attack = m_Attacks[i];
            if (attack.IsReady())
            {
                if (Input.GetButtonDown(attack.Button))
                {
                    p_FronzenTimer = attack.FronzenTime;
                    DecreaseHealth(attack.HealthCost);

                    StartCoroutine(UseAttack(attack));
                    break;


                }

            } else if(attack.CoolDown>0){
                attack.CoolDown -= Time.deltaTime;

            }


        }


        

        float forward=Input.GetAxis("Vertical");
        float right = Input.GetAxis("Horizontal");


        //updating animation
        cr_Anim.SetFloat("Speed", Mathf.Clamp01(Mathf.Abs(forward) + Mathf.Abs(right)));



        //Updating velocity
        float moveThreadshold = 0.3f;


        if (forward > 0 && forward < moveThreadshold)
        {
            forward = 0;
        }else if(forward<0 && forward > -moveThreadshold)
        {
            forward = 0;
        }
        if(right>0 && right < moveThreadshold)
        {

            right = 0;

        }
        if(right<0 && right > -moveThreadshold)
        {

            right = 0;
        }

        p_Velocity.Set(right, forward);




    }


    private void FixedUpdate()
    {
        //Update the postition of the player
        cc_Rb.MovePosition(cc_Rb.position + m_speed * Time.fixedDeltaTime*transform.forward*p_Velocity.magnitude);


        // update the rotation of the player
        cc_Rb.angularVelocity = Vector3.zero;

        if (p_Velocity.sqrMagnitude > 0)
        {
            float angleToRotCam = Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.up, p_Velocity);
            Vector3 camForward = m_CameraTransform.forward;
            Vector3 newRot = new Vector3(Mathf.Cos(angleToRotCam) * camForward.x - Mathf.Sin(angleToRotCam) * camForward.z, 0, Mathf.Cos(angleToRotCam) * camForward.z + Mathf.Sin(angleToRotCam) * camForward.x);

            float theta = Vector3.SignedAngle(transform.forward, newRot, Vector3.up);
            cc_Rb.rotation = Quaternion.Slerp(cc_Rb.rotation, cc_Rb.rotation * Quaternion.Euler(0, theta, 0), 0.2f);


        }
    }




    #endregion


    #region Health/Dying Methods

//really like the reflection

    public void DecreaseHealth(float amount)
    {



        p_CurHealth -= amount;
        m_HUD.UpdateHealth(1.0f * p_CurHealth / m_MaxHealth);
        if (p_CurHealth <= 0)
        {
            SceneManager.LoadScene("MainMenu");

        }
        
      

    }


    public void IncreaseHealth(int amount)
    {

        p_CurHealth += amount;
        if (p_CurHealth >= m_MaxHealth)
        {
            p_CurHealth = m_MaxHealth;

        }

        m_HUD.UpdateHealth(1.0f * p_CurHealth / m_MaxHealth);

    }


    #endregion

    #region Attack Method

    private IEnumerator UseAttack(PlayerAttackInfo attack)
    {
        cc_Rb.rotation = Quaternion.Euler(0, m_CameraTransform.eulerAngles.y, 0);
        cr_Anim.SetTrigger(attack.TriggerName);
        IEnumerator toColor = ChangeColor(attack.AbilityColor, 10);
        StartCoroutine(toColor);


        yield return new WaitForSeconds(attack.WindUpTime);


        Vector3 offset = transform.forward * attack.Offset.z + transform.right * attack.Offset.x + transform.up * attack.Offset.y;
        GameObject go = Instantiate(attack.AbilityGo, transform.position + offset,cc_Rb.rotation);
        go.GetComponent<Ability>().Use(transform.position + offset);


        StopCoroutine(toColor);
        StartCoroutine(ChangeColor(p_DefaultColor, 50));


        yield return new WaitForSeconds(attack.CoolDown);

        attack.ResetCoolDown();




    }


    #endregion



    #region Misc Methods

    private IEnumerator ChangeColor(Color newColor,float speed)
    {
        Color curColor = cr_Renderer.material.color;
        while (curColor != newColor)
        {
            curColor = Color.Lerp(curColor, newColor, speed / 100);
            cr_Renderer.material.color = curColor;
            yield return null;

        }



    }





    #endregion


    #region Collision Methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPill"))
        {
            IncreaseHealth(other.GetComponent<HealthPill>().HealthGain);
            Destroy(other.gameObject);
        }
    }



    #endregion
}



