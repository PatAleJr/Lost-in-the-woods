using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove playerMove;

    public float moveSpeed = 4f;
    private Vector2 move = new Vector2(0, 0);

    private string dir; //hor or ver or none
    public Vector2 facing = new Vector2(0, -1); //way sprite is facing

    private Rigidbody2D rb;

    public Room currentRoom;
    public Room oldRoom;
    public Vector2 nPos;

    private Animator anim;

    private void Start()
    {
        playerMove = this;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        dir = "none";

        currentRoom = RoomsManager.roomManager.firstRoom;
        oldRoom = currentRoom;
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");


        if (dir == "none")
        { 
            if (inputX != 0)
            {
                dir = "hor";
            }
            else if (inputY != 0)
            {
                dir = "ver";
            }

            move = new Vector2(0, 0);
        }
        else if (dir == "hor")
        {
            if (inputX != 0)
            {
                move = new Vector3(inputX * moveSpeed, 0);

                if (facing != move.normalized)
                {
                    facing = move.normalized;
                    anim.SetTrigger("WalkSide");
                    transform.localScale = new Vector3(facing.x, 1, 1);
                }
                anim.speed = 1;
            }
            else {
                dir = "none";
                anim.speed = 0;
            }
        }
        else if (dir == "ver")
        {
            if (inputY != 0)
            {
                move = new Vector3(0, inputY * moveSpeed, 0);

                if (facing != move.normalized)
                {
                    facing = move.normalized;
                    if (facing.y > 0)
                    {
                        anim.SetTrigger("WalkUp");
                    }
                    else {
                        anim.SetTrigger("WalkDown");
                    }
                    anim.speed = 1;
                }

            }
            else
            {
                dir = "none";
                anim.speed = 0;
            }
        }

        //If entered new room
        if (getCurrentRoom() != currentRoom)
        {
            oldRoom = currentRoom;
            currentRoom = getCurrentRoom();
            
            currentRoom.loadAdjacent(oldRoom);
            RoomsManager.destroyRooms();
        }
    }

    private Room getCurrentRoom()
    {
        float xx = Mathf.RoundToInt(transform.position.x / Room.roomWidth);
        float yy = Mathf.RoundToInt(transform.position.y / Room.roomHeight);
        nPos = new Vector2(xx, yy);
        return RoomsManager.rooms[nPos];
    }

    private void FixedUpdate()
    {
        rb.velocity = move;
    }
}
