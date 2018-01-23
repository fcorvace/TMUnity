using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // Required when Using UI elements.
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;

public class loginMenu : MonoBehaviour {

    public InputField usernameTxt;
    public InputField passwordTxt;
    public GameObject uName;

    public void LoginAccess()
    {
        string userName2 = "";
        string userName = usernameTxt.text + " " + passwordTxt.text;
        int i = 0;

        Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse("192.168.1.157");
        System.Net.IPEndPoint remoteEP = new IPEndPoint(ipAdd, 10000);
        soc.Connect(remoteEP);

        uName = GameObject.Find("unameText");
        userName2 = uName.GetComponent<Text>().text;
        i++;

        byte[] byData = System.Text.Encoding.ASCII.GetBytes("un:" + userName2 + ";pw:" + userName2);
        soc.Send(byData);

        byte[] buffer = new byte[1024];
        int iRx = soc.Receive(buffer);
        char[] chars = new char[iRx];

        System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
        int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
        System.String recv = new System.String(chars);
    }
}
