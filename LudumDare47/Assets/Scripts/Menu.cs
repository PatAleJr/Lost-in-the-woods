using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Animator blackAnim;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            blackAnim.SetTrigger("ExitScene");
            StartCoroutine(exitScene());
        }
    }

    IEnumerator exitScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }
}
