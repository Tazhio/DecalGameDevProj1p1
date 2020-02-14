using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The player to follow")]
    private Transform m_PlayerTransform;



    [SerializeField]
    [Tooltip("The offset from player's origin to camera")]
    private Vector3 m_Offset;

    [SerializeField]

    [Tooltip("How fast the player can rotate the camera")]
    private float m_RotationSpeed = 10;


    #endregion


    #region Main Updates

    private void LateUpdate()
    {
        Vector3 newPos = m_PlayerTransform.position + m_Offset;

        transform.position = Vector3.Slerp(transform.position,newPos,1);

        float rotationAmout = m_RotationSpeed * Input.GetAxis("Mouse X");
        transform.RotateAround(m_PlayerTransform.position, Vector3.up, rotationAmout);

        m_Offset = transform.position - m_PlayerTransform.position;

    }


    #endregion
}
