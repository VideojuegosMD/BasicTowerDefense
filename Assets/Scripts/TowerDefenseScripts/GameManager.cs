using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager main;
    public Text tRonda, tVida, tConstP, tEnemLeft;
    RoundManager roundM;
    ConstructorManager constM;
    Base playerBase;
    bool onEdit, onSimul;
    public bool playerDead, enemiesDead;

    public void CheckEnemyAlive(AgenteBasic aB)
    {     
        roundM.enemleft--;
        aB.gameObject.SetActive(false);
        tEnemLeft.text = "Enemigos restantes: " + roundM.enemleft; 
    }
    public void EnemyGotBase(AgenteBasic aB)
    {
       roundM.enemleft--;
        aB.dead = true;
       aB.gameObject.SetActive(false);
       tEnemLeft.text = "Enemigos restantes: " + roundM.enemleft;
    }


    public void Iniciar()
    {
        playerBase = GameObject.FindObjectOfType<Base>();
        roundM = transform.GetComponentInChildren<RoundManager>();
        constM = transform.GetComponentInChildren<ConstructorManager>();
        onEdit = true;
        onSimul = false;
        constM.constructMode = onEdit;
        roundM.onSimulation = onSimul;
    }
    public void ActualizarTextos()
    {        
        tVida.text ="Puntos de vida: " + playerBase.vida + "/" + playerBase.vidaMax;
        tRonda.text = "Número de ronda: " + roundM.round;
        tEnemLeft.text = "Enemigos restantes: " + roundM.enemleft;
        tConstP.text = "Puntos de construcción: " + constM.constructPoints;
    }
    public void ActualizarTextoConst()
    {
        tConstP.text = "Puntos de construcción: " + constM.constructPoints;
    }
   
    private void Awake()
    {
        if (main == null) 
        {
            main = this;
        }
    }

    void Start()
    {
        Iniciar();
        ActualizarTextos();
    }

    private void Update()
    {
        CheckEnd();
    }

    public void CheckEnd()
    {
        if (playerDead)
        {
            Debug.Log("Perdiste.");
            //Hacer cosas de hacer
        }
        else if (enemiesDead)
        {
            Debug.Log("Ganaste.");
            roundM.NextRound();
            //constM.constructPoints += 10 + roundM.round * 10; //Puntos final de ronda
            ActualizarTextos();
            Round(true);
        }
    }
    public void SumConstructPoints(int x)
    {
        constM.constructPoints += x;
    }
    public int GetNumRonda()
    {
        return roundM.round;
    }

    public void BComenzar()
    {
        if (onSimul) { return; }
        Round(false);
    }

    void Round(bool editMode)    
    {        
        onEdit = editMode;
        onSimul = !editMode;
        constM.constructMode = onEdit;
        roundM.onSimulation = onSimul;
        if (onSimul)
        {
            roundM.spawnActive = true;
        }
    }
    
}
