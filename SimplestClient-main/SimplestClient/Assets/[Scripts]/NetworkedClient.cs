using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;

public static class NetworkedClient
{
    private static int connectionID;
    private static int maxConnections = 1000;
    private static int reliableChannelID;
    private static int unreliableChannelID;
    private static int hostID;
    private static int socketPort = 25001;
    private static byte error;
    private static bool isConnected = false;
    private static int ourClientID;
    private static bool isSpectator = false;

    private static string serverMsg;

    public static event Action<bool> OnMessageReceived;
    public static event Action<bool, string> OnLogin;
    public static event Action<bool, string, string, int> OnFindingMatch;
    public static event Action<bool> OnPlayerTurnChanged;
    public static event Action<string> OnBoardChanged;
    public static event Action<bool> OnPlayerWin;
    public static event Action OnRestart;
    public static event Action<string> OnChatMessageReceived;
    public static event Action<string, int> OnSpectateList;
    public static event Action<string> OnSpectateGame;
    public static event Action<string, int> OnMatchHistory;
    public static event Action<string, string> OnReplayGame;




    public static void UpdateNetworkConnection()
    {
        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    Debug.Log("connected.  " + recConnectionID);
                    ourClientID = recConnectionID;
                    break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    ProcessRecievedMsg(msg, recConnectionID);
                    //Debug.Log("got msg = " + msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    isConnected = false;
                    Debug.Log("disconnected.  " + recConnectionID);
                    break;
            }
        }
    }
    
    public static void Connect()
    {

        if (!isConnected)
        {
            Debug.Log("Attempting to create connection");

            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            Debug.Log("Socket open.  Host ID = " + hostID);

            connectionID = NetworkTransport.Connect(hostID, "192.168.1.108", socketPort, 0, out error); // server is local on network

            if (error == 0)
            {
                isConnected = true;

                Debug.Log("Connected, id = " + connectionID);
                //SendMessageToHost("Hello from client");

            }
        }
    }
    
    public static void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }
    
    public static void SendMessageToHost(string msg)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }

    private static void ProcessRecievedMsg(string msg, int id)
    {
        string[] data = msg.Split(',');
        serverMsg = data[2];
        if (data[0] == ServerStatus.Error)
        {
            if (data[1] == ServerClientSignifiers.Login || data[1] == ServerClientSignifiers.Register)
            {
                OnLogin?.Invoke(false, serverMsg);
            }
            else if (data[1] == ServerClientSignifiers.FindMatch)
            {
                OnFindingMatch?.Invoke(false,"", "", -1);
            }
            else if (data[1] == ServerClientSignifiers.InGame)
            {
                OnPlayerTurnChanged?.Invoke(false);
            }
            else if (data[1] == ServerClientSignifiers.PlayerWin)
            {
                OnPlayerWin?.Invoke(false); // you win.
            }
        }
        else if (data[0] == ServerStatus.Success)
        {
            if (data[1] == ServerClientSignifiers.Login || data[1] == ServerClientSignifiers.Register)
            {
                OnLogin?.Invoke(true, "");
            }
            else if (data[1] == ServerClientSignifiers.FindMatch)
            {
                OnFindingMatch?.Invoke(true, data[3] == "Player1" ? "1" : "2", data[4], int.Parse(data[5]));
            }
            else if (data[1] == ServerClientSignifiers.InGame)
            {
                OnPlayerTurnChanged?.Invoke(true); //player turn
            }
            else if (data[1] == ServerClientSignifiers.Board)
            {
                OnBoardChanged?.Invoke(data[3]); //board data
            }
            else if (data[1] == ServerClientSignifiers.PlayerWin)
            {
                OnPlayerWin?.Invoke(true); // you win.
            }
            else if (data[1] == ServerClientSignifiers.Restart)
            {
                OnRestart?.Invoke();
            }
            else if (data[1] == ServerClientSignifiers.ChatUpdated)
            {
                OnChatMessageReceived?.Invoke(data[3]);
            }
            else if (data[1] == ServerClientSignifiers.SpectateList)
            {
                OnSpectateList?.Invoke(data[3], int.Parse(data[4]));
            }
            else if (data[1] == ServerClientSignifiers.SpectateGame)
            {
                OnSpectateGame?.Invoke(data[3]);
            }
            else if (data[1] == ServerClientSignifiers.MatchHistory)
            {
                OnMatchHistory?.Invoke(data[3], int.Parse(data[4]));
            }
            else if (data[1] == ServerClientSignifiers.ReplaySystem)
            {
                OnReplayGame?.Invoke(data[3], data[4]);
            }


        }
        Debug.Log("---- From server: " + msg);
    }

    public static bool IsConnected() { return isConnected;}
    public static bool IsSpectator() { return isSpectator; }

    public static void SetSpectator(bool value) { isSpectator = value; }

    public static string GetServerMessage() { return serverMsg;}



}
public static class ServerClientSignifiers
{
    public static string Message = "000";
    public static string Login = "001";
    public static string Register = "002";
    public static string FindMatch = "003";
    public static string InGame = "004";
    public static string Board = "005";
    public static string PlayerWin = "006";
    public static string Restart = "007";
    public static string ChatUpdated = "008";
    public static string SpectateList = "009";
    public static string SpectateGame = "010";
    public static string MatchHistory = "011";
    public static string ReplaySystem = "012";
}
public static class ServerStatus
{
    public static string Success = "001";
    public static string Error = "002";
}

