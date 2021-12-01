using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    [SerializeField]
    Sprite on = null;
    [SerializeField]
    Sprite off = null;
    [SerializeField]
    SpriteRenderer mRenderer = null;
    
    // 650; 1640
    // Update is called once per frame
    void Update()
    {
        if (PauseButton.mainButton.StateChange)
        {
            if (PauseButton.mainButton.Run)
            {
                mRenderer.sprite = on;
            }
            else
            {
                mRenderer.sprite = off;
            }
        }
    }
}
