using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CamsGame : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera[] virtualCameras;

    private void Start()
    {
        GameManager.instance.OnCamerasInit += InitCameras;
    }

    private void InitCameras()
    {
        for (int i = 0; i < GameManager.instance.PlayersInGame.Count; i++)
        {
            if (!GameManager.instance.PlayersInGame[i].HasBomb)
            {
                virtualCameras[i].LookAt = GameManager.instance.PlayersInGame[i].transform; 
            }
            else
            {
                virtualCameras[i] = virtualCameras[virtualCameras.Length - 1];
                virtualCameras[i].LookAt = GameManager.instance.PlayersInGame[i].transform;
            }
        }
    }
}
