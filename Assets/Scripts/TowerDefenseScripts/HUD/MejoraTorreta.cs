using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MejoraTorreta : MonoBehaviour
{

    public AnimationCurve aC;
    public ConstructorManager constructorManager;
    public TorretaBasic torretaSelec;
    public Text tNombre, tDmg, tCadencia, tVel, tRango, tNivel, tValor, tCoste;
    public Image img;

    public int costeMejora, incremento;

    public float iDmg,iLvl,iLvlMax=100;
    public float fCadencia, fVel, fRango; //variables para incrementar los stats

    // Start is called before the first frame update
    void Start()
    {
       /* iDmg = 0;
        fCadencia = 101;
        fVel = 0;
        fRango = 0;
        iLvl = 0;*/
        iLvlMax = 100;
        incremento = 0;
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(costeMejora);
    }
    #region FuncionesParaBotones
    public void VenderTorreta()
    {
        //Consultar el precio de la torreta,
        //Mandar el precio a el manager de puntos.
        //eliminar la torreta.
        //Borrar icono
        //cerrar panel de mejora.
        constructorManager.OperateConstructPoints(torretaSelec.cost);
        constructorManager.mT.DestroyTorreta(torretaSelec);
        this.gameObject.SetActive(false);
    }
    public void ConfirmarVenta()
    {
        PopUp.main.ActiDeactPopUp();

        PopUp.main.ChangeQuestion("¿Estás seguro de que quieres vender la torreta?");

        PopUp.main.ChangeButtonText("Sí", "No");

        PopUp.main.ChangeButtonAction(true, delegate { PopUp.main.ActiDeactPopUp(); PopUp.main.RemoveActions(); VenderTorreta(); });
        PopUp.main.ChangeButtonAction(false, delegate { PopUp.main.ActiDeactPopUp(); PopUp.main.RemoveActions(); });
    }

    public void ConfirmarCambios()
    {  
        if (costeMejora > constructorManager.constructPoints)
        {
            Debug.Log("No puedes aplicar la mejora, faltan puntos.");
            return;
        }

        PopUp.main.ActiDeactPopUp();

        PopUp.main.ChangeQuestion("¿Estás seguro de que quieres aplicar la mejora?");

        PopUp.main.ChangeButtonText("Sí", "No");

        PopUp.main.ChangeButtonAction(true, delegate { PopUp.main.ActiDeactPopUp();PopUp.main.RemoveActions(); AplicarCambios(); });
        PopUp.main.ChangeButtonAction(false, delegate { PopUp.main.ActiDeactPopUp(); PopUp.main.RemoveActions(); });
    }
    public void AplicarCambios()
    {
        constructorManager.OperateConstructPoints(-costeMejora);
        torretaSelec.dmg = (int)iDmg;
        torretaSelec.nivel = (int)iLvl;
        torretaSelec.fireRate = fCadencia;
        torretaSelec.bulletSpeed = fVel;
        torretaSelec.rango = fRango;
        torretaSelec.GetComponent<SphereCollider>().radius = fRango + torretaSelec.rangoBase;
        torretaSelec.cost = costeMejora;
        torretaSelec.incremento = incremento;
        tNivel.text = "Nivel de torreta:" + "\n" + torretaSelec.nivel.ToString();
        tValor.text = "Valor de torreta:" + "\n" + torretaSelec.cost.ToString();
        tCoste.text = "Coste de cambios:" + "\n" + 0;
        ResetValues();
    }
    public void ResetValues()
    {
        costeMejora = 0;       
    }
    void CalculoCoste(int i, float fValor)
    {
        if (i < 0)//num negativo / quitar valores
        {
            incremento--;           
        }
        else //num positivo / poner valores
        {
            incremento++;           
        }
        costeMejora = Mathf.RoundToInt((aC.Evaluate(iLvl / iLvlMax) + fValor)) * incremento;
        Debug.Log(incremento);
    }
    public void CalcMejoraDmg(int i)
    {
        if (iLvl + i <= iLvlMax && iLvl + i >= 0)
        {           
            iDmg += i;            
            tDmg.text = iDmg.ToString();
            iLvl += i;
            tNivel.text = "Nivel de torreta:" + "\n" + iLvl.ToString();

            CalculoCoste(i, iDmg);

            //Debug.Log(Mathf.RoundToInt(aC.Evaluate(iLvl / iLvlMax)));

            tCoste.text = "Coste de cambios:" + "\n" + costeMejora.ToString();
        }
        else if(iLvl + i > iLvlMax)
        {
            Debug.Log("No se puede mejorar más esta torreta.");
        }
        else
        {
            Debug.Log("No se puede disminuir el nivel de la torreta.");
        }
    }
    public void CalcMejoraCad(int i)
    {        
        if (iLvl + i <= iLvlMax && iLvl + i >= 0)
        {
            fCadencia -= i/101f ;
            Debug.Log(i / 101f);
            tCadencia.text = fCadencia.ToString("0.00");
            iLvl += i;
            tNivel.text = "Nivel de torreta:" + "\n" + iLvl.ToString();

            CalculoCoste(i, fCadencia/fCadencia);

            //Debug.Log(Mathf.RoundToInt(aC.Evaluate(iLvl / iLvlMax)));

            tCoste.text = "Coste de cambios:" + "\n" + costeMejora.ToString();
        }
        else if (iLvl + i > iLvlMax)
        {
            Debug.Log("No se puede mejorar más esta torreta.");
        }
        else
        {
            Debug.Log("No se puede disminuir el nivel de la torreta.");
        }
    }
    public void CalcMejoraVel(int i)
    {
        if (iLvl + i <= iLvlMax && iLvl + i >= 0)
        {
            fVel += i*10;
            tVel.text = fVel.ToString();
            iLvl += i;
            tNivel.text = "Nivel de torreta:" + "\n" + iLvl.ToString();

            CalculoCoste(i, fVel/1000);

            Debug.Log(Mathf.RoundToInt(aC.Evaluate(iLvl / iLvlMax)));

            tCoste.text = "Coste de cambios:" + "\n" + costeMejora.ToString();
        }
        else if (iLvl + i > iLvlMax)
        {
            Debug.Log("No se puede mejorar más esta torreta.");
        }
        else
        {
            Debug.Log("No se puede disminuir el nivel de la torreta.");
        }
    }
    public void CalcMejoraRango(int i)
    {
        if (iLvl + i <= iLvlMax && iLvl + i >= 0)
        {
            fRango += i;
            tRango.text =  fRango.ToString();
            iLvl += i;
            tNivel.text = "Nivel de torreta:" + "\n" + iLvl.ToString();

            CalculoCoste(i, fRango*10);

            Debug.Log(Mathf.RoundToInt(aC.Evaluate(iLvl / iLvlMax)));

            tCoste.text = "Coste de cambios:" + "\n" + costeMejora.ToString();
        }
        else if (iLvl + i > iLvlMax)
        {
            Debug.Log("No se puede mejorar más esta torreta.");
        }
        else
        {
            Debug.Log("No se puede disminuir el nivel de la torreta.");
        }
    }
    #endregion


    public void UpdateContent()
    {
        //Actualización de variables con la torreta seleccionada

        iDmg = torretaSelec.dmg;
        iLvl = torretaSelec.nivel;
        fCadencia = torretaSelec.fireRate;
        fVel = torretaSelec.bulletSpeed;
        fRango = torretaSelec.rango;
        incremento = torretaSelec.incremento;
        //Actualización de textos con la torreta seleccionada
        tNombre.text = torretaSelec.name;
        tDmg.text = torretaSelec.dmg.ToString();
        tCadencia.text = torretaSelec.fireRate.ToString();
        tVel.text = torretaSelec.bulletSpeed.ToString();
        tRango.text = torretaSelec.rango.ToString();
        tNivel.text = "Nivel de torreta:" + "\n" + torretaSelec.nivel.ToString();
        tValor.text = "Valor de torreta:" + "\n" + torretaSelec.cost.ToString();
        tCoste.text = "Coste de cambios:" + "\n" + 0;
        img.sprite = torretaSelec.icon;
    }
}
