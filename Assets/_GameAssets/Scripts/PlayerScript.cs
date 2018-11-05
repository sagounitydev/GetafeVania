using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    [SerializeField] LayerMask floorLayer;
    [SerializeField] Transform posPies;
    [SerializeField] Text txtPuntuacion;
    [SerializeField] Text txtVidas;
    [SerializeField] float speed = 6;
    [SerializeField] float jumpForce = 1;
    int vidasMaximas = 3;
    [SerializeField] int vidas;
    [SerializeField] int puntos = 0;
    [SerializeField] float radioOverlap = 0.01f;
    Rigidbody2D rb2D;
    bool saltando = false;

	void Start () {
        rb2D = GetComponent<Rigidbody2D>();
        txtPuntuacion.text = "Puntuacion:" + puntos;
       // txtVidas.text = "Vidas:" + vidas;
    }
	
    private bool EstaEnElSuelo() {
        bool enSuelo = false;
        Collider2D colider = Physics2D.OverlapCircle(posPies.position, radioOverlap, floorLayer);
        if (colider != null) {
            enSuelo = true;
        }
        return enSuelo;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            saltando = true;
        }
    }

    /*
    //VERSION BASADA EN TAGS
    private bool EstaEnElSuelo() {
        bool enSuelo = false;
        Collider2D[] cols = Physics2D.OverlapCircleAll(posPies.position, radioOverlap);
        for (int i = 0; i < cols.Length; i++) {
            if (cols[i].gameObject.tag == "Suelo") {
                enSuelo = true;
                break;
            }
        }
        return enSuelo;
    }
	*/

    void FixedUpdate () {
        
        float xPos = Input.GetAxis("Horizontal");        
        
        //PARA USAR FLECHAS
        //float yPos = Input.GetAxis("Vertical");
        float ySpeedActual = rb2D.velocity.y;

        if (saltando) {
            saltando = false;
            if (EstaEnElSuelo()) {
                rb2D.velocity = new Vector2(xPos * speed, jumpForce);
            } else {
                rb2D.velocity = new Vector2(xPos * speed, ySpeedActual);
            }
        } else {
            rb2D.velocity = new Vector2(xPos * speed, ySpeedActual);
        }
        }
              

        //ESTO ES UTILIZADO CURSORES
        /*if (yPos>0) {
            if (EstaEnElSuelo()) {
                rb2D.velocity = new Vector2(xPos * speed, jumpForce);
            } else {
                rb2D.velocity = new Vector2(xPos * speed, ySpeedActual);
            }           
        } else {
            rb2D.velocity = new Vector2(xPos * speed, ySpeedActual);
        }*/
    

    public void incrementarPuntuacion(int puntosAIncrementar) {
        puntos = puntos + puntosAIncrementar;
        txtPuntuacion.text = "Puntuacion:" + puntos;

    }

    public void sumarVida(int vidasAIncrementar) {
        vidas = vidas + vidasAIncrementar;
        txtVidas.text = "Vidas:" + vidas;

    }

    public void restarVida(int vidasARestar) {
        vidas = vidas - vidasARestar;
        txtVidas.text = "Vidas:" + vidas;

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Gema")) {
            incrementarPuntuacion(10);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Vida")) {
            sumarVida(1);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Vida")) {
            restarVida(-1);
            Destroy(collision.gameObject);
        }
    }

  /*  public void RecibirDanyo(int danyo) {

        Debug.Log("RECIBIENDO DAÑO");

        vidas = vidas - danyo;

        if (vidas <= 0) {
            vidas = 0;
            Morir();
        }
    }

 public void Morir() {

        Debug.Log("ESTAS MUERTO JUGADOR!!");
        audioBoom.Play();
        ParticleSystem ps = Instantiate(prefabExplosion, generadorMuerte.transform.position, generadorMuerte.transform.rotation);
        ps.Play();
        estaVivo = false;
        Invoke("MostrarGameOver", 5);
    }

    private void MostrarGameOver() {
        SceneManager.LoadScene(2);
    }*/
}
