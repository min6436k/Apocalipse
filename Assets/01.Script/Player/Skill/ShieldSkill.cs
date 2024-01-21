using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShieldSkill : BaseSkill
{
    public GameObject bullet;
    public int count = 8;
    public float rotateSpeed = 200;

    private bool rotate = false;

    private void FixedUpdate()
    {
        if (!rotate) return;
        transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
        if (transform.childCount == 0) rotate = false;
    }

    public override void Activate()
    {
        base.Activate();

        for (int i = 0; i < count; i++)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);

            GameObject instance = Instantiate(bullet, transform);

            instance.GetComponent<Projectile>().autoDestroy = false;
            instance.transform.localPosition = Vector3.up;

            float spawnangle = 360.0f / count;

            instance.transform.localPosition = new Vector3(Mathf.Sin(i * spawnangle * Mathf.Deg2Rad), Mathf.Cos(i * spawnangle * Mathf.Deg2Rad), 0.0f);

            instance.transform.rotation = Quaternion.AngleAxis(-spawnangle * i+90, Vector3.forward);
        }

        rotate = true;
    }
}
