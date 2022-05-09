using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineaAleatoria : MonoBehaviour
{
    //Queda pendiente poner sistema con AnimationCurve
    LineRenderer line;
    public int nPuntos;
   // public AnimationCurve aC;
    public Transform objetivoBase;    
    public float width, height;
    public float difRandomHeight = 5;
    public LayerMask l;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();      
        line.positionCount = nPuntos;

        if(nPuntos <= 0)
        {
            Debug.LogError("OjoCuidado con la linea");
            return;
        }

        //line.SetPosition(0, Vector3.zero);

        //Generación aleatoria
        //Control un poco bobo para que no pongamos un valor de diferencia mayor al de altura permitida
        if (difRandomHeight > height) 
        {
            difRandomHeight = height;
        }

        float cortesX = width / (nPuntos - 1);
        Debug.Log(cortesX);
        Vector3 puntero = transform.position;
        float z=0;
        for (int i = 0; i < nPuntos-1; i++)
        {
           
            //Dividir anchura en tramos para que la linea no se entrecruce            
            if (i == 0)
            {
                z = Random.Range(-height / 2, height / 2) + puntero.z;
            }
            else
            {
                do
                {
                    z = Random.Range(-difRandomHeight, difRandomHeight) + puntero.z;
                }
                while (z >height && z <-height);
            }          

            puntero.z = z;
            line.SetPosition(i, ObtenerPos(new Vector3(puntero.x, 0, puntero.z)) - transform.position);
            puntero.x -= cortesX;
           
        }
        line.SetPosition(nPuntos - 1, objetivoBase.position - transform.position);       
    }

    public Vector3 ObtenerPos(Vector3 posIni)
    {
        Vector3 v = new Vector3();
        RaycastHit rHit;
        posIni += Vector3.up * 100;

        if (Physics.Raycast(posIni,Vector3.down, out rHit, 10000, l))//Podemos diferenciar por Layers.
        {
            //Podemos comprobar nombres, tags
            v = rHit.point;
        }
        else
        {
            Debug.LogWarning("El rayo no ha golpeado ningún objeto: " + posIni);
        }

        return v;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
