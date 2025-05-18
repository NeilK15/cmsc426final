using UnityEngine;

public class FruitNinjaCore : MonoBehaviour
{
    [SerializeField] private UILayerRequest FruitNinjaHUD;
    [SerializeField] private GameObject FruitSpawnerPrefab;
    public FruitSpawner fruitSpawner { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartFruitNinja()
    {
        Debug.Log("Start fruit ninja");
        fruitSpawner = Instantiate(FruitSpawnerPrefab).GetComponent<FruitSpawner>();
        GameAccess.GetMainHUD().PushToLayer(FruitNinjaHUD);
        GameAccess.GetGameState().timerComponent.onTimerEnd.AddListener(EndFruitNinja);
    }
    public void EndFruitNinja()
    {

        Debug.Log("End fruit ninja");
        fruitSpawner.End();
    }

}
