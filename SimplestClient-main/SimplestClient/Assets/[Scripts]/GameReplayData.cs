using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReplayData : MonoBehaviour
{
    private int replayId;

    private string replayName;
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public int GetReplayId() { return replayId; }
    public void SetReplayId(int id) { replayId = id; }

    public void SetReplayName(string name) { replayName = name; }
    public string GetReplayName() { return replayName; }
}
