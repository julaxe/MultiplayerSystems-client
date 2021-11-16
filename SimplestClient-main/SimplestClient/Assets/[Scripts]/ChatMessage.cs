using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    private TMPro.TMP_InputField _inputMessage;
    private GameScene gameScene;
    void Start()
    {
        _inputMessage = GetComponent<TMPro.TMP_InputField>();
        gameScene = GameObject.Find("Canvas").GetComponent<GameScene>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(_inputMessage.text != "" && gameScene.IsPlayer())
            {
                string messageToServer = _inputMessage.text;
                NetworkedClient.SendMessageToHost(ServerClientSignifiers.ChatUpdated + "," + messageToServer);
                _inputMessage.text = "";
            }
        }
    }

}
