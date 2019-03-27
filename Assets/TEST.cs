
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public AI.Blackboard bb;

    // Start is called before the first frame update
    void Start()
    {
        bb.SetProperty("banan", false);
        bb.SetProperty("quartz", 1);
        bb.SetProperty("lmao", 7.7f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
