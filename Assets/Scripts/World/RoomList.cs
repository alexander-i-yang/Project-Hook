using ASK.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using World;

public class RoomList : MonoBehaviour, IFilterLoggerTarget
{
    public static List<Room> Rooms;

    private void Awake()
    {
        Rooms = new List<Room>(GetComponentsInChildren<Room>(true));
    }

    public LogLevel GetLogLevel()
    {
        return LogLevel.Info;
    }
}
