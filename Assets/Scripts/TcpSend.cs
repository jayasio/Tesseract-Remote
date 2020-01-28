using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;

public class TcpSend
{
    #region tcp
    TcpClient client;
    Thread thread;
    #endregion

    #region  tcp meta
    string IP;
    int port;
    #endregion

    #region tcp methods
    void ServerStart(string IP, int port)
    {
        this.IP = IP;
        this.port = port;

        thread = new Thread(new ThreadStart(() => client = new TcpClient(IP, port)));
        thread.IsBackground = true;
        thread.Start();
    }

    void ServerSend(string message)
    {
        NetworkStream stream = client.GetStream();
        if (stream.CanWrite)
        {
            byte[] messageEncoded = Encoding.ASCII.GetBytes(message + "&&##@@");
            stream.Write(messageEncoded, 0, messageEncoded.Length);
        }
    }

    void ServerStop()
    {
        thread.Interrupt();
        thread = null;
        client = null;
    }
    #endregion
}
