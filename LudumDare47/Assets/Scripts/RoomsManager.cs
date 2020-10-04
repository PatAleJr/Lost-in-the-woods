using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    public static RoomsManager roomManager;

    public static Dictionary<Vector2, Room> rooms = new Dictionary<Vector2, Room>();

    public Room firstRoom;

    public Animator endGameAnim;

    public static Dictionary<string, List<Vector3>> roomTorches = new Dictionary<string, List<Vector3>>();

    void Awake()
    {
        roomManager = this;
    }

    public static void addNewRoom(Vector2 nPos, Room room)
    {
        rooms.Add(nPos, room);
    }

    public static void removeRoom(Vector2 nPos)
    {
        //Remove torches from room if any
        Room roomToRemove = rooms[nPos];
        string rName = roomToRemove.roomName;

        if (roomTorches.ContainsKey(rName))
        {
            List<Vector3> torches = roomTorches[rName];

            foreach (GameObject torch in GameObject.FindGameObjectsWithTag("torch"))
            {
                if (torches.Contains(torch.transform.position - roomToRemove.transform.position))
                    Destroy(torch);
            }
        }

        //Removes room
        rooms.Remove(nPos);
    }

    public static bool roomExists(Vector2 nPos)
    {
        return rooms.ContainsKey(nPos);
    }

    public static bool isAdjacentToPlayer(Vector2 nPos)
    {
        Vector2 playerNPos = PlayerMove.playerMove.nPos;

        //Find all nPos that are adjacent
        List<Vector2> adjacents = new List<Vector2>();

        adjacents.Add(new Vector2(playerNPos.x, playerNPos.y)); //The current one

        adjacents.Add(new Vector2(playerNPos.x, playerNPos.y + 1));
        adjacents.Add(new Vector2(playerNPos.x + 1, playerNPos.y + 1));
        adjacents.Add(new Vector2(playerNPos.x + 1, playerNPos.y));
        adjacents.Add(new Vector2(playerNPos.x + 1, playerNPos.y - 1));
        adjacents.Add(new Vector2(playerNPos.x, playerNPos.y - 1));
        adjacents.Add(new Vector2(playerNPos.x - 1, playerNPos.y - 1));
        adjacents.Add(new Vector2(playerNPos.x - 1, playerNPos.y));
        adjacents.Add(new Vector2(playerNPos.x - 1, playerNPos.y + 1));

        //Checks if the room is adjacent to player
        foreach (Vector2 adjacent in adjacents)
        {
            if (nPos == adjacent)
                return true;
        }

        return false;
    }

    public static Room[] getAdjacents(Vector2 nPos)
    {
        Room[] adjacents = new Room[4];

        Vector2 above = nPos + new Vector2(0, 1);
        Vector2 right = nPos + new Vector2(1, 0);
        Vector2 below = nPos + new Vector2(0, -1);
        Vector2 left = nPos + new Vector2(-1, 0);

        if (rooms.ContainsKey(above))
            adjacents[0] = rooms[above];

        if (rooms.ContainsKey(right))
            adjacents[1] = rooms[right];

        if (rooms.ContainsKey(below))
            adjacents[2] = rooms[below];

        if (rooms.ContainsKey(left))
            adjacents[3] = rooms[left];

        return adjacents;
    }

    public static void destroyRooms()
    {
        List<Room> toDestroy = new List<Room>();

        //Delete all rooms not adjacent to player
        foreach (KeyValuePair<Vector2, Room> entry in rooms)
        {
            if (!isAdjacentToPlayer(entry.Key))
            {
                toDestroy.Add(entry.Value);
            }
        }

        foreach (Room room in toDestroy)
        {
            room.destroy();
        }
    }

    public static void addTorch(string room, Vector3 pos)
    {
        if (roomTorches.ContainsKey(room))
        {
            roomTorches[room].Add(pos);
        }
        else {
            roomTorches.Add(room, new List<Vector3>());
            roomTorches[room].Add(pos);
        }
    }

    public static void removeTorch(string room, Vector3 pos)
    {
        if (roomTorches.ContainsKey(room))
        {
            roomTorches[room].Remove(pos);


            if (!torchesLeft(room))
                roomTorches.Remove(room);
        }
    }

    public static bool torchesLeft(string room)
    {
        foreach (Vector3 torch in roomTorches[room])
        {
            if (torch != null)
                return true;
        }
        return false;
    }

    public static List<Vector3> getTorches(string roomName)
    {
        if (roomTorches.ContainsKey(roomName))
            return roomTorches[roomName];

        return new List<Vector3>();
    }
}
