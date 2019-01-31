using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonRoom : MonoBehaviour
{
    public static PhotonRoom room;

    private PhotonView pV;

    public bool isGameLoaded;
    public int currentScene;

    private Player[] photonPlayers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
