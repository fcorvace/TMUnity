using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // Required when Using UI elements.
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

public class loginMenu : MonoBehaviour {

    public InputField usernameTxt;
    public InputField passwordTxt;
    public GameObject uName;
    public GameObject pWord;

    public void LoginAccess()
    {
        string uName2 = "";
        string pWord2 = "";
        string userName = usernameTxt.text + ";" + CalculateMD5Hash(passwordTxt.text);
        int i = 0;

        Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse("192.168.1.157");
        System.Net.IPEndPoint remoteEP = new IPEndPoint(ipAdd, 10000);
        soc.Connect(remoteEP);

        /*uName = GameObject.Find("unameText");
        uName2 = uName.GetComponent<Text>().text;
        pWord = GameObject.Find("pwordText");
        pWord2 = pWord.GetComponent<Text>().text;
        pWord2 = CalculateMD5Hash(pWord2);*/
        i++;

        byte[] byData = System.Text.Encoding.ASCII.GetBytes(userName);
        soc.Send(byData);

        byte[] buffer = new byte[1024];
        int iRx = soc.Receive(buffer);
        char[] chars = new char[iRx];

        System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
        int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
        System.String recv = new System.String(chars);
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
