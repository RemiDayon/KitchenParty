using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    [SerializeField] GameObject vFencePrefab = null;
    [SerializeField] GameObject hulFencePrefab = null;
    [SerializeField] GameObject hurFencePrefab = null;
    [SerializeField] GameObject hdlFencePrefab = null;
    [SerializeField] GameObject hdrFencePrefab = null;
    [SerializeField] GameObject roofGrid = null;
    [SerializeField] GameObject sideLight = null;
    [SerializeField] float nextObstacle = 0f;
    [SerializeField] float nextRoofGrid = 0f;
    [SerializeField] float nextSideLight = 0f;
    float obstacleDistance = 0f;
    float roofDistance = 0f;
    float sideDistance = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseButton.mainButton.Run)
        {
            float speed = Character.main.Speed;
            obstacleDistance += speed * Time.deltaTime;
            roofDistance += speed * Time.deltaTime;
            sideDistance += speed * Time.deltaTime;

            if (obstacleDistance >= nextObstacle)
            {
                int fenceNb = Random.Range(1, 3);
                int type = -1;
                for (int x = 0; x < fenceNb; ++x)
                { 
                    type = CreateFence(type);
                }
                obstacleDistance -= nextObstacle;
            }

            if (roofDistance >= nextRoofGrid)
            {
                Instantiate(roofGrid);
                roofDistance -= nextRoofGrid;
            }

            if (sideDistance >= nextSideLight)
            {
                Instantiate(sideLight);
                sideDistance -= nextSideLight;
            }
        }
    }

    int CreateFence(int old)
    {
        switch(old)
        {
            case 0:
            case 1:
            case 5:
            case 6:
                {
                    int type = Random.Range(2, 5);
                    Instantiate(vFencePrefab).GetComponent<Obstacle>().Type = type;
                    return type;
                }

            default:
                {
                    int type = Random.Range(0, 7);
                    type = (type == old) ? type = (type + 1) % 5 : type;
                    switch (type)
                    {
                        case 0:
                            Instantiate(hurFencePrefab).GetComponent<Obstacle>().Type = type;
                            break;

                        case 1:
                            Instantiate(hdrFencePrefab).GetComponent<Obstacle>().Type = type;
                            break;

                        case 5:
                            Instantiate(hulFencePrefab).GetComponent<Obstacle>().Type = type;
                            break;

                        case 6:
                            Instantiate(hdlFencePrefab).GetComponent<Obstacle>().Type = type;
                            break;

                        default:
                            Instantiate(vFencePrefab).GetComponent<Obstacle>().Type = type;
                            break;
                    }
                    return type;
                }
        }
    }
}
