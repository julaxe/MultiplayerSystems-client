using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

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
    private static bool serverErrorStatus = false;
    private static bool isLoggedIn = false;
    private static bool inGame = false;
    private static bool isPlayer1 = false;
    private static bool isPlayerTurn = false;
    private static bool endGame = false;
    private static bool youWin = false;
    private static bool restart = false;
    private static string player2 = "";
    private static string board = "0 0 0 0 0 0 0 0 0";
    private static string serverMsg;

  

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

            connectionID = NetworkTransport.Connect(hostID, "192.168.1.107", socketPort, 0, out error); // server is local on network

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
            serverErrorStatus = true;
            if (data[1] == ServerClientSignifiers.FindMatch)
            {
                inGame = false;
                player2 = "";
            }
            else if (data[1] == ServerClientSignifiers.InGame)
            {
                isPlayerTurn = false;
            }
            else if (data[1] == ServerClientSignifiers.PlayerWin)
            {
                endGame = true;
            }
        }
        else if (data[0] == ServerStatus.Success)
        {
            serverErrorStatus = false;
            if (data[1] == ServerClientSignifiers.Login || data[1] == ServerClientSignifiers.Register)
            {
                isLoggedIn = true;
            }
            else if (data[1] == ServerClientSignifiers.FindMatch)
            {
                inGame = true;
                if (data[3] == "Player1")
                {
                    isPlayer1 = true;
                }
                player2 = data[4];
            }
            else if (data[1] == ServerClientSignifiers.InGame)
            {
                isPlayerTurn = true;
            }
            else if (data[1] == ServerClientSignifiers.Board)
            {
                board = data[3];
            }
            else if (data[1] == ServerClientSignifiers.PlayerWin)
            {
                endGame = true;
                youWin = true;
            }
            else if (data[1] == ServerClientSignifiers.Restart)
            {
                restart = true;
            }


        }
        Debug.Log("---- From server: " + msg);
    }

    public static bool IsConnected() { return isConnected;}
    public static bool IsLoggedIn() { return isLoggedIn;}
    public static bool InGame() { return inGame;}
    public static bool IsPlayer1() { return isPlayer1; }
    public static bool IsPlayerTurn() { return isPlayerTurn; }
    public static string Player2() { return player2; }

    public static bool YouWin() { return youWin; }
    public static bool EndGame() { return endGame; }
    public static bool Restart() { return restart; }
    public static string GetBoard() { return board; }
    public static void SetBoard(string newBoard) { board = newBoard; }
    public static string GetServerMessage() { return serverMsg;}
    public static bool GetServerErrorStatus() { return serverErrorStatus;}

    public static void RestartBoard()
    {
        youWin = false;
        endGame = false;
        restart = false;
        isPlayer1 = false;
    }


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
}
public static class ServerStatus
{
    public static string Success = "001";
    public static string Error = "002";
}

