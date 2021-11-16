using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReplayButton : MonoBehaviour
{
    // Start is called before the first frame update
    private int replayId;
    void Start()
    {
        
    }

    public void SetReplayId(int id) { replayId = id; }
    public int GetReplayId() { return replayId; }
}
