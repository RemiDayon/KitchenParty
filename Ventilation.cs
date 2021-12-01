using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilation : MonoBehaviour
{
    [SerializeField]
    Transform mTransform = null;

    // Update is called once per frame
    void Update()
    {
        if (PauseButton.mainButton.Run)
        {
            mTransform.localEulerAngles += new Vector3(0f, 0f, 2000f * Time.deltaTime);
        }
    }
}
