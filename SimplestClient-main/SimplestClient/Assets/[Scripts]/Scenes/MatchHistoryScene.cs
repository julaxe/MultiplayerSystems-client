using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchHistoryScene : MonoBehaviour
{
    private bool thereAreRooms = false;
    private GameObject NoGameHistoryText;
    private GameObject GameReplay;
    private Transform ListTransform;

    void Start()
    {
        NetworkedClient.SendMessageToHost(ServerClientSignifiers.MatchHistory);
        NetworkedClient.OnMatchHistory += NetworkedClient_OnMatchHistory;
        NoGameHistoryText = transform.Find("NoGamesText").gameObject;
        GameReplay = Resources.Load<GameObject>("Prefabs/GameReplay");
        ListTransform = transform.Find("MatchHistory/List");
    }

    private void NetworkedClient_OnMatchHistory(string replayName, int replayId)
    {
        if (thereAreRooms == false)
        {
            thereAreRooms = true;
            NoGameHistoryText.SetActive(false);
        }
        GameObject newGameReplay = Instantiate(GameReplay, ListTransform);
        newGameReplay.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = replayName;
        newGameReplay.GetComponent<GameReplayButton>().SetReplayId(replayId);
        newGameReplay.GetComponent<GameReplayButton>().SetReplayName(replayName);
    }
    public void UnsubscribeEvents()
    {
        NetworkedClient.OnMatchHistory -= NetworkedClient_OnMatchHistory;
    }
    public void GoToMainMenu()
    {
        UnsubscribeEvents();
        GameObject replayGameData = GameObject.Find("MatchHistoryData");
        Destroy(replayGameData);
        SceneManager.LoadScene(1);
    }
}
