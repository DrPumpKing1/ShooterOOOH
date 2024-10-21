using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentLink : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private Transform target;

    private void Update()
    {
        target.rotation = parent.rotation;
        target.position = parent.position;
    }
}
