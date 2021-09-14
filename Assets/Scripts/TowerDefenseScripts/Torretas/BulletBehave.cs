using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehave : MonoBehaviour
{
    bool corrutinaFuncionando;
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine("IWaitAndHide"); //Al chocar llamamos a la corrutina.
    }

    //Corrutina para dejar que la bala choque y haga cosas, podría ser mejor pasarlo a Update con un contador simple.
    IEnumerator IWaitAndHide()
    {
        corrutinaFuncionando = true;
        yield return new WaitForSeconds(1f);       
        GetComponent<Rigidbody>().velocity = Vector3.zero; //Muy importante detener la velocidad.
        corrutinaFuncionando = false;
        gameObject.SetActive(false); //Desactivamos la bala.
    }
}
