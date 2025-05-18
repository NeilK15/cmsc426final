using UnityEngine;
using System.Collections;

public class GameMode : MonoBehaviour
{
    [SerializeField] private GameObject controllerPrefab;
    [SerializeField] private GameObject FruitNinjaCorePrefab;
    public FruitNinjaCore fruitNinjaCore { get; private set; }


    public IEnumerator Init()
    {

        var spawn = FindFirstObjectByType<PlayerSpawn>();
        if (controllerPrefab && spawn != null)
        {
            var controllerGO = Instantiate(controllerPrefab, spawn.transform.position, spawn.transform.rotation);
            var controller = controllerGO.GetComponent<Controller>();
            if (controller != null)
                yield return controller.Init(); // waits until controller is fully initialized  
                fruitNinjaCore = Instantiate(FruitNinjaCorePrefab).GetComponent<FruitNinjaCore>();
                fruitNinjaCore.StartFruitNinja();
                
        }
        else
        {
            Debug.LogWarning("GameMode: Missing ControllerPrefab or PlayerSpawn.");
        }
    }
}
