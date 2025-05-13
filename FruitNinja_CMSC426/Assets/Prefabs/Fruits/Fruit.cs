using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Fruit : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;
    [SerializeField] private GameObject slicedPrefab;
    [SerializeField] private float spinTorque = 5f;

    public Action<Fruit> OnFruitDeath;

    private Rigidbody rb;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);

        // Add randomized spin around a random axis
        Vector3 torqueAxis = UnityEngine.Random.onUnitSphere;
        float torqueAmount = UnityEngine.Random.Range(-spinTorque, spinTorque);
        rb.AddTorque(torqueAxis * torqueAmount, ForceMode.Impulse);
    }

    public void Split()
    {
        if (isDead) return;

        isDead = true;
        if (slicedPrefab)
            Instantiate(slicedPrefab, transform.position, transform.rotation);

        GameAccess.GetGameState()?.AddScore(scoreValue);
        OnFruitDeath?.Invoke(this);
        Destroy(gameObject);
    }

    public void Die()
    {
        if (isDead) return;
        if (rb.linearVelocity.y > -0.1f) return;

        isDead = true;
        OnFruitDeath?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("DeathZone"))
            Die();
    }
}
