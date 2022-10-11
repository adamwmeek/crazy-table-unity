using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -50.0F, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
