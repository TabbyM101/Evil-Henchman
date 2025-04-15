using System.Collections;
using UnityEngine;

public class SpriteParticleSystem : MonoBehaviour
{
    [Header("Particle Settings")]
    public Sprite[] possibleSprites;
    public GameObject particlePrefab;
    public int burstCount = 10;
    public float minSpeed = 1f;
    public float maxSpeed = 3f;
    public float lifetime = 1f;

    [Header("Explosion Power")]
    [Range(0f, 10f)]
    public float power = 1f;

    [Header("Loop Settings")]
    public bool loop = false;
    public float loopInterval = 1f;

    private Coroutine loopCoroutine;

    void Start()
    {
        if (loop && loopInterval > 0.01f)
        {
            loopCoroutine = StartCoroutine(LoopEmit());
        }
        else if (loop)
        {
            Debug.LogWarning("Loop is enabled but interval is too small. Aborting loop.");
        }
    }

    public void EmitBurst()
    {
        int count = Mathf.Clamp(burstCount, 1, 1000);
        for (int i = 0; i < count; i++)
        {
            GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity, transform);
            SpriteRenderer sr = particle.GetComponent<SpriteRenderer>();
            if (sr != null && possibleSprites.Length > 0)
            {
                sr.sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];
            }

            Rigidbody2D rb = particle.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = Random.insideUnitCircle.normalized;
                float speed = Random.Range(minSpeed, maxSpeed)/5 * power;

                rb.linearVelocity = direction * speed;
                rb.angularVelocity = Random.Range(-360f, 360f);
                rb.gravityScale = 0.2f; // You can expose this as a setting too
            }

            StartCoroutine(FadeAndDestroy(particle, lifetime));
        }
    }

    private IEnumerator LoopEmit()
    {
        while (loop)
        {
            EmitBurst();
            yield return new WaitForSeconds(loopInterval);
        }
    }

    public void SetLoop(bool shouldLoop)
    {
        if (shouldLoop && loopCoroutine == null)
        {
            loop = true;
            loopCoroutine = StartCoroutine(LoopEmit());
        }
        else if (!shouldLoop && loopCoroutine != null)
        {
            loop = false;
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        }
    }
    
    private IEnumerator FadeAndDestroy(GameObject particle, float duration)
    {
        SpriteRenderer sr = particle.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Destroy(particle, duration);
            yield break;
        }

        Color originalColor = sr.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(particle);
    }

}
