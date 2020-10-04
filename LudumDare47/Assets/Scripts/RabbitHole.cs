using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RabbitHole : MonoBehaviour
{
    private Transform player;

    public float interactDistance = 1f;

    public GameObject prompt;

    void Start()
    {
        prompt.SetActive(false);
        StartCoroutine(setupPlayer());
    }

    IEnumerator setupPlayer()
    {
        yield return new WaitForSeconds(0.1f);
        player = PlayerMove.playerMove.transform;
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < interactDistance)
        {
            prompt.SetActive(true);


            if (Input.GetButtonDown("Fire1"))
            {
                enterRabbitHole();
            }
        }
        else {
            prompt.SetActive(false);
        }
    }

    public void enterRabbitHole()
    {
        RoomsManager.roomManager.endGameAnim.SetTrigger("ExitScene");
        StartCoroutine(endGame());
    }

    IEnumerator endGame()
    {
        yield return new WaitForSeconds(10.2f);
        SceneManager.LoadScene("Menu");
    }
}
