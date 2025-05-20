using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class FruitButton : MonoBehaviour
{

    [SerializeField]
    private float rotationSpeed = 10f;

    enum Action { PLAY, EXIT }

    [SerializeField]
    private Action action;

    private static float hoverScaleFactor = 1.2f;

    private Rigidbody _rb;
    private Vector3 originalScale;
    private Vector3 hoverScale;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        originalScale = transform.localScale;
        hoverScale = originalScale * hoverScaleFactor;
    }

    void Update()
    {
        Rotate();
    }

    void Rotate()
    {

        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }

    public void SetHover(bool isHovered)
    {
        if (isHovered)
        {
            transform.localScale = hoverScale;
        }
        else
        {
            transform.localScale = originalScale;
        }
    }

    public void Interact()
    {
        if (action == Action.PLAY)
        {
            Debug.Log("Playing Fruit Ninja");
            SceneManager.LoadScene(1);
        }
        else if (action == Action.EXIT)
        {
            Debug.Log("Quitting the game");
            Application.Quit();
        }
    }

}
