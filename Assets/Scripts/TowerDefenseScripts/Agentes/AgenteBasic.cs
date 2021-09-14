/*
 Script editado por Pablo Salas.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgenteBasic : MonoBehaviour
{
    public NavMeshAgent agent; //Referencia del Navmesh
    public LineRenderer line; //Recorrido a seguir, se establece al instanciar el agente.
    [SerializeField]
    internal int nPaso; //Posición de linea.
    public bool finRecorrido;
    public int dmg, hpPoints,baseHp; //Daño y vida, vida base
    public bool dead;
    public int puntosBase = 2;
    bool healing = false;
    public virtual int puntos { get { return puntosBase * Mathf.Clamp(GameManager.main.GetNumRonda(), 1, 15); } }

    private void Awake()
    {
        dead = false;
        hpPoints = baseHp;
        dmg = 10;
        agent = GetComponent<NavMeshAgent>();
  
    }

    //Reset de posición
    public void ResetPos()
    {
        nPaso = 0;      
        agent.transform.position = new Vector3(line.GetPosition(0).x + line.transform.position.x, 0, line.GetPosition(0).z + line.transform.position.z);
        agent.SetDestination(new Vector3(line.GetPosition(0).x + line.transform.position.x, 0, line.GetPosition(0).z + line.transform.position.z));
    }

    // Update is called once per frame
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

    public virtual void SumRestHP(int valor) 
    {
        hpPoints += valor;
        if (hpPoints <= 0)
        {           
            hpPoints = 0;
            dead = true;
           
            GameManager.main.SumConstructPoints(puntos);
            GameManager.main.ActualizarTextoConst();
            GameManager.main.CheckEnemyAlive(this); //Control de enemigos destruidos.       
        }
    }

    public virtual void Move()
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CharcoVerde"))
        {
            agent.speed += 1;
        }

        if (other.CompareTag("CharcoRojo"))
        {
            hpPoints += 5;            
        }
        if (other.CompareTag("Healer"))
        {
            healing = true;
            StartCoroutine(Healing());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Healer"))
        {
           healing=true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Healer"))
        {
            healing = false;
        }
    }
    IEnumerator Healing()
    {
        while(healing)
        {
            yield return new WaitForSeconds(1f);
            hpPoints += 1;
        }
        yield return null;
    }
}
