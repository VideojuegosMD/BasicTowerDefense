using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorretaBasic : MonoBehaviour
{    
    public Sprite icon;
    public GameObject balaPrefab; //Proyectil a disparar.
    public Transform canon,spawnPoint; //Estructura de torre.
    public List<Collider> enemies; //Lista de enemigos
    public float fireRate, bulletSpeed; //Velocidad de disparo y bala.
    float timer; //Contador.
    public int cost, dmg, nivel; // Coste de torreta y daño por bala.
    public bool chorrazo; //Efecto curioso.
    bool oneShot; //Control de un solo disparo en Update.  
    public float rango;
    SphereCollider colRango;

    public void Start()
    {
        colRango = GetComponent<SphereCollider>();
    }

    public void SetRango(float r)
    {
        rango = r;
        colRango.radius = r;
    }

    private void Update()
    {
        if(!(enemies.Count<=0))  //Si hay objetos en la lista
        {          
            if (!enemies[0].gameObject.activeInHierarchy) //Si el primer enemigo está desactivado/muerto
            {
                enemies.Remove(enemies[0]); //Lo quitamos de la lista.
                return;
            }

            Vector3 dir = enemies[0].transform.position - canon.position; //Apuntamos al enemigo desde el cañon

            //Debug.Log("Tamaño de vector: " + dir.magnitude);
            dir = dir.normalized;
            Debug.Log("Tamaño de vector: " + dir.magnitude);
            canon.forward = dir; //Apuntamos el cañon.
            

            if(Time.timeSinceLevelLoad >= timer && (!oneShot || chorrazo)) //Control de disparo
            {                
                oneShot = true; 
                GameObject shotInstance = InstanceManager.main.GetItem(); //Disparo de bala por ObjectPooling.
                shotInstance.SetActive(true);
                shotInstance.transform.position = spawnPoint.position;
                shotInstance.GetComponent<Rigidbody>().AddForce(bulletSpeed * dir,ForceMode.Impulse);
              
            }

            //Restamos vida al enemigo, también se puede hacer por colisiones pero podemos perder disparos.
            if (Time.timeSinceLevelLoad >= timer +0.08f) 
            {
                timer = fireRate + Time.timeSinceLevelLoad; //Reset de contador
                if (!enemies[0].GetComponentInParent<AgenteBasic>().dead) //Si el enemigo no está muerto.
                {
                    enemies[0].GetComponentInParent<AgenteBasic>().SumRestHP(-dmg); //Restar vida al primer enemigo.
                }
                    oneShot = false; //reset de control de disparo.
            }
        }        
    }

    //OnTrigger controla la lista de enemigos que entra y sale, mucho cuidado con la disposición de colliders en las torretas.

    private void OnTriggerEnter(Collider other)
    {       
        if (other.tag == "Enemigo")
        {           
            if (!enemies.Contains(other) && other.gameObject.activeInHierarchy)
            {                
                enemies.Add(other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemigo")
        {
            if (enemies.Contains(other))
            {
                enemies.Remove(other);               
            }
        }
    }
    

}
