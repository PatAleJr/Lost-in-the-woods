using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //Up, right, down, left \\ N E S W
    public GameObject[] adjacentRooms = new GameObject[4]; //Room prefabs
    public Room[] realAdjacents = new Room[4]; //Rooms that are really there

    public static float roomWidth = 256/16;
    public static float roomHeight = 144/16;

    private Vector2 nPos;   //Numeric position

    public bool isFirstRoom = false;

    public bool isCurrentRoom = false;

    private Transform player;

    public GameObject torchPrefab;

    public string roomName;

    private void Start()
    {
        player = Player.playerTransform;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        nPos = new Vector2(pos.x / roomWidth, pos.y / roomHeight);

        if (!RoomsManager.roomExists(nPos))
            RoomsManager.addNewRoom(nPos, this);

        if (isFirstRoom)
        {
            loadAdjacent(null);
        }

        loadTorches();
    }

    public void loadTorches()
    {
        List<Vector3> torches = RoomsManager.getTorches(roomName);

        foreach (Vector3 torchPos in torches)
        {
            GameObject torch = Instantiate(torchPrefab);
            torch.transform.position = transform.position + torchPos;
        }
    }

    public void destroy()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        RoomsManager.removeRoom(nPos);
        Destroy(gameObject);
    }

    public void loadAdjacent(Room cameFrom)
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 roomPos = new Vector3(0,0,0);
            Vector3 roomDiagPos = new Vector3(0, 0, 0);
            switch (i)
            {
                case 0:
                    roomPos = new Vector3(0, roomHeight, 0);
                    break;
                case 1:
                    roomPos = new Vector3(roomWidth, 0, 0);
                    break;
                case 2:
                    roomPos = new Vector3(0, -roomHeight, 0);
                    break;
                case 3:
                    roomPos = new Vector3(-roomWidth, 0, 0);
                    break;
            }

            Vector2 roomNPos = new Vector2(roomPos.x / roomWidth, roomPos.y / roomHeight);

            if (!RoomsManager.roomExists(roomNPos + nPos) && adjacentRooms[i] != null)
            {
                Room room = Instantiate(adjacentRooms[i], transform.position + roomPos, Quaternion.identity).GetComponent<Room>();
                RoomsManager.addNewRoom(roomNPos + nPos, room);
            }
        }

        realAdjacents = RoomsManager.getAdjacents(nPos);

        for (int i = 0; i < 4; i++)
        {
            Vector3 roomDiagPos = new Vector3(0, 0, 0);
            switch (i)
            {
                case 0:
                    roomDiagPos = new Vector3(roomWidth, roomHeight, 0);
                    break;
                case 1:
                    roomDiagPos = new Vector3(roomWidth, -roomHeight, 0);
                    break;
                case 2:
                    roomDiagPos = new Vector3(-roomWidth, -roomHeight, 0);
                    break;
                case 3:
                    roomDiagPos = new Vector3(-roomWidth, roomHeight, 0);
                    break;
            }

            Vector2 roomDiagNPos = new Vector2(roomDiagPos.x / roomWidth, roomDiagPos.y / roomHeight);

            if (!RoomsManager.roomExists(roomDiagNPos + nPos))
            {
                int j = i + 1;
                if (j == 4)
                    j = 0;

                if (realAdjacents[i] != null && realAdjacents[i].adjacentRooms[j] != null)
                    Instantiate(realAdjacents[i].adjacentRooms[j], transform.position + roomDiagPos, Quaternion.identity);

            }
        }
    }
}
