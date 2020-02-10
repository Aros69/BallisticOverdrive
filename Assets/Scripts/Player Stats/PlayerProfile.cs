using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile
{
    struct RedProfile
    {
        const float m_speed = 5.0f;
        const float m_lookSensitivity = 3.0f;
        const float m_jumpforce = 500;
        const float m_airDrag = 1.001f;
        const float m_groundDrag = 1.2f;
        const float m_airControl = 0.05f;
        const float m_bonusGravity = 2.0f;
    }
    struct BlueProfile
    {
        const float m_speed = 5.0f;
        const float m_lookSensitivity = 3.0f;
        const float m_jumpforce = 500;
        const float m_airDrag = 1.001f;
        const float m_groundDrag = 1.2f;
        const float m_airControl = 0.05f;
        const float m_bonusGravity = 2.0f;
    }
    public void loadRedProfile(GameObject player)
    {

    }
    public void loadBlueProfile(GameObject player)
    {

    }
}
