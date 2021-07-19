using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceManager : MonoBehaviour //PoolManager
{
    public GameObject prefabBala; //prefab de la bala, si hubiese distintos proyectiles podemos tener distintas listas o bien tratar todas como una sola.
    public List<GameObject> lG; //Lista de objetos instanciados
    public int cantObj=25; //Objetos por defecto.
    public static InstanceManager main; //Singleton.
    // Start is called before the first frame update
    void Awake()
    {
        if(main == null)
        {
            main = this; //Asignación del Singleton.
        }
    }
    private void Start()
    {
        CrearElementos(); //Creamos los objetos que necesitaremos en un principio.
    }

    void CrearElementos() //Bucle de instancia en base a num entero cantObj.
    {
        for(int i = 0; i<cantObj; i++)
        {
            GameObject g = Instantiate(prefabBala, transform);
            lG.Add(g); //Lo metemos en la lista.
            g.SetActive(false); //Desactivamos el objeto.
        }      
    }
    public GameObject GetItem() //Método para devolver objetos desactivados o crearlos.
    {
        foreach(GameObject g in lG)
        {
            if (!g.activeSelf) { return g; } //devolvemos objeto desactivado.
        }
        GameObject h = Instantiate(prefabBala, transform); //Si no hemos devuelto ningún objeto, instanciamos uno y lo devolvemos.
        lG.Add(h);
        h.SetActive(false);
        return h;
    }

    public IEnumerator IDesactivarObj(GameObject g) //Corrutina para desactivar objetos en 5s; se puede quitar, se usa en otra escena distinta.
    {
        yield return new WaitForSeconds(5f);
        if (g.activeSelf)
        {
            g.SetActive(false);
        }
    }
}
