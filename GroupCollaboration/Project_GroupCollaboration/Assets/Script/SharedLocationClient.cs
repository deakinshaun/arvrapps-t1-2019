using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;

public class SharedLocationClient : MonoBehaviour {

    private TcpClient clientSocket = null;

    private Mutex clientSendMutex = new Mutex();

    private Mutex clientReceiveMutex = new Mutex();

    private int clientIdentifier = -1;

    private List<LocationData> clientDatabase = new List<LocationData>();

    private Mutex clientDatabaseMutex = new Mutex();


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void handleClient (TcpClient clientSocket)
    {
        NetworkStream stream = clientSocket.GetStream();

        while (true)
        {
            try
            {
                LocationData Id = SharedLocation.receiveMessage(stream, clientReceiveMutex);

                switch (Id.status)
                {
                    case MessageStatus.IdSet:
                        clientIdentifier = Id.identifier;
                        break;
                    case MessageStatus.Update:
                        SharedLocation.addLocation(clientDatabase, clientDatabaseMutex, Id);
                        SharedLocation.updateRequired = true;
                        break;
                    default:
                        Debug.Log("Unexpected message type from server: " + Id.status);
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Client Communication Failed: " + e + " " + e.Message);
                stream.Close();
            }
        }
    }

    public SharedLocationClient (string host)
    {
        clientIdentifier = -1;
        try
        {
            Debug.Log("Client Exception: " + host);
            clientSocket = new TcpClient(host, SharedLocation.port);
        }
        catch (Exception e)
        {
            Debug.Log("Client Eception: " + e.Message);
        }
        Thread t = new Thread(() => handleClient(clientSocket));
        t.Start();

    }

    public void sendPosition (float lati, float alti, float longi)
    {
        if (clientSocket == null)
            return;

        LocationData Id = new LocationData();
        Id.status = MessageStatus.Add;
        Id.locType = LocationType.Participant;
        Id.identifier = clientIdentifier;
        Id.latitude = lati;
        Id.longtitude = longi;
        Id.altitude = alti;
    }

    public void sendPortal(float lati, float alti, float longi)
    {
        if (clientSocket == null)
            return;

        LocationData Id = new LocationData();
        Id.status = MessageStatus.Add;
        Id.locType = LocationType.Portal;
        Id.identifier = -1;
        Id.latitude = lati;
        Id.longtitude = longi;
        Id.altitude = alti;

        SharedLocation.sendMessage(Id, clientSocket.GetStream(), clientSendMutex);

    }

    public void updateScene (GameObject boardState, GameObject pieceTemplate)
    {
        foreach(Transform child in boardState.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        clientDatabaseMutex.WaitOne();

        foreach (LocationData Id in clientDatabase)
        {
            GameObject g = GameObject.Instantiate(pieceTemplate);
            g.transform.position = new Vector3(Id.latitude + 1.6f, Id.altitude - 1.9f, Id.longtitude + 7.2f);
            /*
            Color c = new Color();
            switch (Id.locType)
            {
                case LocationType.Participant: c = new Color(1, 0, 0);
                    break;
                case LocationType.Waypoint: c = new Color(0, 1, 0);
                    break;
                case LocationType.Defunct: c = new Color(0.4f, 0.4f, 0.4f);
                    break;
            }
            
            g.GetComponent<MeshRenderer>().material.color = c;
            */
            g.transform.SetParent(boardState.transform);

        }

        clientDatabaseMutex.ReleaseMutex();

    }


 }

