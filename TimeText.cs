using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    [SerializeField]
    Text text = null;
    float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Stringify();
        if (time >= 20f)
        {
            InterGame.onLoadCommand = InterGame.GameDataCommand.Advance;
            SceneController.LoadScene("TransitionScene", 1f);
        }
    }

    void Stringify()
    {
        int minutes = (int)(time / 60);
        if (minutes < 10)
        {
            text.text = "0" + minutes;
        }
        else
        {
            text.text = minutes.ToString();
        }
        int seconds = (int)time % 60;
        text.text += ":";
        if (seconds < 10)
        {
            text.text += "0";
        }
        text.text += seconds;
        
    }
}
