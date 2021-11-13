using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTicTac : MonoBehaviour
{
    private GameObject circle;
    private GameObject cross;
    private GameScene gameScene;
    private string _playerMark = "0";
    public string PlayerMark
    {
        get { return _playerMark; }
        set 
        {
            _playerMark = value;
            UpdateButton();
        }
    }
    public int PositionInBoard;
    void Start()
    {
        circle = transform.Find("Circle").gameObject;
        cross = transform.Find("Cross").gameObject;
        gameScene = GameObject.Find("Canvas").GetComponent<GameScene>();
        circle.SetActive(false);
        cross.SetActive(false);
    }

    public void OnClickEvent()
    {
        if (gameScene.IsPlayerTurn() && !gameScene.IsTheGameFinished())
        {
            if (PlayerMark == "0")
            {
                PlayerMark = gameScene.GetPlayerMark();
                UpdateBoard();
                NetworkedClient.SendMessageToHost(ServerClientSignifiers.Board + "," + gameScene.GetBoard());
            }
        }
    }

    private void UpdateBoard()
    {
        string[] board = gameScene.GetBoard().Split(' ');
        board[PositionInBoard] = PlayerMark;
        gameScene.SetBoard(string.Join(" ", board));
    }
    private void UpdateButton()
    {
        if(PlayerMark == "1")
        {
            circle.SetActive(true);
        }
        else if(PlayerMark == "2")
        {
            cross.SetActive(true);
        }
    }

    public void Blank()
    {
        circle.SetActive(false);
        cross.SetActive(false);
    }
}
