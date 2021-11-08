using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FindMatch()
    {
        NetworkedClient.SendMessageToHost(ServerClientSignifiers.FindMatch);
        SceneManager.LoadScene(2);//game Scene
    }
    public void SpectateGame()
    {

    }
}
