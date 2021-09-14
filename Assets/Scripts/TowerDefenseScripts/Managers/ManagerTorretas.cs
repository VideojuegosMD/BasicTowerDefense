using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerTorretas : MonoBehaviour
{
    public List<TorretaBasic> listaTorretas; //Lista de torretas
    public GameObject prefabBoton;
    public GameObject buttonHolder;

    public void AddTorreta(TorretaBasic t)
    {
        listaTorretas.Add(t);
        CreateButton(t);
    }

    public void CreateButton(TorretaBasic t)
    {
        GameObject gButton = Instantiate(prefabBoton,buttonHolder.transform);
        gButton.GetComponent<Image>().sprite = t.icon;

    }

}
