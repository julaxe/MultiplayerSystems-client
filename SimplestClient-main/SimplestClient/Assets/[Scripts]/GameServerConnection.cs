using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameServerConnection : MonoBehaviour
{
    private void Awake()
    {
        //check if there is more objects like this
        GameServerConnection[] list = GameObject.FindObjectsOfType<GameServerConnection>();
        OnlyOne(list);
    }
    void Start()
    {
        NetworkedClient.Connect();

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        NetworkedClient.UpdateNetworkConnection();

        if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            NetworkedClient.SendMessageToHost("Hellooooooo");
        }
    }

    private void OnlyOne(GameServerConnection[] list)
    {
        if (list.Length > 1)
        {
            Destroy(this);
        }
    }
}
