using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    public float INITIAL_ATTACK_CD = 2f;
    public float ATTACK_CD = 1f;

    public GameObject bulletPrefab;
    public Image timer;

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

        float healthbarFraction = (Time.time - lastShotTime) / ATTACK_CD;
        healthbarFraction = Mathf.Clamp01(healthbarFraction);

        timer.transform.localScale = new Vector3((1 - healthbarFraction), 1, 1);
        timer.transform.position = new Vector3(401.5f + (1 - healthbarFraction) * 100f, timer.transform.position.y, 0);
    }
}
