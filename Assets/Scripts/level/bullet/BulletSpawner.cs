using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour
{
    [System.Obsolete]
    public void ExecutePattern(BulletPattern p)
    {
        switch (p.type)
        {
            case PatternType.Circle:
                SpawnCircle(p);
                break;

            case PatternType.Spiral:
                StartCoroutine(SpawnSpiral(p));
                break;

            case PatternType.Wave:
                StartCoroutine(SpawnWave(p));
                break;

            case PatternType.TargetPlayer:
                SpawnTargetPlayer(p);
                break;

            case PatternType.Rain:
                StartCoroutine(SpawnRain(p));
                break;
        }
    }

    // ---------------- Circle Burst ----------------
    [System.Obsolete]
    void SpawnCircle(BulletPattern p)
    {
        for (int i = 0; i < p.count; i++)
        {
            float angle = (i / (float)p.count) * 360f + p.angleOffset;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

            var b = Instantiate(p.bulletPrefab, transform.position, Quaternion.identity);
            b.GetComponent<Rigidbody2D>().velocity = dir * p.speed;
        }
    }

    // ---------------- Spiral ----------------------
    [System.Obsolete]
    IEnumerator SpawnSpiral(BulletPattern p)
    {
        float angle = p.angleOffset;

        for (int i = 0; i < p.count; i++)
        {
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

            var b = Instantiate(p.bulletPrefab, transform.position, Quaternion.identity);
            b.GetComponent<Rigidbody2D>().velocity = dir * p.speed;

            angle += 360f / p.count;

            yield return new WaitForSeconds(p.duration / p.count);
        }
    }

    // ---------------- Wave ------------------------
    [System.Obsolete]
    IEnumerator SpawnWave(BulletPattern p)
    {
        for (int i = 0; i < p.count; i++)
        {
            float angle = Mathf.Sin(i * 0.3f) * 45f;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

            var b = Instantiate(p.bulletPrefab, transform.position, Quaternion.identity);
            b.GetComponent<Rigidbody2D>().velocity = dir * p.speed;

            yield return new WaitForSeconds(p.duration / p.count);
        }
    }

    // -------------- Target Player ------------------
    [System.Obsolete]
    void SpawnTargetPlayer(BulletPattern p)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 dir = (player.transform.position - transform.position).normalized;

        var b = Instantiate(p.bulletPrefab, transform.position, Quaternion.identity);
        b.GetComponent<Rigidbody2D>().velocity = dir * p.speed;
    }

    // ---------------- Rain -------------------------
    [System.Obsolete]
    IEnumerator SpawnRain(BulletPattern p)
    {
        Camera cam = Camera.main;

        for (int i = 0; i < p.count; i++)
        {
            float x = Random.Range(-9f, 9f);

            Vector3 pos = new Vector3(x, cam.transform.position.y + 6f, 0);
            var b = Instantiate(p.bulletPrefab, pos, Quaternion.identity);
            b.GetComponent<Rigidbody2D>().velocity = Vector2.down * p.speed;

            yield return new WaitForSeconds(0.05f);
        }
    }
}
