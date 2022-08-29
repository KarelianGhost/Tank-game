using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public Grid map;

    // Start is called before the first frame update
    void Start()
    {
        map = new Grid(10, 10, 2f, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
