using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayMoveButton : MonoBehaviour
{
    private string player;
    private string board;
    private GameScene gameScene;

    private void Start()
    {
        gameScene = GameObject.Find("Canvas").GetComponent<GameScene>();
    }
    public void OnClickReplayMove()
    {
        gameScene.UpdateBoard(board);
    }

    public void SetPlayer(string s) {
        player = s;
        transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = player;
    }
    public string GetPlayer() { return player; }
    public void SetBoard(string newBoard) { board = newBoard; }
    public string GetBoard() { return board; }
}
