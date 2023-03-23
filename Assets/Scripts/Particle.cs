/*using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private ParticleSystem _particle;
    // Start is called before the first frame update
    void Start()
    {
        _particle = GetComponent<ParticleSystem>();
    }
        private void OnEnable()
        {
            Player.PlayParticule += particlePlayer;
        }

        private void OnDisable()
        {
            Player.PlayParticule -= particlePlayer;
        }

        public void particlePlayer()
    {
        _particle.Play();
    }
}*/
