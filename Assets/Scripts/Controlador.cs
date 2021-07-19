using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador : MonoBehaviour
{
    public int golpesRecibidos;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 15f * Input.GetAxis("Vertical");
        transform.position += transform.right * Time.deltaTime * 15f * Input.GetAxis("Horizontal");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Interactivo")
        {
            golpesRecibidos++;
        }
    }
}
