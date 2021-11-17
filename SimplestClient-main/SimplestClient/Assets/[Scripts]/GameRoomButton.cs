using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoomButton : MonoBehaviour
{
    private int _gameRoomId;

    public void SetGameId(int id)
    {
        _gameRoomId = id;
    }
    public  int GetGameId()
    {
        return _gameRoomId;
    }
    public void OnClickGameRoom()
    {
        GameObject SpectatorData = GameObject.Find("SpectateData");
        if (SpectatorData)
        {
            GameObject.Find("Canvas").GetComponent<SpectateListScene>().UnsubscribeEvents();
            SpectatorData.GetComponent<SpectatorData>().SetGameRoomId(_gameRoomId);
            SceneManager.LoadScene(2);
        }
    }    
}
