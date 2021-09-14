/*
 Script realizado por Pablo Salas.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : AgenteBasic //Enemigo que resucita una vez al cierto tiempo
{
    private int ariseTime; //Tiempo que tarda en resucitar
    public bool canDie; //Booleano que indica si puede morir o no, empieza en false, si muere una vez se pone a true, si esta a true, muere y no resucita
    private MeshRenderer meshR; //Componente Meshrenderer de la capsula
    private CapsuleCollider capsule;//Collider de la capsula
    private void Awake()
    {
        ariseTime = 3;
        dead = false;
        hpPoints = baseHp;
        dmg = 10;
        puntosBase = 3;
        agent = GetComponent<NavMeshAgent>();
        meshR = GetComponentInChildren<MeshRenderer>();
        capsule = GetComponentInChildren<CapsuleCollider>();
        canDie = false;
       

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
            if(canDie)//Si puede morir, muere normal;
            {
                GameManager.main.SumConstructPoints(puntos);
                GameManager.main.ActualizarTextoConst();
                GameManager.main.CheckEnemyAlive(this); //Control de enemigos destruidos.     
                canDie = false;
            }
            else//Si no puede morir iniciamos Coroutine de Resurreccion,
            {
                StartCoroutine(Resurrection());
            }
            
        }
    }
    IEnumerator Resurrection()//Coroutina de resureccion
    {
        agent.isStopped = true; //Paramos al agente
        meshR.gameObject.SetActive(false); //Lo hacemos invisible
        capsule.isTrigger = true; //Lo hacemos atravesable
        yield return new WaitForSeconds(ariseTime); //Esperamos el tiempo indicado para resucitar
        capsule.isTrigger = false; //Activamos collider
        meshR.gameObject.SetActive(true); //Lo hacemos visible
        agent.isStopped = false; //Lo ponemos en marcha
        hpPoints = 5; // Le asignamos una vida
        dead = false; //Le indicamos que no esta muerto
        canDie = true; //Indicamos que puede morir

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
