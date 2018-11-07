using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    public Image prefabImagenVida;
    public GameObject panelVidas;
    public PlayerScript pls;
    private int numeroVidas;
    //Image nuevaImage;
    Image[] imagenesVida = new Image[5];

    private void Start() {
        numeroVidas = pls.GetVidas();
        imagenesVida = new Image[numeroVidas];

        for(int i = 0; i < imagenesVida.Length; i++)
        {
            imagenesVida[i] = Instantiate(prefabImagenVida, panelVidas.transform);
        }
    }

    private void Update() {
        numeroVidas = pls.GetVidas();
        for (int i = numeroVidas; i < imagenesVida.Length; i++) {
            imagenesVida[i].color = new Color32(160, 160, 160, 128);
        }
    }
}