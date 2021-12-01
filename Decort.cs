using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decort : MonoBehaviour
{
    [SerializeField] static float m_initDistance = 10000f;
    [SerializeField] float m_fova = 1000f;
    [SerializeField] float m_from = 0f;
    [SerializeField] float m_where = 0f;
    [SerializeField] float m_drawLimit = 0f;
    Transform m_spriteData = null;
    Character m_character = null;
    float m_distance = m_initDistance;
    float m_groundDistance = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_character = Character.main;
        m_spriteData = GetComponent<Transform>();

        float ratio = 1 + m_distance / m_fova;
        m_groundDistance = m_where / ratio - m_from;
        m_spriteData.localScale = new Vector3(1f / ratio, 1f / ratio, 1f);
        m_spriteData.position = new Vector3(0f, m_groundDistance, m_distance / m_initDistance);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseButton.mainButton.Run)
        {
            float speed = m_character.Speed;
            float ratio = 1 + m_distance / m_fova;
            m_distance -= speed * Time.deltaTime;
            m_groundDistance = m_where / ratio - m_from;
            if (m_groundDistance <= m_drawLimit)
            {
                m_spriteData.localScale = new Vector3(1f / ratio, 1f / ratio, 1f);
                m_spriteData.position = new Vector3(0f, m_groundDistance, m_distance / m_initDistance);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
