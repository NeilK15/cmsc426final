using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    [SerializeField] private GameObject mainHUDPrefab;

    public IEnumerator Init()
    {
        GameAccess.RegisterController(this);

        if (mainHUDPrefab != null)
        {
            var hud = mainHUDPrefab.GetComponent<MainHUD>();
            if (hud != null)
                yield return hud.Init(); // waits until MainHUD is fully initialized
        }

        yield return null;
    }
}
