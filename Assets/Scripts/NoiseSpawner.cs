using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseSpawner : MonoBehaviour
{
    public float noiseRadius = 10f;
    public float noiseDuration = 1f;

    public void SpawnNoise(float radius, float duration)
    {
        noiseRadius = radius;
        noiseDuration = duration;
        StartCoroutine(SpawnNoiseCoroutine());
    }

    private IEnumerator SpawnNoiseCoroutine()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, noiseRadius);
        foreach (Collider collider in hitColliders)
        {
            EnemyAI enemy = collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.OnNoiseReceived(transform.position);
            }
        }

        // Optional: draw sphere for debugging
        DebugDrawNoise(transform.position, noiseRadius, noiseDuration);

        yield return new WaitForSeconds(noiseDuration);
    }

    private void DebugDrawNoise(Vector3 center, float radius, float duration)
    {
        float step = 10f;
        for (float angle = 0; angle < 360; angle += step)
        {
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Debug.DrawRay(center, dir * radius, Color.yellow, duration);
        }
    }
}