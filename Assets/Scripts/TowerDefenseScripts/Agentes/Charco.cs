/*
 Script realizado por Pablo Salas.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charco : MonoBehaviour //Area que se autodestruye al cierto tiempo y que segun el TAG que se le haya dado puede curar o puede aumentar la velocidad
{
    public float lifeTime; //Tiempo para la autodestruccion, configurado en el editor a 3
    private void Start()
    {
        StartCoroutine(AutoDestruction());    
    }
    IEnumerator AutoDestruction() //El objeto se autodestruye a los X segundos de haberse creado
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }
}
