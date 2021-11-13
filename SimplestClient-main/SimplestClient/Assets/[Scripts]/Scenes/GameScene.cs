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
    private GameObject Restart;
    private GameObject VsText;
    private GameObject[] TicTacToeButtons;
    private string _board = "0 0 0 0 0 0 0 0 0";

    private bool _inGame = false;
    private bool _gameFinished = false;
    private string _playerMark = "";
    private bool _playerTurn;

    
    void Start()
    {
        InQueueText = GameObject.Find("Canvas/InQueueText");
        TicTacToe = GameObject.Find("Canvas/TicTacToe");
        Restart = GameObject.Find("Canvas/Restart");
        TurnText = TicTacToe.transform.Find("Who's Turn").gameObject;
        VsText = TicTacToe.transform.Find("PlayingVS").gameObject;
        TicTacToeButtons = new GameObject[9];
        TicTacToe.SetActive(false);
        Restart.SetActive(false);
        CreateTicTacToeGrid();

        NetworkedClient.OnFindingMatch += Event_OnFindingMatch;
        NetworkedClient.OnBoardChanged += Event_OnBoardChanged;
        NetworkedClient.OnPlayerTurnChanged += Event_OnPlayerTurnChanged;
        NetworkedClient.OnPlayerWin += Event_OnPlayerWin;
        NetworkedClient.OnRestart += Event_OnRestart;
        
    }
    private void Event_OnFindingMatch(bool found, string playerMark, string player2Name)
    {
        if(found)
        {
            TicTacToe.SetActive(true);
            InQueueText.SetActive(false);
            VsText.GetComponent<TMPro.TextMeshProUGUI>().text = "vs " + player2Name;
            _inGame = true;
            _playerMark = playerMark;

        }
        else
        {
            RestartBoard();
            UnSubscribeEvents();
            SceneManager.LoadScene(1); //main menu
        }
    }
    private void Event_OnPlayerTurnChanged(bool playerTurn)
    {
        _playerTurn = playerTurn;
        UpdatePlayerTurnText();
    }
    private void Event_OnBoardChanged(string newBoard)
    {
        UpdateBoard(newBoard);
    }
    private void Event_OnPlayerWin(bool win)
    {
        Restart.SetActive(true);
        _gameFinished = true;
        if (win)
        {
            TurnText.GetComponent<TMPro.TextMeshProUGUI>().text = "You Win!";
        }
        else
        {
            TurnText.GetComponent<TMPro.TextMeshProUGUI>().text = "You Lose!";
        }
    }
    private void Event_OnRestart()
    {
        RestartBoard();
    }

    private void UnSubscribeEvents()
    {
        NetworkedClient.OnFindingMatch -= Event_OnFindingMatch;
        NetworkedClient.OnBoardChanged -= Event_OnBoardChanged;
        NetworkedClient.OnPlayerTurnChanged -= Event_OnPlayerTurnChanged;
        NetworkedClient.OnPlayerWin -= Event_OnPlayerWin;
        NetworkedClient.OnRestart -= Event_OnRestart;
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

    private void UpdateBoard(string newBoard)
    {
        _board = newBoard;
        string[] board = _board.Split(' ');
        for (int i = 0; i < 9; i++)
        {
            TicTacToeButtons[i].GetComponent<ButtonTicTac>().PlayerMark = board[i];
            if(board[i] == "0")
            {
                TicTacToeButtons[i].GetComponent<ButtonTicTac>().Blank();
            }
        }
       
    }
    private void UpdatePlayerTurnText()
    {
        if (_playerTurn)
        {
            TurnText.GetComponent<TMPro.TextMeshProUGUI>().text = "Is Your Turn!";
        }
        else
        {
            TurnText.GetComponent<TMPro.TextMeshProUGUI>().text = "Is Opponent turn!";
        }
    }

    public void ClickRestartButton()
    {
        NetworkedClient.SendMessageToHost(ServerClientSignifiers.Restart);
    }
    private void RestartBoard()
    {
        _gameFinished = false;
        UpdatePlayerTurnText();
        UpdateBoard("0 0 0 0 0 0 0 0 0");
        Restart.SetActive(false);
    }

    public void ClickBackButton()
    {
        NetworkedClient.SendMessageToHost(ServerClientSignifiers.InGame);
        if(_inGame)
        {
            RestartBoard();
        }
        UnSubscribeEvents();
        SceneManager.LoadScene(1); //main menu
    }

    public bool IsPlayerTurn() { return _playerTurn; }
    public bool IsInGame() { return _inGame; }
    public bool IsTheGameFinished() { return _gameFinished; }
    public string GetPlayerMark() { return _playerMark; }

    public string GetBoard() { return _board; }
    public void SetBoard(string newBoard) { _board = newBoard; }



    

}
