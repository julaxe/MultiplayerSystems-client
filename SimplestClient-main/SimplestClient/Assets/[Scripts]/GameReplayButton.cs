using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameReplayButton : MonoBehaviour
{
    // Start is called before the first frame update
    private int replayId;
    private string replayName;
    public void OnClickReplayGame()
    {
        GameObject gameReplayData = GameObject.Find("MatchHistoryData");
        if (gameReplayData)
        {
            GameObject.Find("Canvas").GetComponent<MatchHistoryScene>().UnsubscribeEvents();
            gameReplayData.GetComponent<GameReplayData>().SetReplayId(replayId);
            gameReplayData.GetComponent<GameReplayData>().SetReplayName(replayName);
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
    }

    public void SetReplayId(int id) { replayId = id; }
    public int GetReplayId() { return replayId; }
    public void SetReplayName(string name) { replayName = name; }
    public string GetReplayName() { return replayName; }
}
