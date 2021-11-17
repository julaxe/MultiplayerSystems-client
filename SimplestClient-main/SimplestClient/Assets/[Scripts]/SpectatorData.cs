using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorData : MonoBehaviour
{
    private int gameRoomId;
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    public void SetGameRoomId(int id) { gameRoomId = id; }
    public int GetGameRoomId() { return gameRoomId; }
}
