using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public float INITIAL_ATTACK_CD = 2f;
    public float ATTACK_CD = 1f;

    public GameObject bulletPrefab;

    private float lastShotTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        lastShotTime = Time.time + INITIAL_ATTACK_CD;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastShotTime > ATTACK_CD)
        {
            lastShotTime = Time.time;

            GameObject rightBullet = Instantiate(bulletPrefab, this.transform.position + Vector3.right, Quaternion.identity);
            rightBullet.GetComponent<BulletScript>().SetDirection(Vector3.right);
            GameObject downBullet = Instantiate(bulletPrefab, this.transform.position + Vector3.down, Quaternion.Euler(Vector3.forward * 90));
            downBullet.GetComponent<BulletScript>().SetDirection(Vector3.down);
            GameObject leftBullet = Instantiate(bulletPrefab, this.transform.position + Vector3.left, Quaternion.Euler(Vector3.forward * 180));
            leftBullet.GetComponent<BulletScript>().SetDirection(Vector3.left);
            GameObject upBullet = Instantiate(bulletPrefab, this.transform.position + Vector3.up, Quaternion.Euler(Vector3.forward * 270));
            upBullet.GetComponent<BulletScript>().SetDirection(Vector3.up);
        }
    }
}
