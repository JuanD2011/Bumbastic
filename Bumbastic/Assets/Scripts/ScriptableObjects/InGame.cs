using UnityEngine;

[CreateAssetMenu(fileName = "InGame",menuName = "InGame")]
public class InGame : ScriptableObject
{
    //public PowerUp powerUp;

    Vector3 crowPos;
    public Vector3 CrowPos { get => crowPos; set => crowPos = value; }
}
