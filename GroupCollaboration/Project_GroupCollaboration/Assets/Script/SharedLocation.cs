using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;


public class SharedLocation : MonoBehaviour
{
    [Tooltip("A text field where the server can display its own address.")]
    public Text serverAddress;

    [Tooltip("An input field to enter the address of a server that the client connects to.")]
    public InputField serverInput;


    [Tooltip("A template for the object created to represent a piece at a given location.")]
    public GameObject pieceTemplate;

    [Tooltip("A parent object for any items added by this component.")]
    public GameObject boardState;


    //port No
    static public Int32 port = 8080;

    //Server name
    static public string serverName = null;

    public static bool updateRequired = false;

    //A counter used to assign identifiers
    private static int id = 99;

    private static int getUniqueID ()
    {
        id += 1;
        return id;
    }

    static public LocationData receiveMessage (Stream stream, Mutex receiveMutex)
    {

        BinaryFormatter formatter = new BinaryFormatter();
        receiveMutex.WaitOne();
        LocationData Id = (LocationData)formatter.Deserialize(stream);
        receiveMutex.ReleaseMutex();
        return Id;
    }

    static public void sendMessage (LocationData message, Stream stream, Mutex sendMutex)
    {
        MemoryStream ms = new MemoryStream();
        new BinaryFormatter().Serialize(ms, message);
        byte[] serializedData = ms.ToArray();

        sendMutex.WaitOne();
        stream.Write(serializedData, 0, serializedData.Length);
        sendMutex.ReleaseMutex();
    }

    static public void markObject (List<LocationData> list, Mutex mutex, int identifier, LocationType targetType)
    {
        mutex.WaitOne();
        foreach (LocationData Id in list)
        {
            if (Id.identifier == identifier)
            {
                Id.locType = targetType;
            }
            mutex.ReleaseMutex();
        }

    }

    static public void addLocation(List<LocationData> list, Mutex mutex, LocationData Id)
    {
        mutex.WaitOne();
        if (Id.identifier < 0)
        {
            Id.identifier = getUniqueID();
        }

        bool found = false;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].identifier == Id.identifier)
                list[i] = Id;
            found = true;
            break;
        }

        if (!found)
        {
            list.Add(Id);
        }

        mutex.ReleaseMutex();
    }


    public void sendPosition(float lati, float alti, float longi)
    {
        activeClient.sendPosition(lati, alti, longi);
    }

    public void sendPortal(float lati, float alti, float longi)
    {
        activeClient.sendPortal(lati, alti, longi);
    }

    private SharedLocationClient activeClient;

    public void startClient()
    {
        activeClient = new SharedLocationClient(serverInput.text);
    }

    public void startServer ()
    {
        SharedLocationServer s = new SharedLocationServer();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (serverName !=null)
        {
            serverAddress.text = serverName;
        }

        if (updateRequired)
        {
            activeClient.updateScene(boardState, pieceTemplate);
            updateRequired = false;
        }
    }
}
