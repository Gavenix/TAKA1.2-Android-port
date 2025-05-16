using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GunMAnager : MonoBehaviour
{
    [SerializeField] private GameObject defaultGunPrefab;

    [Header("Offsets")]
    public Vector2 leftOffset = new Vector2(-0.5f, 0.7f);
    public Vector2 rightOffset = new Vector2(0f, 0.7f);

    [Header("References")]
    public PlayerInventory playerInventory;
    public Transform playerTransform;

    private GameObject currentLeftWeapon;
    private GameObject currentRightWeapon;
    private int activeSet = 0; // 0 = Set1, 1 = Set2

    private bool usingMelee = false;
    private float meleeDisplayTime = 0.5f;
    private float meleeTimer = 0f;

    private void Start()
    {
        if (playerTransform == null)
            playerTransform = GameObject.FindWithTag("Player")?.transform;

        if (playerInventory == null)
            playerInventory = playerTransform?.GetComponent<PlayerInventory>();

        EquipCurrentSet();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            activeSet = 1 - activeSet;
            playerTransform.GetComponent<PlayerController>();
            EquipCurrentSet();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            UseMeleeWeapons();
        }

        if (usingMelee)
        {
            meleeTimer -= Time.deltaTime;
            if (meleeTimer <= 0f)
            {
                usingMelee = false;
                EquipCurrentSet();
            }
        }
        if (currentLeftWeapon != null)
        {
            int facing = playerTransform.GetComponent<PlayerController>().GetFacingDirection();

            // Меняем offset у Gun
            var gun = currentLeftWeapon.GetComponent<Gun>();
            if (gun != null)
            {
                Vector2 newOffset = leftOffset;
                newOffset.x = Mathf.Abs(leftOffset.x) * facing;
                gun.SetOffset(newOffset);
            }

            // Меняем порядок сортировки
            var sortingGroup = currentLeftWeapon.GetComponent<UnityEngine.Rendering.SortingGroup>();
            if (sortingGroup != null)
            {
                sortingGroup.sortingOrder = (facing == 1) ? 3 : 5;
            }
        }
        if (currentRightWeapon != null)
        {
            int facing = playerTransform.GetComponent<PlayerController>().GetFacingDirection();

            // Меняем порядок сортировки
            var sortingGroup = currentRightWeapon.GetComponent<UnityEngine.Rendering.SortingGroup>();
            if (sortingGroup != null)
            {
                sortingGroup.sortingOrder = (facing == 1) ? 2 : 4;
            }
        }
    }

    public void EquipCurrentSet()
    {
        ClearHands(); 
        int facing = playerTransform.GetComponent<PlayerController>().GetFacingDirection();

        int leftIndex = activeSet == 0 ? 0 : 2;
        int rightIndex = activeSet == 0 ? 1 : 3;

        EquipFromSlot(leftIndex, leftOffset, isRight: false, facing);
        EquipFromSlot(rightIndex, rightOffset, isRight: true, facing);
    }

    private void UseMeleeWeapons()
    {
        ClearHands();
        usingMelee = true;
        meleeTimer = meleeDisplayTime;

        int facing = playerTransform.GetComponent<PlayerController>().GetFacingDirection();

        EquipFromSlot(4, leftOffset, false, facing);
        EquipFromSlot(5, rightOffset, true, facing);

        Debug.Log("Melee attack used!");
    }

    public void EquipFromSlot(int index, Vector2 baseOffset, bool isRight, int facingDirection)
    {
        if (index >= playerInventory.weaponSlots.Length) return;

        WeaponData weapon = playerInventory.weaponSlots[index].currentWeapon;
        if (weapon == null || weapon.weaponPrefab == null) return;

        Vector2 finalOffset = baseOffset;
        finalOffset.x = Mathf.Abs(baseOffset.x) * facingDirection; 

        Vector3 worldPos = playerTransform.position + (Vector3)finalOffset;
        GameObject newWeapon = Instantiate(weapon.weaponPrefab, worldPos, Quaternion.identity);

        // Используем scale из префаба!
        newWeapon.transform.localScale = weapon.weaponPrefab.transform.localScale;

        newWeapon.GetComponent<Gun>()?.SetOffset(finalOffset);

        if (isRight)
            currentRightWeapon = newWeapon;
        else
            currentLeftWeapon = newWeapon;
    }
    public void SetActiveGunsFiring(bool firing)
    {
        if (currentLeftWeapon != null)
        {
            var gun = currentLeftWeapon.GetComponent<Gun>();
            if (gun != null)
                gun.SetFiring(firing);
        }
        if (currentRightWeapon != null)
        {
            var gun = currentRightWeapon.GetComponent<Gun>();
            if (gun != null)
                gun.SetFiring(firing);
        }
    }
    private void ClearHands()
    {
        if (currentRightWeapon) Destroy(currentRightWeapon);
        if (currentLeftWeapon) Destroy(currentLeftWeapon);
    }
    public void SwitchWeaponSet()
    {
        activeSet = 1 - activeSet;
        EquipCurrentSet();
    }
}