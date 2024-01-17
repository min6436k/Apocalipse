using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossAfterImage : MonoBehaviour
{
    public float moveStep;

    public float ProjectileMoveSpeed;

    public GameObject projectile;
    private void Start()
    {
        StartCoroutine(Move());
        StartCoroutine(HitFlickAndGun());
        Destroy(gameObject,6f);
        StartCoroutine(FadeOut());
    }

    public void ShootProjectile(Vector3 position, Vector3 direction)
    {
        GameObject instance = Instantiate(projectile, position, Quaternion.identity); 
        Projectile _projectile = instance.GetComponent<Projectile>();

        if (_projectile != null) 
        {
            _projectile.MoveSpeed = ProjectileMoveSpeed; 
            _projectile.SetDirection(direction.normalized); 
        }
    }

    IEnumerator Move()
    {
        Vector3 CurrentPosition = transform.position;
        Vector3 targetPosition = CurrentPosition- new Vector3(moveStep*1.2f,0,0);
        float elapsedTime = 0f;

        while (elapsedTime < 0.4f)
        {
            // Lerp�� ����Ͽ� ���� ��ġ���� ��ǥ ��ġ���� �ε巴�� �̵�
            transform.position = Vector3.Lerp(CurrentPosition, targetPosition, elapsedTime / 0.4f);

            // ��� �ð� ����
            elapsedTime += Time.deltaTime;

            // ���� �����ӱ��� ���
            yield return null;
        }

        Debug.Log("Movement finished!");
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(5f);

        float elapsedTime = 0f;
        Color startColor = GetComponentInChildren<SpriteRenderer>().color; // ���� ���� ��������
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // ���İ��� 0���� ����

        while (elapsedTime < 0.5f)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(startColor, targetColor, elapsedTime / 0.5f);

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator HitFlickAndGun()
    {
        int flickCount = 0;

        while (flickCount < 6)
        {
            GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);

            yield return new WaitForSeconds(0.08f);

            GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.Abs(moveStep) == 1 ? 0.5f : 0.25f);

            yield return new WaitForSeconds(0.08f);

            flickCount++;
        }

        yield return new WaitForSeconds(Mathf.Abs(moveStep) == 1 ? 0.2f : 0.4f);

        shotBullet();

        yield return new WaitForSeconds(1.5f);

        shotBullet();

        yield return new WaitForSeconds(1.5f);

        shotBullet();
    }

    private void shotBullet()
    {
        for (int i = 0; i < 2; i++)
        {
            ShootProjectile(transform.position, Vector3.down);
            ShootProjectile(transform.position, new Vector3(0.3f, -1, 0));
            ShootProjectile(transform.position, new Vector3(-0.3f, -1, 0));
        }
    }
}
