using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameScene : MonoBehaviour
{
    private GameObject TicTacToe;
    private GameObject InQueueText;
    private GameObject TicTacToeButtonPrefab;
    private GameObject TurnText;
    private GameObject Restart;
    private GameObject VsText;
    private GameObject[] TicTacToeButtons;
    private GameObject ChatMessage;
    private GameObject BoardMoveReplay;
    private GameObject ListOfReplays;
    private string _board = "0 0 0 0 0 0 0 0 0";

    private bool _inGame = false;
    private bool _gameFinished = false;
    private string _playerMark = "";
    private bool _playerTurn;
    private int _gameRoomID;
    private bool _isPlayer = false; //this is for the spectators.

    void Start()
    {
        InQueueText = GameObject.Find("Canvas/InQueueText");
        TicTacToe = GameObject.Find("Canvas/TicTacToe");
        Restart = GameObject.Find("Canvas/Restart");
        TurnText = TicTacToe.transform.Find("Who's Turn").gameObject;
        VsText = TicTacToe.transform.Find("PlayingVS").gameObject;
        BoardMoveReplay = Resources.Load<GameObject>("Prefabs/BoardMoveReplay");
        ListOfReplays = transform.Find("ListOfReplays").gameObject;

        ChatMessage = Resources.Load<GameObject>("Prefabs/ChatMessage");
        TicTacToeButtons = new GameObject[9];
        TicTacToe.SetActive(false);
        Restart.SetActive(false);
        ListOfReplays.SetActive(false);
        CreateTicTacToeGrid();

        NetworkedClient.OnFindingMatch += Event_OnFindingMatch;
        NetworkedClient.OnBoardChanged += Event_OnBoardChanged;
        NetworkedClient.OnPlayerTurnChanged += Event_OnPlayerTurnChanged;
        NetworkedClient.OnPlayerWin += Event_OnPlayerWin;
        NetworkedClient.OnRestart += Event_OnRestart;
        NetworkedClient.OnChatMessageReceived += NetworkedClient_OnChatMessageReceived;
        NetworkedClient.OnSpectateGame += NetworkedClient_OnSpectateGame;
        NetworkedClient.OnReplayGame += NetworkedClient_OnReplayGame;

        GameObject SpectateData = GameObject.Find("SpectateData");
        if (SpectateData)
        {
            NetworkedClient.SendMessageToHost(ServerClientSignifiers.SpectateGame + "," + SpectateData.GetComponent<SpectatorData>().GetGameRoomId());
            TicTacToe.SetActive(true);
            InQueueText.SetActive(false);
            _isPlayer = false;
        }

        GameObject MatchHistoryData = GameObject.Find("MatchHistoryData");
        if(MatchHistoryData)
        {
            NetworkedClient.SendMessageToHost(ServerClientSignifiers.ReplaySystem + "," + MatchHistoryData.GetComponent<GameReplayData>().GetReplayId());
            TicTacToe.SetActive(true);
            InQueueText.SetActive(false);
            ListOfReplays.SetActive(true);
            _isPlayer = false;
            TurnText.GetComponent<TMPro.TextMeshProUGUI>().text = "Replay "+ MatchHistoryData.GetComponent<GameReplayData>().GetReplayName();
        }
    }

    private void NetworkedClient_OnReplayGame(string player, string board)
    {
        GameObject newReplayMove = Instantiate(BoardMoveReplay, ListOfReplays.transform.Find("Mask/Panel"));
        newReplayMove.GetComponent<ReplayMoveButton>().SetPlayer(player);
        newReplayMove.GetComponent<ReplayMoveButton>().SetBoard(board);
    }

    private void NetworkedClient_OnSpectateGame(string board)
    {
        UpdateBoard(board);
    }

    private void NetworkedClient_OnChatMessageReceived(string msg)
    {
        GameObject newMessage = Instantiate(ChatMessage, TicTacToe.transform.Find("Chat/ChatMask/Panel"));
        newMessage.GetComponent<TMPro.TextMeshProUGUI>().text = msg;
    }

    private void Event_OnFindingMatch(bool found, string playerMark, string player2Name, int gameRoomId)
    {
        if(found)
        {
            TicTacToe.SetActive(true);
            InQueueText.SetActive(false);
            VsText.GetComponent<TMPro.TextMeshProUGUI>().text = "vs " + player2Name;
            _inGame = true;
            _playerMark = playerMark;
            _gameRoomID = gameRoomId;
            _isPlayer = true;

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
        NetworkedClient.OnChatMessageReceived -= NetworkedClient_OnChatMessageReceived;
        NetworkedClient.OnSpectateGame -= NetworkedClient_OnSpectateGame;
        NetworkedClient.OnReplayGame -= NetworkedClient_OnReplayGame;
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

    public void UpdateBoard(string newBoard)
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
        if(!NetworkedClient.IsSpectator())
        {
            UpdatePlayerTurnText();
        }
        _gameFinished = false;
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
        GameObject SpectateData = GameObject.Find("SpectateData");
        if (SpectateData)
        {
            Destroy(SpectateData);
        }

        GameObject MatchHistoryData = GameObject.Find("MatchHistoryData");
        if (MatchHistoryData)
        {
            Destroy(MatchHistoryData);
        }
        SceneManager.LoadScene(1); //main menu
    }

    public bool IsPlayerTurn() { return _playerTurn; }
    public bool IsInGame() { return _inGame; }
    public bool IsTheGameFinished() { return _gameFinished; }
    public string GetPlayerMark() { return _playerMark; }

    public bool IsPlayer() { return _isPlayer; }
    public void SetAsAPlayer() { _isPlayer = true; }

    public string GetBoard() { return _board; }
    public void SetBoard(string newBoard) { _board = newBoard; }



    

}
