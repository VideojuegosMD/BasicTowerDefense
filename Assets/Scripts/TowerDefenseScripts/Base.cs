using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    internal int vida,vidaMax=100;
    
    public bool gotDMG;
    

    // Start is called before the first frame update
    void Awake()
    {
        vida = vidaMax;       
    }

    // Update is called once per frame
    void Update()
    {
        if (gotDMG)
        {
           GameManager.main.tVida.text ="Puntos de vida: " +vida+ "/"+vidaMax; //Actualizar texto desde Singleton.          
           gotDMG = false;
            if (vida <= 0){ GameManager.main.playerDead = true; } //Comprobamos si hemos perdido.
        }
    }
    public void CalcHP(int calcHP)
    {
        vida += calcHP;
        vida = Mathf.Clamp(vida, 0, vidaMax);
        Debug.Log("Base a " + vida + " de vida.");
    }

    void OnTriggerEnter(Collider other) //Control de llegada de ensemigos.
    {
        Debug.Log(other.name);
        if (other.tag=="Enemigo")
        {           
            if (other.GetComponentInParent<AgenteBasic>() != null)
            {
                AgenteBasic ab = other.GetComponentInParent<AgenteBasic>(); //Acceso a la instancia de enemigo
                CalcHP(-ab.dmg); //Restamos la vida correspondiente al daño del enemigo.
                gotDMG = true; 
                GameManager.main.EnemyGotBase(ab);

            }
        }
    }
}
