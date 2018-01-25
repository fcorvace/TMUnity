using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
//using UnityEngine.EditorSceneManagement;
using UnityEngine.UI; // Required when Using UI elements.
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

public class loginMenu : MonoBehaviour {

    public InputField usernameTxt;
    public InputField passwordTxt;
    public Text outputTxt;

    public void LoginAccess()
    {
        string userName = usernameTxt.text + ";" + CalculateMD5Hash(passwordTxt.text);
        int i = 0;

        Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse("192.168.1.157");
        System.Net.IPEndPoint remoteEP = new IPEndPoint(ipAdd, 10000);
        soc.Connect(remoteEP);
        outputTxt.text = "Connectected to server, authenticating...";

        i++;

        byte[] byData = System.Text.Encoding.ASCII.GetBytes(userName);
        soc.Send(byData);

        byte[] buffer = new byte[1024];
        int iRx = soc.Receive(buffer);
        char[] chars = new char[iRx];

        System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
        int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
        System.String recv = new System.String(chars);
        if(recv == "s")
        {
            outputTxt.text = "Connection Success! Logging in...";
            //SceneManager.UnloadScene("scena");
            //SceneManager.LoadScene("mainScreen", LoadSceneMode.Additive);
            SceneManager.LoadScene("mainScreen", LoadSceneMode.Single);
            //SceneManager.UnloadSceneAsync("scena");
            //EditorSceneManager.
        }
        else
        {
            outputTxt.text = "Invalid Username or Password, try again.";
        }
    }

    public string CalculateMD5Hash(string input)
    {
        // step 1, calculate MD5 hash from input
        MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        // step 2, convert byte array to hex string
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }

        return sb.ToString();
    }
}
