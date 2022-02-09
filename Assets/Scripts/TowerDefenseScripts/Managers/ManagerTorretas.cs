using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerTorretas : MonoBehaviour
{
    public List<TorretaBasic> listaTorretas; //Lista de torretas
    public List<Button> listaBotones;
    public GameObject prefabBoton;
    public GameObject buttonHolder;
    public MejoraTorreta panelMejora;

    public void AddTorreta(TorretaBasic t)
    {
        listaTorretas.Add(t);
        CreateButton(t);
    }

    public void CreateButton(TorretaBasic t)
    {
        GameObject gButton = Instantiate(prefabBoton,buttonHolder.transform);
        gButton.GetComponent<Image>().sprite = t.icon;
        gButton.GetComponent<Button>().onClick.AddListener(() => SelectTower(t));

        listaBotones.Add(gButton.GetComponent<Button>());
    }

    public void DestroyTorreta(TorretaBasic t)
    {
        int x = listaTorretas.IndexOf(t);
        Destroy(listaBotones[x].gameObject);
        Destroy(listaTorretas[x].gameObject);
        listaTorretas.Remove(t);
        listaBotones.RemoveAt(x);
    } 

    public void SelectTower(TorretaBasic t)
    {
        if (!panelMejora.gameObject.activeSelf)
        {
            panelMejora.gameObject.SetActive(true);
        }
        panelMejora.torretaSelec = t;
        panelMejora.UpdateContent();
    }

}
