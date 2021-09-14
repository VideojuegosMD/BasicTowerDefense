/*
 Script realizado por Pablo Salas.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Runner : AgenteBasic //Enemigo que se hace mas veloz a cada impacto recibido
{
    private int speedIncrement;

    private void Awake()
    {
        dead = false;
        hpPoints = baseHp;
        dmg = 10;
        agent = GetComponent<NavMeshAgent>();
        speedIncrement = 1;// Incremento de velocidad por cada impacto recibido
        puntosBase = 3;
        agent.speed = 5;

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
        agent.speed += speedIncrement; //Incremento de la velocidad a cada Impacto
        if (hpPoints <= 0)
        {
            hpPoints = 0;
            dead = true;
            agent.speed = 5;//Incicializacion de la velocidad, ya que al resucitar no la resteaba en el AWAKE asi que la reseamos al morir.
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
