using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    private GameObject TicTacToe;
    private GameObject InQueueText;
    private GameObject TicTacToeButtonPrefab;
    private GameObject TurnText;
    private GameObject VsText;
    private GameObject[] TicTacToeButtons;
    private string _board = "0 0 0 0 0 0 0 0 0";

    private bool _inGame = false;

    
    void Start()
    {
        InQueueText = GameObject.Find("Canvas/InQueueText");
        TicTacToe = GameObject.Find("Canvas/TicTacToe");
        TurnText = TicTacToe.transform.Find("Who's Turn").gameObject;
        VsText = TicTacToe.transform.Find("PlayingVS").gameObject;
        TicTacToeButtons = new GameObject[9];
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
                VsText.GetComponent<TMPro.TextMeshProUGUI>().text = NetworkedClient.Player2();
            }
        }
        else
        {
            if(!NetworkedClient.InGame())
            {
                SceneManager.LoadScene(1); //main menu
            }
        }
        if (NetworkedClient.IsPlayerTurn())
        {
            TurnText.GetComponent<TMPro.TextMeshProUGUI>().text = "Is Your Turn!";
        }
        else
        {
            TurnText.GetComponent<TMPro.TextMeshProUGUI>().text = "Is Opponent turn!";
        }

        UpdateBoard();
    }
    private void CreateTicTacToeGrid()
    {
        TicTacToeButtonPrefab = Resources.Load<GameObject>("Prefabs/Button");
        Transform gridTransfrom = transform.Find("TicTacToe/Grid").transform;
        for(int i = 0; i < 9; i++)
        {
            TicTacToeButtons[i] = Instantiate(TicTacToeButtonPrefab, gridTransfrom);
            TicTacToeButtons[i].GetComponent<ButtonTicTac>().PositionInBoard = i;
        }
    }

    private void UpdateBoard()
    {
        if(_board != NetworkedClient.GetBoard())
        {
            _board = NetworkedClient.GetBoard();
            string[] board = _board.Split(' ');
            for (int i = 0; i < 9; i++)
            {
                TicTacToeButtons[i].GetComponent<ButtonTicTac>().PlayerMark = board[i];
            }
        }
    }


    

}
