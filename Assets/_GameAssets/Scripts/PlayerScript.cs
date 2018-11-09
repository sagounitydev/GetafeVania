using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    enum EstadoPlayer { Pausa, AndandoDer, AndandoIzq, Saltando, Sufriendo};
    EstadoPlayer estado = EstadoPlayer.Pausa;


    [SerializeField] LayerMask floorLayer;
    [SerializeField] Transform posPies;
    [SerializeField] Text txtPuntuacion;
    [SerializeField] Text txtVidas;
    [SerializeField] float speed = 6;
    [SerializeField] float jumpForce = 1;
    int vidasMaximas = 5;
    [SerializeField] int vidas;
    int saludMaxima = 100;
    [SerializeField] int salud;
    [SerializeField] Image barraDeVida;
    float vida = 1;
    [SerializeField] UIScript uiScript;
    
    [SerializeField] int puntos = 0;
    [SerializeField] float radioOverlap = 0.01f;
    Animator playerAnimator;
    Rigidbody2D rb2D;

    public int fuerzaImpactoX = 2;
    public int fuerzaImpactoY = 2;

    private void Awake() {         
            vidas = vidasMaximas;
            salud = saludMaxima;
    }

    //ANTERIOR CODIGO
    //bool saltando = false;
    bool mirarFrente = true;

    public void RecibirDanyo(int danyo) {

        vida -= 0.2f;
        barraDeVida.fillAmount = vida;

        salud = salud - danyo;
        if (salud <= 0) {
            vidas--;
            uiScript.RestarVida();
            salud = saludMaxima;
            //Morir();            
        }
        if (estado==EstadoPlayer.AndandoDer) {
            GetComponent<Rigidbody2D>().AddRelativeForce(
            new Vector2(-fuerzaImpactoX, fuerzaImpactoY),
            ForceMode2D.Impulse);
        } else if (estado==EstadoPlayer.AndandoIzq) {
            GetComponent<Rigidbody2D>().AddRelativeForce(
            new Vector2(fuerzaImpactoX, fuerzaImpactoY),
            ForceMode2D.Impulse);
        }
        estado = EstadoPlayer.Sufriendo;
    }

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        txtPuntuacion.text = "Puntuacion:" + puntos;
        vidas = vidasMaximas;
        salud = saludMaxima;
        Vector2 position = GameController.GetPosition();
        if (position != Vector2.zero) {
            this.transform.position = position;
        }
        // txtVidas.text = "Vidas:" + vidas;
    }

    void CambiarOrientacion() {
        print("Cambiar orientacion");
        if (mirarFrente) {
            transform.localScale = new Vector2(-1, 1);
        } else {
            transform.localScale = new Vector2(1, 1);
        }
        mirarFrente = !mirarFrente;
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
            estado = EstadoPlayer.Saltando;
        }
        if (estado==EstadoPlayer.Sufriendo && EstaEnElSuelo()) {
            estado = EstadoPlayer.Pausa;
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

    void FixedUpdate() {

        float xPos = Input.GetAxis("Horizontal");
                
        //PARA USAR FLECHAS
        //float yPos = Input.GetAxis("Vertical");
        float ySpeedActual = rb2D.velocity.y;

        if (estado == EstadoPlayer.Sufriendo) {
            return;
        }

        if (Mathf.Abs(xPos) > 0.01f) {
            playerAnimator.SetBool("Andando", true);
        } else {
            playerAnimator.SetBool("Andando", false);
        }

        if (estado == EstadoPlayer.Saltando) {
            estado = EstadoPlayer.Pausa;
            if (EstaEnElSuelo()) {
                rb2D.velocity = new Vector2(xPos * speed, jumpForce);
            } else {
                rb2D.velocity = new Vector2(xPos * speed, ySpeedActual);
            }
        } else if (xPos > 0.01f) {
            rb2D.velocity = new Vector2(xPos * speed, ySpeedActual);
            estado = EstadoPlayer.AndandoDer;
        } else if (xPos < -0.01f) {
            rb2D.velocity = new Vector2(xPos * speed, ySpeedActual);
            estado = EstadoPlayer.AndandoIzq;
        }

        if (mirarFrente && xPos < -0.01) {
            CambiarOrientacion();
        } else if (!mirarFrente && xPos > 0.01) {
            CambiarOrientacion();
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

    public void RecibirSalud(int incrementoSalud) {
        salud = salud + incrementoSalud;

        salud = Mathf.Min(salud, saludMaxima);

        txtVidas.text = "Vidas: " + salud.ToString();
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

    public int GetVidas() {
        return this.vidas;
    }

   /*public void Morir() {

          Debug.Log("ESTAS MUERTO JUGADOR!!");
          //audioBoom.Play();
          //ParticleSystem ps = Instantiate(prefabExplosion, generadorMuerte.transform.position, generadorMuerte.transform.rotation);
          //ps.Play();
          //estaVivo = false;
          Invoke("MostrarGameOver", 5);
      }

      private void MostrarGameOver() {
          SceneManager.LoadScene(2);
      }*/
}
