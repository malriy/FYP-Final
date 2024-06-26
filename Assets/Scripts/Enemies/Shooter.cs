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
        Vector3 playerInstance = PlayerController1.Instance != null ? PlayerController1.Instance.transform.position : Player.Instance.transform.position;
        Vector2 targetDirection = playerInstance - transform.position;
        Debug.Log("shooting @ " + playerInstance);
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.transform.right = targetDirection;

        if (newBullet.TryGetComponent<Projectile>(out Projectile projectile)){
            projectile.UpdateMoveSpeed(bulletMoveSpeed);
        }
    }
}
