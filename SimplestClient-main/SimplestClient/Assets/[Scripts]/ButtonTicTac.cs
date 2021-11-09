using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTicTac : MonoBehaviour
{
    private GameObject circle;
    private GameObject cross;
    void Start()
    {
        circle = transform.Find("Circle").gameObject;
        cross = transform.Find("Cross").gameObject;
        circle.SetActive(false);
        cross.SetActive(false);
    }

    public void OnClickEvent()
    {
        if(NetworkedClient.IsPlayer1())
        {
            if(!circle.activeSelf)
            {
                circle.SetActive(true);
            }
        }
        else
        {
            if (!cross.activeSelf)
            {
                cross.SetActive(true);
            }
        }
    }
}
