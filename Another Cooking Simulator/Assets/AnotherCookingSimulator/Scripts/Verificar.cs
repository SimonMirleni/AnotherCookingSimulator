﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Verificar : MonoBehaviour
{
    GameObject Bandeja;
    private bool noSeHizo;
    

    // Start is called before the first frame update
    void Start()
    {
        noSeHizo = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    // Cuando un pati toca el horno se cambia a true el bool::touchGrill
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Verificacion")
        {
            noSeHizo = true;
            VRTK_SnapDropZone snapDropZone = this.GetComponentInChildren<VRTK_SnapDropZone>();
            if (ObtenerHijos(snapDropZone).Count != 0)
            {
                if (noSeHizo)
                {
                    List<VRTK_InteractableObject> hijos = ObtenerTodosLosHijos(snapDropZone);
                    List<string> listaNombres = new List<string>();
                    noSeHizo = false;
                    string strIngredientes = "";
                    for(int i = 0; i < hijos.Count; i++)
                    {
                        strIngredientes += hijos[i].name + " ";
                        listaNombres.Add(hijos[i].name);
                    }
                    int[] interpretacionDeVR = new int[listaNombres.Count];
                    for (int i = 0; i < listaNombres.Count; i++)
                    {
                        if (listaNombres[i].Contains("Pan"))
                        {
                            interpretacionDeVR[i] = 0;
                        }else if (listaNombres[i].Contains("Paty"))
                        {
                            interpretacionDeVR[i] = 1;
                        }else if (listaNombres[i].Contains("Cheddar"))
                        {
                            interpretacionDeVR[i] = 2;
                        }else if (listaNombres[i].Contains("Cebolla"))
                        {
                            interpretacionDeVR[i] = 3;
                        }else if (listaNombres[i].Contains("Bacon"))
                        {
                            interpretacionDeVR[i] = 4;
                        }
                    }
                    PedidoManager.instancePedidoManager.agarrarUltimoPedido().SetInterpretacionIngredientes(interpretacionDeVR);
                    ClientesManager.instanceClientesManager.seEntregoUnPedido();
                    PedidoManager.instancePedidoManager.MostrarVerificacion(interpretacionDeVR);
                    PedidoManager.instancePedidoManager.cambiarPuntaje();
                    
                    
                    strIngredientes = "";
                }
            }
        }
    }

    List<GameObject> listaHamburguesa(GameObject go)
    {
        List<GameObject> listaHamburguesa = new List<GameObject>();
        foreach (Transform child in go.transform)
        {
            listaHamburguesa.Add(child.GetComponent<GameObject>());
        }
        return listaHamburguesa;
    }
    private List<VRTK_InteractableObject> ObtenerTodosLosHijos(VRTK_SnapDropZone snapDropZone)
    {
        List<VRTK_InteractableObject> hijos = new List<VRTK_InteractableObject>();
        VRTK_SnapDropZone objectParaBuscarHijos = snapDropZone;
        bool tieneHijos = true;
        do
        {
            if (ObtenerHijos(objectParaBuscarHijos).Count != 0)
            {
                hijos.Add(ObtenerHijos(objectParaBuscarHijos)[0]);
                VRTK_SnapDropZone nuevoObject = ObtenerHijos(objectParaBuscarHijos)[0].GetComponentInChildren<VRTK_SnapDropZone>();
                objectParaBuscarHijos = nuevoObject;
            }
            else
            {
                tieneHijos = false;
            }
        } while (tieneHijos == true);
        return hijos;
    }
    private List<VRTK_InteractableObject> ObtenerHijos(VRTK_SnapDropZone objeto)
    {
        List<VRTK_InteractableObject> hijos = new List<VRTK_InteractableObject>();
        foreach (Transform child in objeto.transform)
        {
            if (child.name != "HighlightContainer")
            {
                hijos.Add(child.GetComponent<VRTK_InteractableObject>());
            }
        }
        return hijos;
    }
}
