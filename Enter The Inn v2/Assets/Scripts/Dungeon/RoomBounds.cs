using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBounds : MonoBehaviour
{
    public Bounds floorBounds()
    {
        return GetComponent<MeshRenderer>().bounds;
    }


}
