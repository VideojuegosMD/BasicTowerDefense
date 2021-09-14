/*
 Script realizado por Pablo Salas.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaman : AgenteBasic //Clase que es igual que un AgentBasic pero al morir deja un charco que puede curar o puede aumentar la velocidad
{

    public GameObject charco; // Prefab del charco que va a dejar
    
    private void Awake()
    {        
        dead = false;
        hpPoints = baseHp;
        dmg = 10;
        puntosBase = 3;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!finRecorrido) { Move(); } //Si no ha llegado al final, se mueve
        else
        {
            //Si ha llegado al final lo detemos.
            if (!agent.isStopped)
            {
                agent.isStopped = true;
            }
        }

    }

    public override void SumRestHP(int valor)
    {
        hpPoints += valor;
        if (hpPoints <= 0)
        {
            hpPoints = 0;
            dead = true;
            Instantiate(charco, transform.position, Quaternion.identity); //Instanciamos el charco justo en el momento de su muerte
            GameManager.main.SumConstructPoints(puntos);
            GameManager.main.ActualizarTextoConst();
            GameManager.main.CheckEnemyAlive(this); //Control de enemigos destruidos.     
           
        }
    }
    
    public override void Move()
    {
        //Vector para la posición a seguir, cuidado con la posición de la linea, componer vector de la suma de las posiciones y la posicion global.
        Vector3 nextPos = new Vector3(line.GetPosition(nPaso).x + line.transform.position.x, 0, line.GetPosition(nPaso).z + line.transform.position.z);

        if (Vector3.Distance(transform.position, nextPos) < 0.26f) //Si la distancia al punto es menos de X.
        {
            if (nPaso + 1 >= line.positionCount) //Final de recorrido.
            {
                finRecorrido = true;
            }
            else { nPaso++; }        //Siguiente paso.
        }
        agent.SetDestination(nextPos); //Movemos al agente.
    }
}

