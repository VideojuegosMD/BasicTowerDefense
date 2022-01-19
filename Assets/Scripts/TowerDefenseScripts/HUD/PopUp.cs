using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PopUp : MonoBehaviour
{
    //En el popup  estamos aplicando varias veces las funciones de ChangeButtonAction, se solapan y no se borran los delegados.
    
    public static PopUp main;
    public Text question;
    public Button yes, no;

    private void Awake()
    {
        main = this;
        main.gameObject.SetActive(false);
    }
    public void ChangeQuestion(string text)
    {
        question.text = text;
    }
    public void ChangeQuestion(string text, int textSize)
    {
        question.text = text;
        question.fontSize = textSize;
    }

    public void ChangeButtonText(string sButtonYes, string sButtonNo)
    {
        yes.transform.GetComponentInChildren<Text>().text = sButtonYes;
        no.transform.GetComponentInChildren<Text>().text = sButtonNo;
    }

    public void RemoveActions()
    {
        yes.onClick.RemoveAllListeners();
        no.onClick.RemoveAllListeners();
    } 

    public void ChangeButtonAction(bool b, UnityAction uE)
    {
        if (b)
        {
            yes.onClick.AddListener(uE);
        }
        else
        {
            no.onClick.AddListener(uE);
        }
        
    }

    public void ChangeButtonAction(Button b, UnityAction uE)
    {
        b.onClick.AddListener(uE);
    }

    public void ActiDeactPopUp()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    } 
}
