using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public static PauseButton mainButton;
    bool run = true;
    public bool Run { get => run; set => run = value; }
    bool stateChange = false;
    public bool StateChange { get => stateChange; set => stateChange = value; }


    [SerializeField]
    Sprite pauseSprite = null;
    [SerializeField]
    Sprite runSprite = null;
    [SerializeField]
    SpriteRenderer mRenderer = null;
    Camera mainCam;
    Bounds bounds;

    // Start is called before the first frame update
    void Start()
    {
        mainButton = this;
        mainCam = Camera.main;
        bounds = GetComponent<Collider>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        StateChange = false;
        if (Input.touchCount == 1)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Ended)
            {
                Vector3 touchPos = mainCam.ScreenToWorldPoint(touch.position);
                touchPos.z = 0f;

                if (bounds.Contains(touchPos))
                {
                    StateChange = true;
                    run = !run;
                    if (run)
                    {
                        mRenderer.sprite = runSprite;
                    }
                    else
                    {
                        mRenderer.sprite = pauseSprite;
                    }
                }
            }
        }
    }
}
