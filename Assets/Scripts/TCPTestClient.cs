using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using TMPro;
using System.Net;


public class TCPTestClient
{
    #region private members 	
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private bool _connected = false;
    private string IP = "141.82.174.252";
    private int port = 8052;
    #endregion

    public bool connected { get { return _connected; } }
    // public TMP_InputField IpInputField, PortInputField;

    
    
    public void OnConnect()
    {
        // IP = IpInputField.text;
        // port = int.Parse(PortInputField.text);
        // IP = "192.168.0.102";
        // port = 8052;
        ConnectToTcpServer();
    }

    public void OnDisconnect()
    {
        clientReceiveThread.Abort();
        clientReceiveThread = null;
    }

    private void ConnectToTcpServer()
    {
        if (!_connected)
        {
            try
            {
                clientReceiveThread = new Thread(new ThreadStart(ListenForData));
                clientReceiveThread.IsBackground = true;
                clientReceiveThread.Start();
                _connected = true;
            }
            catch (Exception e)
            {
                Debug.Log("On client connect exception " + e);
            }
        }
    }

    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient(IP, port);
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                        Debug.Log("server message received as: " + serverMessage);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    string clientMessage;
    public void SendTracking(string track)
    {
        clientMessage = track + "&&##@@";
        SendMessage();
    }

    private void SendMessage()
    {
        if (socketConnection == null)
        {
            Handheld.Vibrate();
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                // string clientMessage = "This is a message from one of your clients.";
                // Convert string message to byte array.                 
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                Debug.Log("Client sent his message - should be received by server");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}