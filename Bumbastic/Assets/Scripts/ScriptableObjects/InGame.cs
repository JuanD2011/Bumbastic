using UnityEngine;

[CreateAssetMenu(fileName = "InGame",menuName = "InGame")]
public class InGame : ScriptableObject
{
    //public PowerUp powerUp;

    public Transform posDropPU;

    Vector3 crowPos;
    public Vector3 CrowPos { get => crowPos; set => crowPos = value; }

    public void ReturnMenu()
    {
        PhotonRoom.room.ReturnMenu();
    }
}
