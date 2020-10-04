using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthSorter : MonoBehaviour
{
    public bool isStatic;
    public float offset;    //From center to lowest point in sprite. In pixels.

    public static float precision = 4;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        updateDepth();
        if (isStatic)
            Destroy(this);
    }

    void updateDepth()
    {
        sr.sortingOrder = Mathf.RoundToInt(-(transform.position.y + (offset/ 16)) * precision);
    }

    void Update()
    {
        updateDepth();
    }
}
