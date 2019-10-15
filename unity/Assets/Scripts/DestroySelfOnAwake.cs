using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfOnAwake : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject);
    }
}
