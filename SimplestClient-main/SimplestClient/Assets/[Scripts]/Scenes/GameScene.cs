using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    private GameObject TicTacToe;
    private GameObject InQueueText;
    private GameObject TicTacToeButtonPrefab;

    private bool _inGame = false;

    
    void Start()
    {
        InQueueText = GameObject.Find("Canvas/InQueueText");
        TicTacToe = GameObject.Find("Canvas/TicTacToe");
        TicTacToe.SetActive(false);
        CreateTicTacToeGrid();
    }

    private void FixedUpdate()
    {
        if(!_inGame)
        {
            if(NetworkedClient.InGame())
            {
                _inGame = true;
                TicTacToe.SetActive(true);
                InQueueText.SetActive(false);
            }
        }
    }
    private void CreateTicTacToeGrid()
    {
        TicTacToeButtonPrefab = Resources.Load<GameObject>("Prefabs/Button");
        Transform gridTransfrom = transform.Find("TicTacToe/Grid").transform;
        for(int i = 0; i < 9; i++)
        {
            Instantiate(TicTacToeButtonPrefab, gridTransfrom);
        }
    }

    

}
