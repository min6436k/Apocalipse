using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5PlayerBullet : Projectile
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.GetComponent<Meteor>()) return;
        if (collision.tag == "Enemy" && !collision.GetComponent<Projectile>())
        {
            int _rotate = -1;
            for (int i = 0; i < 3; i++)
            {
                GameObject instance = Instantiate(Homingtile, transform.position, Quaternion.Euler(new Vector3(0, 0, 80 * _rotate++)));
                instance.GetComponent<HomingProjectile>().target = collision.transform;
            }

            Destroy(this.gameObject);
        }
    }
}
