using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FruitButton : MonoBehaviour
{

    [SerializeField]
    private float rotationSpeed = 10f;

    private Rigidbody _rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    void Rotate()
    {

        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }
}
