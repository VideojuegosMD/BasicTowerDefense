/*
 Script realizado por Pablo Salas.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tank : AgenteBasic //Enemigo mas lento y con mas vida, tiene Armadura y con cada impacto esta ira aumentando, siempre recibira minimo 1 Punto de daño
{
    private int armor; //Armadura para reducir el daño

    private void Awake()
    {
        dead = false;
        hpPoints = baseHp;
        dmg = 10;
        agent = GetComponent<NavMeshAgent>();
        armor = 0;
        puntosBase = 5;

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
        int damage = valor + armor; //Calculamos el daño a aplicar

        if (damage > -1) //Si la armadura reduce el daño a 0, aplicamos el daño minimo de 1
        {
            damage = -1;
        }
        hpPoints += damage;
        armor++;//Incrementamos la armadura en 1 por cada impacto
        
        
        if (hpPoints <= 0)
        {
            hpPoints = 0;
            dead = true;            
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
