using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_RToResetPosition : MonoBehaviour
{
    Vector3 pos;
    Quaternion rot;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        rot = transform.rotation;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = pos;
            transform.rotation = rot;

            if(rb)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
}
