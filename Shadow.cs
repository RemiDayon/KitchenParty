using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    Transform m_transform = null;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FireMouth")
        {
            other.GetComponentInParent<Obstacle>().IsDangerous = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "FireMouth")
        {
            other.GetComponentInParent<Obstacle>().IsDangerous = false;
        }
    }
}
