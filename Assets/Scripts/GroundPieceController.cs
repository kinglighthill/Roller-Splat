using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPieceController : MonoBehaviour
{
    public bool isColoured = false;

    public void ChangeColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
        isColoured = true;

        GameManager.singleton.CheckComplete();
    }
}
