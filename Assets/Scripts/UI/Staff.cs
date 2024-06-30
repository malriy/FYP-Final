using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaser;
    [SerializeField] private Transform magiclaserSpawnPoint;

    private Animator myAnimator;
    [SerializeField] private Player player;


    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake(){
        myAnimator = GetComponent<Animator>();
    }

    private void Update(){
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        myAnimator.SetTrigger(ATTACK_HASH);
    }

    public void SpawnStaffProjectAnimEvent(){
        GameObject newLaser = Instantiate(magicLaser, magiclaserSpawnPoint.position, Quaternion.identity);
        newLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint((PlayerController1.Instance != null ? PlayerController1.Instance.transform.position : player.transform.position));

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
