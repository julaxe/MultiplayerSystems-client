using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginForm : MonoBehaviour
{
    // all the components inside the login
    private GameObject UsernameObject;
    private GameObject PasswordObject;
    private GameObject LoginButtonObject;
    private GameObject RegisterButtonObject;
    private GameObject ErrorMessage;

    private string username = "";
    private string password = "";
    void Start()
    {
        //initialize our gameobjects with the object from our game.
        GameObject[] listOfObjects = FindObjectsOfType<GameObject>();

        foreach(GameObject go in listOfObjects)
        {
            if(go.name == "UsernameInput")
            {
                UsernameObject = go;
            }
            else if(go.name == "PasswordInput")
            {
                PasswordObject = go;
            }
            else if (go.name == "LoginButton")
            {
                LoginButtonObject = go;
            }
            else if (go.name == "RegisterButton")
            {
                RegisterButtonObject = go;
            }
            else if(go.name == "ErrorMessage")
            {
                ErrorMessage = go;
            }
        }

        LoginButtonObject.GetComponent<Button>().onClick.AddListener(Login);
        RegisterButtonObject.GetComponent<Button>().onClick.AddListener(Register);
        NetworkedClient.OnLogin += CheckLoginStatus;
        SetErrorMessage(false); //hide the error message
    }
    private void CheckLoginStatus(bool success, string errorMsg)
    {
        if (success)
        {
            SetErrorMessage(false, "");
            NetworkedClient.OnLogin -= CheckLoginStatus;
            SceneManager.LoadScene(1); //Main menu
        }
        else
        {
            SetErrorMessage(true, errorMsg);
        }
    }
    public void Login()
    {
        username = UsernameObject.GetComponent<TMP_InputField>().text;
        password = PasswordObject.GetComponent<TMP_InputField>().text;
        NetworkedClient.SendMessageToHost(ServerClientSignifiers.Login + "," +  username + "," + password);
    }

    public void Register()
    {
        username = UsernameObject.GetComponent<TMP_InputField>().text;
        password = PasswordObject.GetComponent<TMP_InputField>().text;
        NetworkedClient.SendMessageToHost(ServerClientSignifiers.Register + "," + username + "," + password);
    }

    public void SetErrorMessage(bool error, string msg = "")
    {
        ErrorMessage.GetComponent<TMPro.TextMeshProUGUI>().text = msg;
        ErrorMessage.SetActive(error);
    }
}
