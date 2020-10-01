﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using System;

public class RoomManagerScript : NetworkBehaviour
{
    #region VARIABLES
    private NetworkRoomManager myNetworkRoomManager;
    private static List<NetworkRoomPlayer> listOfPlayers;
    NetworkRoomPlayer myPlayer;
    GameObject aPlayer;

    [Header("Player's Canvas")]
    [SerializeField]
    GameObject player1;
    [SerializeField]
    GameObject player2;
    [SerializeField]
    GameObject player3;
    [SerializeField]
    GameObject player4;
    [SerializeField]
    GameObject player5;

    [Header("Botones")]
    public Button btnVR;
    public Button btnPC;
    public Button btnStart;

    [Header("TMP_Text")]
    public TMP_Text tmpJugadoresVR;
    public TMP_Text tmpJugadoresPC;

    //[SyncVar(hook = nameof(CmdOnCountVrChange))]
    public int iCantVR = 0;

   //[SyncVar(hook = nameof(CmdOnCountPCChange))]
    public int iCantPC = 0;

    public int iLimiteJugadoresVR = 1;
    public int iLimiteJugadoresPC = 4;

    #endregion

    #region METODOS MOSTRAR JUGADORES
    // Start is called before the first frame update
    void Awake()
    { //Puede que tenga que mover la funcion de Awake al ClientConnect (o sea, editar el NetworkRoomManager y que en OnCLientConnect o algo así se llame a ShowPlayers(), un metodo publico que puedo crear dentro de esta funcion
        myNetworkRoomManager = FindObjectOfType<NetworkRoomManager>();

        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
        player5.SetActive(false);

        btnStart.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        listOfPlayers = myNetworkRoomManager.roomSlots;

        int[] cantPlayers = myNetworkRoomManager.ObtenerCantJugadoresPorEquipo();
        iCantVR = cantPlayers[0];
        iCantPC = cantPlayers[1];

        UpdateCanvas();
        UpdatePlayerTypeCanvas();
    }

    private void UpdateCanvas()
    {
        int num = 1;

        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
        player5.SetActive(false);

        foreach (NetworkRoomPlayer p in listOfPlayers)
        {
            switch (num)
            {
                case 1:
                    aPlayer = player1;
                    break;
                case 2:
                    aPlayer = player2;
                    break;
                case 3:
                    aPlayer = player3;
                    break;
                case 4:
                    aPlayer = player4;
                    break;
                case 5:
                    aPlayer = player5;
                    break;
                default:

                    break;

            }

            aPlayer.GetComponentInChildren<TMP_Text>().SetText(p.index.ToString());

            //myPlayer.gameObject.transform.GetChild(0); --> 0 = fondo, 1 = nombre, 2 = ready
            aPlayer.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().SetText(p.netId.ToString());

            string isReady;
            if (p.readyToBegin == true)
            {
                isReady = "READY!";
            }
            else
            {
                isReady = "NOT READY";
            }
            aPlayer.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().SetText(isReady);

            aPlayer.SetActive(true);


            //Obtengo MI jugador
            if (p.isLocalPlayer)
            {
                myPlayer = p;
                aPlayer.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
            }

            num++;
        }

    }

    [Client]
    public void SetReadyState()
    {
        myPlayer.CmdCUSTOMChangeReadyState(!myPlayer.readyToBegin); //Revisar NetworkRoomPlayer para ver funcion, evita que inicie el juego de forma automatica
    }

    [Server]
    public void StartGame() // --> Solo el host puede correr este script
    {
        myNetworkRoomManager = FindObjectOfType<NetworkRoomManager>();
        myNetworkRoomManager.CheckReadyToBegin();
    }

    #endregion

    #region SELECCION DE EQUIPO

    //Glosario playerType:
    //0 = VR
    //1 = PC
    //2 = undefined --> No eligió aún

   [Client]
    public void SetPcPlayer()
    {
        if(myPlayer.playerType == 1)  //Cuando deselecciona el tipo pc
        {
            iCantPC--;
            myPlayer.CmdChangePlayerType(2);
            UpdateButtonsStatus();
            return;
        }
        if(iCantPC < iLimiteJugadoresPC)
        {
            if (myPlayer.playerType == 2)
            {
                iCantPC++;
            }
            else //No debería poder entrar por el momento
            {
                iCantPC++;
                iCantVR--;
            }

            myPlayer.CmdChangePlayerType(1); //myPlayer pasa a ser PC
        }
    }

    [Client]
    public void SetVrPlayer()
    {
        if (myPlayer.playerType == 0)
        {
            iCantVR--;
            myPlayer.CmdChangePlayerType(2);
            UpdateButtonsStatus();
        }
        else if (iCantVR < iLimiteJugadoresVR)
        {

            myPlayer.CmdChangePlayerType(0); //myPlayer pasa a ser VR
            iCantVR++;
        }
    }
   
    public void UpdateButtonsStatus() //Actualiza para cada persona los botones a activar
    {
        if(myPlayer == null)
        {
            return;
        }
        switch (myPlayer.playerType)
        {
            case 0: //VR
                btnVR.interactable = true;
                btnPC.interactable = false;
                break;
            case 1: //PC
                btnVR.interactable = false;
                btnPC.interactable = true;
                break;
            case 2: //No tiene clase
                if (iCantPC < iLimiteJugadoresPC)
                {
                    btnPC.interactable = true;
                }
                else
                {
                    btnPC.interactable = false;
                }

                if (iCantVR < iLimiteJugadoresVR)
                {
                    btnVR.interactable = true;
                }
                else
                {
                    btnVR.interactable = false;
                }
                break;
        }
    }


    public void UpdatePlayerTypeCanvas()
    {
        tmpJugadoresPC.text = iCantPC + " / " + iLimiteJugadoresPC;
        tmpJugadoresVR.text = iCantVR + " / " + iLimiteJugadoresVR;

        UpdateStartButton();

        UpdateButtonsStatus();
    }

    private void UpdateStartButton()
    {
        if(iCantVR > 0 || iCantPC > 0)
        {
            btnStart.interactable = true;
        }
        else
        {
            btnStart.interactable = false;
        }
    }
    #endregion
}