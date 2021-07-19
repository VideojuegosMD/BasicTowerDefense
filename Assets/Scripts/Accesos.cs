using UnityEngine;
using System.Collections.Generic;


public class Accesos : MonoBehaviour
{
    public List<Rigidbody> lObjetos;

    public float gravity;

    //Versión Transform
   /* private void Update()
    {
        foreach (Rigidbody r in lObjetos)
        {
            Vector3 dir = transform.position - r.position;

            r.transform.transform.up= -dir;
            r.transform.position += -r.transform.up * Time.deltaTime;
        }
    }*/

    //Versión de físicas:
    private void FixedUpdate()
    {
        if (lObjetos.Count == 0) { return; }

        foreach(Rigidbody r in lObjetos)
        {
            if (r.GetComponent<BoolControlContacto>().contacto) { continue; }
            Vector3 dir = transform.position - r.position;
          //  r.velocity += new Vector3(0,9.81f,0);
            r.AddForce(dir.normalized * gravity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BoolControlContacto>() != null)
        {
            collision.gameObject.GetComponent<BoolControlContacto>().contacto = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() == null) { return; }
      
        if (!lObjetos.Contains(other.GetComponent<Rigidbody>())) 
        { 
             lObjetos.Add(other.GetComponent<Rigidbody>());
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Rigidbody>() == null) { return; }
        if (!lObjetos.Contains(other.GetComponent<Rigidbody>()))
        {
            lObjetos.Add(other.GetComponent<Rigidbody>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>() == null) { return; }
        if (lObjetos.Contains(other.GetComponent<Rigidbody>()))
        {
            lObjetos.Remove(other.GetComponent<Rigidbody>());
        }
    }
}
