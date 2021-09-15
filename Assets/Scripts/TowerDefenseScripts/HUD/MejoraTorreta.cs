using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MejoraTorreta : MonoBehaviour
{
    public TorretaBasic torretaSelec;
    public Text tNombre;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateContent()
    {
        //Actualización de contenido con la torreta seleccionada
        tNombre.text = torretaSelec.name;
    }
}
