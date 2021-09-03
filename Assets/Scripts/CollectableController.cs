using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    private void Start()
    {
        gameObject.tag = "Collectable";
    }
}
