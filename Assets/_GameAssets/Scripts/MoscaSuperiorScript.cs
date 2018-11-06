using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoscaSuperiorScript : MonoBehaviour {

    [SerializeField] protected ParticleSystem psExplosion;
    [SerializeField] Transform posExploGeneracion;

    private void OnCollisionEnter2D(Collision2D collision) {
        ParticleSystem ps = Instantiate(psExplosion, posExploGeneracion.position, Quaternion.identity);
        ps.Play();
        Destroy(transform.parent.gameObject);
    }
}
