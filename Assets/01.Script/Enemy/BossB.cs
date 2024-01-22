using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossB : BossA
{
    public override void Start()
    {
        base.Start();

        MaxpatternIndex = 3;
        MoveSpeed *= 1.3f;
    }

    public override IEnumerator Pattern1()
    {
        int numBullets = 30;
        float BulletInterval = 0.1f;

        int angle = 270;

        bool IsRight = false;

        for (int i = 0; i < numBullets; i++)
        {
            angle += IsRight ? 10 : -10;
            if (angle <= 220 || angle >= 320) IsRight = !IsRight;
            float radian = angle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0);

            ShootProjectile(transform.position, direction);

            yield return new WaitForSeconds(BulletInterval);
        }

        yield return new WaitForSeconds(FireRate);


        NextPattern();
    }

    public override IEnumerator Pattern2()
    {
        float moveTime = 0;
        _bCanMove = false;

        yield return new WaitForSeconds(1);

        while (moveTime <= 1f)
        {
            moveTime += Time.deltaTime;
            transform.Translate(Vector3.up * 4 * Time.deltaTime);
            yield return null;
        }
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        int numBullets = 5;
        float BulletInterval = 1f;

        for (int i = 0; i < numBullets; i++)
        {
            for (int j = -12; j < 12; j++)
            {
                if (Random.Range(0, 3) == 0) continue;
                ShootProjectile(new Vector3(j, transform.position.y), Vector3.down);
            }
            yield return new WaitForSeconds(BulletInterval);
        }
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        moveTime = 0;
        while (moveTime <= 1f)
        {
            moveTime += Time.deltaTime;
            transform.Translate(Vector3.down * 4 * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        _bCanMove = true;

        yield return new WaitForSeconds(FireRate);

        NextPattern();
    }

    public override IEnumerator Pattern3()
    {
        float R = 8;
        float deg = 00;
        Vector3 originPosition = transform.position;

        _bCanMove = false;

        float moveTime = 0;
        while (moveTime < 1f)
        {
            moveTime += Time.deltaTime;
            transform.position = Vector3.Lerp(originPosition, PlayerPosition() + new Vector3(0, 7, 0),moveTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(Pattern3Bullet());

        while (deg < 720)
        {

            deg += Time.deltaTime * 300;

            var rad = Mathf.Deg2Rad * deg;
            var x = R * Mathf.Sin(rad);
            var y = R * Mathf.Cos(rad);
            transform.position = PlayerPosition() + new Vector3(x, y);

            yield return null;
        }

        yield return new WaitForSeconds(1);

        moveTime = 0;
        while (moveTime < 1f)
        {
            moveTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, originPosition, moveTime);
            yield return null;
        }

        _bCanMove = true;

        yield return new WaitForSeconds(FireRate);

        NextPattern();

    }

    IEnumerator Pattern3Bullet()
    {
        for(int i = 0; i < 24; i++)
        {
            Vector3 playerDirection = (PlayerPosition() - transform.position).normalized;
            ShootProjectile(transform.position, playerDirection,speed:4);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
