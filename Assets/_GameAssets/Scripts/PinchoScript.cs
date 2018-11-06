using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchoScript : MonoBehaviour {

    [SerializeField] int danyo = 30;
    [SerializeField] protected ParticleSystem psExplosion;
    [SerializeField] Transform posExploGeneracion;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Player") {
            collision.gameObject.GetComponent<PlayerScript>().RecibirDanyo(danyo);
            ParticleSystem ps = Instantiate(psExplosion, posExploGeneracion.position, Quaternion.identity);
            ps.Play();
        }
    }
}
