using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MessageStatus { Add, IdSet, Update };
public enum LocationType { Participant, Portal, Defunct};


[Serializable]
public class LocationData {
    public MessageStatus status;
    public LocationType locType;
    public int identifier;
    public float latitude;
    public float longtitude;
    public float altitude;
}
