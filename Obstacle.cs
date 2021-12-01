using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] Renderer m_spriteRenderer = null;
    [SerializeField] ParticleSystem m_pSystem = null;
    [SerializeField] Renderer m_pSRenderer = null;
    [SerializeField] static float m_initDistance = 10000f;
    [SerializeField] float m_fova = 1000f;
    Vector2 m_position = new Vector2(0f, 0f);
    Transform m_spriteData = null;
    Character m_character = null;
    float m_distance = m_initDistance;
    float m_groundDistance = 0f;
    bool m_isDangerous = false;
    int m_type = 0;

    public float GroundDistance { get => m_groundDistance; set => m_groundDistance = value; }
    public int Type { get => m_type; set => m_type = value; }
    public bool IsDangerous { get => m_isDangerous; set => m_isDangerous = value; }

    // Start is called before the first frame update
    void Start()
    {
        m_character = Character.main;
        m_spriteData = GetComponent<Transform>();
     
        if (m_type >= 2 && m_type < 5)
        {
            m_position.x = (m_type - 3) * 2f;
        }

        float ratio = 1 + m_distance / m_fova;
        m_groundDistance = -4f / ratio - 1f;
        m_spriteData.localScale = new Vector3(1f / ratio, 1f / ratio, 1f);
        m_spriteData.position = new Vector3(m_position.x / ratio, m_groundDistance + m_position.y / ratio, m_distance / m_initDistance);
        m_pSystem.Play(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseButton.mainButton.Run)
        {
            float speed = m_character.Speed;
            float ratio = 1 + m_distance / m_fova;
            m_distance -= speed * Time.deltaTime;
            m_groundDistance = -4f / ratio - 1f;
            if (m_distance > 0f)
            {

                m_spriteData.localScale = new Vector3(1f / ratio, 1f / ratio, 1f);
                m_spriteData.position = new Vector3(m_position.x / ratio, m_groundDistance + m_position.y / ratio, m_distance / m_initDistance);

                if (m_groundDistance <= -4.5f)
                {
                    m_pSRenderer.sortingLayerName = "Behind";
                    m_spriteRenderer.sortingLayerName = "Behind";
                    m_pSystem.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, (5f + m_groundDistance) * 2f - 1f));
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
