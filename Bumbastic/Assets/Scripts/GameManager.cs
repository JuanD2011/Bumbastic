using Photon.Realtime;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    List<Bummie> playersInGame;
    public Bummie bombHolder;
    public GameObject bomb;

    public List<Bummie> PlayersInGame { get => playersInGame; set => playersInGame = value; }

    private PhotonView pV;

    Vector3 crowPos;
    public Vector3 CrowPos { get => crowPos; set => crowPos = value; }

    private void Start()
    {
        pV = GetComponent<PhotonView>();

        PlayersInGame = new List<Bummie>();

        Invoke("FillList", 1f);

        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Soy yo");
            Invoke("GiveBomb", 1.5f);
        }
    }

    [PunRPC]
    void WhoHasTheBomb(int _bombHolderID, int bombID)
    {
        bomb = PhotonView.Find(bombID).gameObject;
        bombHolder = PhotonView.Find(_bombHolderID).gameObject.GetComponent<Bummie>();
        bombHolder.HasBomb = true;
        bomb.transform.SetParent(bombHolder.transform);
        bomb.gameObject.transform.position = bombHolder.transform.GetChild(1).transform.position;
        bomb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        Debug.Log("Holi");
    }

    public void GiveBomb()
    {
        Debug.Log(PlayersInGame.Count);
        if (PlayersInGame.Count > 1)
        {
            int spawnPicker = Random.Range(0, PlayersInGame.Count);
            bomb = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bomb Variant"), PlayersInGame[spawnPicker].transform.position + new Vector3(0, 5, 0), Quaternion.identity, 0);

            bombHolder = PlayersInGame[spawnPicker];
            bombHolder.HasBomb = true;
            Debug.Log(bombHolder.gameObject.GetComponent<PhotonView>().ViewID);

            pV.RPC("WhoHasTheBomb", RpcTarget.All, bombHolder.gameObject.GetComponent<PhotonView>().ViewID, bomb.GetComponent<PhotonView>().ViewID);
        }
        else if(PlayersInGame.Count == 0)
        {
            pV.RPC("GameOver", RpcTarget.All, PlayersInGame[0].gameObject.GetComponent<PhotonView>().ViewID);
        }
    }

    private void FillList()
    {
        PlayersInGame.AddRange(FindObjectsOfType<Bummie>());
    }

    [PunRPC]
    void GameOver(int IDwinner)
    {
        GameObject winner = PhotonView.Find(IDwinner).gameObject; 
        winner.transform.localScale *= 2; 
    }
}
