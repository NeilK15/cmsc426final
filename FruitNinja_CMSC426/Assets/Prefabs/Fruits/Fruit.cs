using UnityEngine;
using EzySlice;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Fruit : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;
    [SerializeField] private Material crossSectionMaterial;
    [SerializeField] private float destroyDelay = 3f;
    private const float MaxLifetime = 12f;

    public Action<Fruit> OnFruitDeath;

    private Rigidbody rb;
    private bool isDead = false;

    private Transform visualRoot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        visualRoot = transform.GetChild(0); // assumes model is first child
        StartCoroutine(FailsafeDespawn());
    }

    public void Launch(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);

        Vector3 torqueAxis = UnityEngine.Random.onUnitSphere;
        float torque = UnityEngine.Random.Range(-5f, 5f);
        rb.AddTorque(torqueAxis * torque, ForceMode.Impulse);
    }
    public void Split(Vector3 contactPoint, Vector3 cutDirection)
    {
        if (isDead || visualRoot == null) return;

        Vector3 worldPos = visualRoot.position;
        Quaternion worldRot = visualRoot.rotation;
        Vector3 worldScale = visualRoot.lossyScale;

        // Fix: plane normal should be perpendicular to blade movement (in world Z-plane)
        Vector3 planeNormal = Vector3.Cross(cutDirection.normalized, Vector3.forward).normalized;

        GameObject[] slices = visualRoot.gameObject.SliceInstantiate(contactPoint, planeNormal, crossSectionMaterial);

        if (slices != null)
        {
            isDead = true;

            for (int i = 0; i < slices.Length; i++)
            {
                GameObject slice = slices[i];
                slice.transform.position = worldPos;
                slice.transform.rotation = worldRot;
                slice.transform.localScale = worldScale;

                Rigidbody rb = slice.AddComponent<Rigidbody>();
                Collider col = slice.GetComponent<Collider>();
                if (col) col.enabled = false;

                float directionSign = (i % 2 == 0) ? 1f : -1f;
                Vector3 force = directionSign * cutDirection.normalized * 5f + Vector3.up * 1.5f;

                rb.AddForce(force, ForceMode.Impulse);
                Destroy(slice, destroyDelay);
            }

            GameAccess.GetGameState()?.AddScore(scoreValue);
            OnFruitDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }

    private IEnumerator FailsafeDespawn()
    {
        yield return new WaitForSeconds(MaxLifetime);
        if (!isDead)
        {
            Die();
        }
    }



    public void Die()
    {
        if (isDead) return;
        if (rb.linearVelocity.y > -0.1f) return;

        isDead = true;
        OnFruitDeath?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        if (isDead) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("DeathZone"))
            Die();

        if (col.CompareTag("Blade"))
        {
            Blade blade = col.GetComponent<Blade>();
            if (blade != null && blade.Velocity.magnitude >= blade.cutVelocityThreshold)
            {
                Vector3 contactPoint = col.ClosestPoint(transform.position);
                Vector3 cutDirection = blade.Velocity.normalized;

                Split(contactPoint, cutDirection);
            }
        }
    }
}
