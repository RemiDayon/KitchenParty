using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character main;

    // if a screen touch is detected this variable store where the touch has begun
    Vector2 m_touchMovement;

    [SerializeField] ParticleSystem m_deathPop = null;
    [SerializeField] ParticleSystem m_death = null;
    [SerializeField] Transform m_shadowTransform = null;
    [SerializeField] Collider2D m_slideBox = null;
    [SerializeField] Collider2D m_runBox = null;
    [SerializeField] bool m_accelerate = true;
    [SerializeField] float m_distance = 0f;
    SpriteRenderer m_renderer = null;
    Transform m_spriteData = null;
    Animator m_animator = null;
    float m_initialJumpSpeed = 10f;
    float m_slideLast = 1f;
    float m_ActionSpeed = 0f;
    float m_fallSpeed = 30f;
    float m_speed = 1000f;
    float m_time = 0f;
    float m_y = -4.5f;
    int m_line = 0;
    bool m_isAlive = true;
    Action m_action = Action.RUN;

    //public Action Action1 { get => m_action; set => m_action = value; }
    public float Speed { get => m_speed; set => m_speed = value; }
    //public Transform SpriteData { get => m_spriteData; set => m_spriteData = value; }

    public enum Action
    {
        RUN,
        JUMP,
        SLIDE,
    };

    // Start is called before the first frame update
    void Start()
    {
        m_speed *= 1 + .1f * MeatlessPartyData.Level;

        main = this;
        m_animator = GetComponent<Animator>();
        m_spriteData = GetComponent<Transform>();
        m_renderer = GetComponent<SpriteRenderer>();

        m_animator.SetInteger("Action", (int)m_action);
        m_slideBox.enabled = false;
    }

    void Update()
    {
        if (m_isAlive && PauseButton.mainButton.Run)
        {
            m_time += Time.deltaTime;
            if (m_accelerate)
            {
                m_speed += 30f * Time.deltaTime;
            }
            m_distance += m_speed * Time.deltaTime;

            handlePlayerInput();
            handleAction();
        }
    }

    void handlePlayerInput()
    {
        // handles player's input
        if (Input.touchCount == 1)
        {
            Touch touch = Input.touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    {
                        m_touchMovement = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                    {
                        m_touchMovement = touch.position - m_touchMovement;

                        // here we check if the movement of the finger is tall enougth to be interpreted as a swap
                        if (m_touchMovement.magnitude >= 40f)
                        {
                            // then we check the angle of the movement to know if it goes up or down
                            float angle = Mathf.Atan2(m_touchMovement.y, m_touchMovement.x);

                            if (angle > Mathf.PI * .25f && angle < Mathf.PI * .75f && m_action != Action.JUMP)
                            {
                                m_action = Action.JUMP;
                                m_animator.SetInteger("Action", (int)m_action);
                                m_ActionSpeed = m_initialJumpSpeed;
                                m_slideBox.enabled = false;
                                m_runBox.enabled = true;
                            }
                            else if (angle < Mathf.PI * -.25f && angle > Mathf.PI * -.75f)
                            {
                                if (m_action == Action.RUN)
                                {
                                    m_action = Action.SLIDE;
                                    m_ActionSpeed = m_slideLast;
                                    m_slideBox.enabled = true;
                                    m_runBox.enabled = false;
                                    m_animator.SetInteger("Action", (int)m_action);
                                }
                                else if (m_action == Action.SLIDE)
                                {
                                    m_action = Action.SLIDE;
                                    m_ActionSpeed = m_slideLast / 2f;
                                    m_slideBox.enabled = true;
                                    m_runBox.enabled = false;
                                    m_animator.ResetTrigger("DoItAgain");
                                    m_animator.SetTrigger("DoItAgain");
                                }
                                else
                                {
                                    m_ActionSpeed -= m_fallSpeed * 100f * Time.deltaTime;
                                }
                            }
                            else if (angle < Mathf.PI * .25f && angle > Mathf.PI * -.25f && m_line < 1)
                            {
                                ++m_line;
                                m_spriteData.position = new Vector2(m_line * 1.5f, m_y);
                                m_shadowTransform.position = new Vector2(m_line * 1.5f, -4.5f);
                            }
                            else if ((angle > Mathf.PI * .75f || angle < Mathf.PI * -.75f) && m_line > -1)
                            {
                                --m_line;
                                m_spriteData.position = new Vector2(m_line * 1.5f, m_y);
                                m_shadowTransform.position = new Vector2(m_line * 1.5f, -4.5f);
                            }
                        }
                    }
                    break;
            }
        }

        //!\ DEBUG ZONE
        if (Input.GetKeyDown(KeyCode.UpArrow) && m_action != Action.JUMP)
        {
            m_action = Action.JUMP;
            m_ActionSpeed = m_initialJumpSpeed;
            m_animator.SetInteger("Action", (int)m_action);
            m_slideBox.enabled = false;
            m_runBox.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (m_action == Action.RUN)
            {
                m_action = Action.SLIDE;
                m_ActionSpeed = m_slideLast;
                m_slideBox.enabled = true;
                m_runBox.enabled = false;

                m_animator.SetInteger("Action", (int)m_action);
            }
            else if(m_action == Action.SLIDE)
            {
                m_action = Action.SLIDE;
                m_ActionSpeed = m_slideLast / 2f;
                m_slideBox.enabled = true;
                m_runBox.enabled = false;

                m_animator.ResetTrigger("DoItAgain");
                m_animator.SetTrigger("DoItAgain");
            }
            else
            {
                m_ActionSpeed -= m_fallSpeed * 100f * Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && m_line < 1)
        {
            ++m_line;
            m_spriteData.position = new Vector2(m_line * 1.5f, m_y);
            m_shadowTransform.position = new Vector2(m_line * 1.5f, -4.5f);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && m_line > -1)
        {
            --m_line;
            m_spriteData.position = new Vector2(m_line * 1.5f, m_y);
            m_shadowTransform.position = new Vector2(m_line * 1.5f, -4.5f);
        }
        //!\ DEBUG ZONE
    }

    void handleAction()
    {
        switch(m_action)
        {
            case Action.JUMP:
                {
                    m_y += m_ActionSpeed * Time.deltaTime;
                    m_ActionSpeed -= m_fallSpeed * Time.deltaTime;

                    if (m_y <= -4.5f)
                    {
                        m_y = -4.5f;
                        m_action = Action.RUN;
                        m_animator.SetInteger("Action", (int)m_action);
                    }

                    m_spriteData.position = new Vector2(m_line * 1.5f, m_y);
                }
                break;

            case Action.SLIDE:
                {
                    m_ActionSpeed -= Time.deltaTime;

                    if (m_ActionSpeed <= 0f)
                    {
                        m_action = Action.RUN;
                        m_animator.SetInteger("Action", (int)m_action);
                        m_slideBox.enabled = false;
                        m_runBox.enabled = true;
                    }
                }
                break;

            case Action.RUN:
                {

                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.tag == "Obstacle" && collision.GetComponentInParent<Obstacle>().IsDangerous && m_isAlive)
        {
            m_isAlive = false;
            m_speed = 0f;
            m_renderer.enabled = false;
            m_death.transform.position = new Vector2(m_line * 1.5f, m_y + 2f);
            m_death.Play();
            m_deathPop.transform.position = new Vector2(m_line * 1.5f, m_y + 2f);
            m_deathPop.Play();
            InterGame.onLoadCommand = InterGame.GameDataCommand.LoseLife;
            SceneController.LoadScene("TransitionScene", 1f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle" && collision.GetComponentInParent<Obstacle>().IsDangerous && m_isAlive)
        {
            m_isAlive = false;
            m_speed = 0f;
            m_renderer.enabled = false;
            m_death.transform.position = new Vector2(m_line * 1.5f, m_y + 2f);
            m_death.Play();
            m_deathPop.transform.position = new Vector2(m_line * 1.5f, m_y + 2f);
            m_deathPop.Play();
            InterGame.onLoadCommand = InterGame.GameDataCommand.LoseLife;
            SceneController.LoadScene("TransitionScene", 1f);
        }
    }
}
