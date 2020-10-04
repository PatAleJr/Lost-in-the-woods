using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torches : MonoBehaviour
{
    private PlayerMove playerMove;

    public float torchPlaceDistance = 1;

    public GameObject torchPrefab;

    public AudioClip torchPlaceClip;
    public AudioClip torchRemoveClip;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            placeTorch();
        }
    }

    public void removeTorch()
    {
        Vector2 checkPos = new Vector2(transform.position.x, transform.position.y) + playerMove.facing * torchPlaceDistance;
        Collider2D[] torches = Physics2D.OverlapBoxAll(checkPos, new Vector2(1f, 1f), 0);

        foreach (Collider2D torch in torches)
        {
            if (torch.gameObject.tag == "torch")
            {
                //Remove from room manager
                Room currentRoom = PlayerMove.playerMove.currentRoom;
                string room = currentRoom.roomName;
                RoomsManager.removeTorch(room, torch.transform.position - currentRoom.transform.position);

                //Destroy it
                Destroy(torch.gameObject);

                source.clip = torchRemoveClip;
                source.Play();
            }
        }
    }

    public void placeTorch()
    {
        Vector2 torchPos = new Vector2(transform.position.x, transform.position.y) + playerMove.facing * torchPlaceDistance;
        if (Physics2D.OverlapBoxAll(torchPos, new Vector2(0.5f, 0.5f), 0).Length != 0)
        {
            removeTorch();
        }
        else
        {
            GameObject torch = Instantiate(torchPrefab);
            torch.transform.position = torchPos;

            //Add to room manager
            Room currentRoom = PlayerMove.playerMove.currentRoom;
            string room = currentRoom.roomName;

            Vector3 relativeTorchPos = torch.transform.position - currentRoom.transform.position;
            RoomsManager.addTorch(room, relativeTorchPos);

            source.clip = torchPlaceClip;
            source.Play();
        }
    }
}
