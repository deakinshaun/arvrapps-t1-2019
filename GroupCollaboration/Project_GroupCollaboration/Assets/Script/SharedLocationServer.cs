using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;


public class SharedLocationServer : MonoBehaviour {

    private List<LocationData> serverDatabase = new List<LocationData>();

    private Mutex ServerDatabaseMutex = new Mutex();

    private List<ConnectionInfo> connections = new List<ConnectionInfo>();

    private Mutex connectionsMutex = new Mutex();

    private class ConnectionInfo
    {
        public TcpClient socket;

        public Mutex sendMutex = new Mutex();
        public Mutex receiveMutex = new Mutex();
        public List<int> myObjects = new List<int>();
    }

    public static string getLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new Exception("No networ adapters with and IPv4 address in the system! ");

    }

    private void removeConnection(ConnectionInfo ci)
    {
        foreach (int id in ci.myObjects)
        {
            SharedLocation.markObject(serverDatabase, ServerDatabaseMutex, id, LocationType.Defunct);
        }
        connectionsMutex.WaitOne();
        connections.Remove(ci);
        connectionsMutex.ReleaseMutex();
    }

    private void sendUpdatesToAllClients()
    {
        List<ConnectionInfo> connectionsCopy;

        connectionsMutex.WaitOne();
        connectionsCopy = new List<ConnectionInfo>(connections);
        connectionsMutex.ReleaseMutex();

        foreach (ConnectionInfo ci in connectionsCopy)
        {
            ServerDatabaseMutex.WaitOne();
            try
            {
                foreach (LocationData Id in serverDatabase)
                {
                    Id.status = MessageStatus.Update;
                    SharedLocation.sendMessage(Id, ci.socket.GetStream(), ci.sendMutex);


                }

            }
            catch (Exception e)
            {
                Debug.Log("Server update Failed: " + e + " " + e.Message);
            }

            ServerDatabaseMutex.ReleaseMutex();
        }

    }

    private void handleServerMessages(ConnectionInfo ci)
    {
        NetworkStream stream = ci.socket.GetStream();
        while (true)
        {
            {
                try
                {
                    LocationData Id = SharedLocation.receiveMessage(stream, ci.receiveMutex);
                    SharedLocation.addLocation(serverDatabase, ServerDatabaseMutex, Id);
                    if (!ci.myObjects.Contains(Id.identifier))
                    {
                        ci.myObjects.Add(Id.identifier);
                    }
                    if (Id.locType ==LocationType.Participant)
                    {
                        Id.status = MessageStatus.IdSet;
                        SharedLocation.sendMessage(Id, stream, ci.sendMutex);
                    }

                    sendUpdatesToAllClients();
                }
                catch (Exception e)
                {
                    Debug.Log("Communication Failed: " + e + " " + e.Message);
                    stream.Close();
                    removeConnection(ci);

                    break;
                }
            }
        }

    }

    private void server()
    {
        TcpListener server = null;
        try
        {
            server = new TcpListener(IPAddress.Any, SharedLocation.port);
            SharedLocation.serverName = getLocalIPAddress();

            server.Start();
            while(true)
            {
                TcpClient client = server.AcceptTcpClient();
                ConnectionInfo ci = new ConnectionInfo();
                ci.socket = client;

                connectionsMutex.WaitOne();
                connections.Add(ci);
                connectionsMutex.ReleaseMutex();

                Thread t = new Thread(() => handleServerMessages(ci));
                t.Start();
            }
        }
        catch (Exception e)
        {
            Debug.Log("Server Exception" + e.Message);
        }

        server.Stop();
    }

    public SharedLocationServer()
    {
        Thread t = new Thread(server);
        t.Start();
    }

}
