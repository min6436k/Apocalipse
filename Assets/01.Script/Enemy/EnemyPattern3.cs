using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPattern3 : EnemyPattern1
{
    public GameObject Projectile;
    public float ProjectileMoveSpeed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerBullet")
        {
            GameObject projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetDirection(Vector3.down);
            projectile.GetComponent<Projectile>().MoveSpeed = ProjectileMoveSpeed;
            movingUp = !movingUp;
            transform.position = new Vector3(Random.Range(startPosition.x - Amplitude, startPosition.x + Amplitude), transform.position.y+1, transform.position.z);
        }
    }
}
