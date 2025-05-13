using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    [SerializeField] private GameObject mainHUDPrefab;

    private void Awake()
    {
        GameAccess.RegisterController(this);
    }

 
}
