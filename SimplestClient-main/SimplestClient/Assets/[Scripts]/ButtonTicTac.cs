using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTicTac : MonoBehaviour
{
    private GameObject circle;
    private GameObject cross;
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
        circle.SetActive(false);
        cross.SetActive(false);
    }

    public void OnClickEvent()
    {
        if(NetworkedClient.IsPlayer1())
        {
            PlayerMark = "1";
        }
        else
        {
            PlayerMark = "2";
        }
        UpdateBoard();
        NetworkedClient.SendMessageToHost(ServerClientSignifiers.Board + "," + NetworkedClient.GetBoard());
    }

    private void UpdateBoard()
    {
        string[] board = NetworkedClient.GetBoard().Split(' ');
        board[PositionInBoard] = PlayerMark;
        NetworkedClient.SetBoard(string.Join(" ", board));
    }
    private void UpdateButton()
    {
        if(PlayerMark == "1")
        {
            circle.SetActive(true);
            cross.SetActive(false);
        }
        else if(PlayerMark == "2")
        {
            circle.SetActive(false);
            cross.SetActive(true);
        }
    }
}
