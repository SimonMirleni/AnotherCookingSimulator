﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class CantJugadores : NetworkBehaviour
{
    int iCantVR = 0;
    int iCantPC = 0;

    int iLimiteJugadoresVR = 1;
    int iLimiteJugadoresPC = 4;

    public TMP_Text tmpJugadoresVR;
    public TMP_Text tmpJugadoresPC;
    public Button btnVR;
    public Button btnPC;


    public void CheckPlayers(int i)
    {
        switch (i)
        {
            case 0: //0 Representa un click en el btn de VR
                if (iCantVR < iLimiteJugadoresVR)
                {
                    CmdSetPlayerAsVR(); //Le asigno al jugador que presionó el boton un id para luego darle el prefab correcto (En este caso VR)
                    CmdUpdateVRPlayers(); //Actualizo el texto

                    btnVR.enabled = true;
                    btnPC.enabled = false;
                }
                else //Solo se podrá entrar acá si el jugador que eligio VR decide dejar el puesto pulsando el boton de nuevo
                {
                    btnPC.enabled = true;
                }
                break;
            case 1: //1 Representa un click en el btn de PC
                if (false) //If(jugador.status == jugador pc)
                {
                    btnVR.enabled = true;

                }
                else if (iCantPC < iLimiteJugadoresPC) //Si no tiene asignado ningún rol y hay espacio....
                {
                    CmdSetPlayerAsPC();
                    CmdUpdatePCPlayers();

                    btnPC.enabled = true;
                }
                break;
        }
    }

    [Command]
    void CmdSetPlayerAsVR()
    {
        iCantVR += 1;
        //Tengo que asignarle el id y hacer que todos los otros jugadores (menos el player) NO puedan presionar el boton VR
        
        btnVR.enabled = false;

    }

    [Command]
    void CmdSetPlayerAsPC()
    {
        iCantPC += 1;
        //Tengo que asignarle el id y hacer que todos los otros jugadores (menos el player) NO puedan presionar el boton PC
        btnPC.enabled = false;
    }

    [Command]
    void CmdUpdatePCPlayers()
    {
        //Actualizo el texto debajo del boton
        tmpJugadoresPC.text = iCantPC + " / " + iLimiteJugadoresPC;
    }

    [Command]
    void CmdUpdateVRPlayers()
    {
        //Actualizo el texto debajo del boton
        tmpJugadoresVR.text = iCantVR + " / " + iLimiteJugadoresVR;
    }
}
