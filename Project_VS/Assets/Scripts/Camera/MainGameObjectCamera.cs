using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameObjectCamera : MonoBehaviour
{
    public static Camera instance = null;

    void Awake()
    {
        instance = GetComponent<Camera>();
    }
}
