using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
    public void DestroyParent()
    {
        Destroy(gameObject);
    }
}
