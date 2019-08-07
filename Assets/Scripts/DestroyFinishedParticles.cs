using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyFinishedParticles : MonoBehaviour
{
    private ParticleSystem _ps;

    // Start is called before the first frame update
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(!_ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
