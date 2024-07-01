using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private Transform shotPos;
    [Tooltip("Stagger must be enabled for oscillate to function properly.")]
    [SerializeField] private Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    private bool isShooting = false;
    public void Attack() {  
        if (!isShooting){
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine(){
        isShooting = true;

        EnemyHealth enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth.CurrentHealth == 1) {
            for (int i = 0; i < burstCount; i++){
                ShootBullet();
                yield return new WaitForSeconds(timeBetweenBursts);
            }
        } else {
            ShootBullet();
        }

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    private void ShootBullet() {
        Vector3 playerInstance = PlayerController1.Instance != null ? PlayerController1.Instance.transform.position : player.transform.position;
        Vector2 targetDirection = playerInstance - transform.position;

        Vector3 shootLocation = shotPos != null ? shotPos.position : transform.position;
        GameObject newBullet = Instantiate(bulletPrefab, shootLocation, Quaternion.identity);
        newBullet.transform.right = targetDirection;

        if (newBullet.TryGetComponent<Projectile>(out Projectile projectile)){
            projectile.UpdateMoveSpeed(bulletMoveSpeed);
        }
    }
}
