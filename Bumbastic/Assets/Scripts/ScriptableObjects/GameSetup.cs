using UnityEngine;

[CreateAssetMenu(fileName = "GameSetup",menuName = "GameSetup")]
public class GameSetup : ScriptableObject
{
    //public PowerUp powerUp;

    Vector3 crowPos;
    public Vector3 CrowPos { get => crowPos; set => crowPos = value; }
}
