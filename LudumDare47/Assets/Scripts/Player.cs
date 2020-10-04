using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player thePlayer;
    public static Transform playerTransform;

    void Start()
    {
        if (thePlayer != null)
        {
            thePlayer = this;
            playerTransform = transform;
        }
    }
}
