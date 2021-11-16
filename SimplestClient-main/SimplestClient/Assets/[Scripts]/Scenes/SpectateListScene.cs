using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpectateListScene : MonoBehaviour
{
    private bool thereAreRooms = false;
    private GameObject NoRoomsMessage;
    private GameObject GameRoom;
    private Transform ListTransform;
    void Start()
    {
        NetworkedClient.SendMessageToHost(ServerClientSignifiers.SpectateList);
        NetworkedClient.OnSpectateList += NetworkedClient_OnSpectateList;
        NoRoomsMessage = transform.Find("NoGamesText").gameObject;
        GameRoom = Resources.Load<GameObject>("Prefabs/GameRoom");
        ListTransform = transform.Find("SpectateList/List");
    }

    private void NetworkedClient_OnSpectateList(string roomName, int roomId)
    {
        if(thereAreRooms == false)
        {
            thereAreRooms = true;
            NoRoomsMessage.SetActive(false);
        }
        GameObject newGameRoom = Instantiate(GameRoom, ListTransform);
        newGameRoom.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = roomName;
        newGameRoom.GetComponent<GameRoomButton>().SetGameId(roomId);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }    
}
