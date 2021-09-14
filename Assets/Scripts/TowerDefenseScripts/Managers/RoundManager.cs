using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public LineRenderer lr; //Linea a seguir por los enemigos, podría haber varios recorridos según los enemigos.
    public GameObject enemyPrefab; //Enemigo a instanciar.
    public List<AgenteBasic> enemyList; //Lista de enemigos.
    public int indexCount, enemleft, round; //control de indexado, enemigos restantes y ronda.

    public float spawnTime; //Tiempo de spawn.
    float timer; //Contador.
    internal bool onSimulation,spawnActive; //bools de control.
    
       
    private void Start()
    {
        lr = GameObject.FindObjectOfType<LineRenderer>(); //Encontramos la linea para darsela a los enemigos.
        CrearElementos((5 + round*3)); //Creamos tantos enemigos como queramos + multiplicador por ronda. El multiplicador sirve por si guardasemos la ronda activa           
        GameManager.main.tEnemLeft.text = "Enemigos restantes: " + enemleft; //Actualizamos texto.
    }

    private void Update()
    {
        if (onSimulation) //Si estamos en simulación de ataque
        {
            if (spawnActive) //Si faltan enemigos por sacar.
            {
                SpawnEnemies(); //Sacamos enemigos.

                if (indexCount >= enemyList.Count) //Control de número de enemigos a sacar.
                {
                    spawnActive = false;
                }
            }

            if (enemleft <= 0) //Si no hay enemigos restantes la ronda acaba.
            {
                GameManager.main.enemiesDead = true;
            }
        }        
    }

    public void NextRound() //Siguiente ronda
    {
        round++;    
        indexCount = 0; //Reset de indexado
        CrearElementos((5 + round * 3)); //Creamos tantos enemigos como queramos + multiplicador por ronda. El multiplicador sirve por si guardasemos la ronda activa           
        onSimulation = true; //bools de control
        spawnActive = true;          
        SetManagerRound();
    }   
    public void SetManagerRound() //LLamadas de control para el GameManager.
    {
        GameManager.main.playerDead = false;
        GameManager.main.enemiesDead = false;       
    }

    void SpawnEnemies()
    {
        if(timer< Time.timeSinceLevelLoad)//Control contador de spawn.
        {
            enemyList[indexCount].gameObject.SetActive(true); //Activamos el objeto de la lista en base a indexCount.
            timer = Time.timeSinceLevelLoad + spawnTime; //Control del contador para la siguiente instancia.          
            enemyList[indexCount].dead = false; //El enemigo no está muerto.
            enemyList[indexCount].hpPoints = 10 + 2 * round; //Vida del enemigo en base a la ronda.
            enemyList[indexCount].dmg = 10 + 2 * round;//Vida del enemigo en base a la ronda.
            enemyList[indexCount].ResetPos(); //Establecemos posición.            
            indexCount++; //Siguiente enemigo.
        }
    }
    void _ForCrearEnemigos(int objetivo) //Bucle for para crear enemigos en base al parametro de la función.
    {
        for (int i = 0; i < objetivo; i++)
        {
            AgenteBasic g = Instantiate(enemyPrefab, lr.GetPosition(0), Quaternion.identity, transform).GetComponent<AgenteBasic>();
            enemyList.Add(g); //Lo añadimos a la lista.        
            g.line = lr; //Le damos la linea a seguir.
            g.gameObject.SetActive(false); //Desactivamos enemigo.
        }
    }

    void CrearElementos(int cantObj)
    {
        if (enemyList == null) //Si la lista es nula creamos una nueva lista.
        {
            enemyList = new List<AgenteBasic>();
        }
        
        if (enemyList.Count <= 0) //Si la lista está vacía creamos tantos enemigos como indiquemos en el parametro.
        {
            enemleft = cantObj;

            _ForCrearEnemigos(cantObj);
        }
        else if (enemyList.Count<cantObj)//Si la lista ya está creada y es menor que el parametro, la mantenemos y ponemos el resto.
        {
            int enemRes = cantObj - enemyList.Count;

            _ForCrearEnemigos(enemRes);

            enemleft = enemyList.Count;            
        }
        else if(enemyList.Count >= cantObj) //Si la lista está creada y no le falta nada, la dejamos como está y establecemos los enemigos restantes.
        {
            enemleft = enemyList.Count;
        }
    }  
}
