
using UnityEngine;
using System.Collections.Generic;

namespace Controllers
{
    public class WeaponController : MonoBehaviour
    {
        public List<WeaponSystems.Weapon> Weapons;
        public int ChosenWeapon;
        public GameObject WeaponSlot;

        [SerializeField]
        private float _throwForce;

        [SerializeField]
        private float _throwUpForce;

        [SerializeField]
        private GameObject _bullet;

        public void InstWeapon()
        {
            GameObject obj = Instantiate(Weapons[ChosenWeapon].Model, Vector3.zero,
            Quaternion.identity, WeaponSlot.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Weapons[ChosenWeapon].Model.transform.rotation;
            switch (Weapons[ChosenWeapon]._Type)
            {
                case WeaponSystems.Weapon.Type.Grenade:
                    WeaponSystems.Explosive ex = obj.GetComponent<WeaponSystems.Explosive>();
                    ex.SetUp(Weapons[ChosenWeapon].Damage,
                    Weapons[ChosenWeapon].Lifetime,
                    Weapons[ChosenWeapon].Radius);
                    break;

                default:
                    break;
            }
        }

        public void Shoot()
        {
            switch (Weapons[ChosenWeapon]._Type)
            {
                case WeaponSystems.Weapon.Type.Grenade:

                    //Out of bounds because we don't end the current turn
                    GameObject grenade = WeaponSlot.transform.GetChild(0).gameObject;

                    SphereCollider sr = grenade.GetComponent<SphereCollider>();
                    sr.enabled = true;

                    Rigidbody rb = grenade.GetComponent<Rigidbody>();

                    rb.useGravity = true;
                    rb.isKinematic = false;

                    Vector3 forceDirection = gameObject.transform.forward;

                    RaycastHit hit;

                    if (Physics.Raycast(gameObject.transform.position,
                    gameObject.transform.forward, out hit, 500f))
                    {
                        forceDirection = (hit.point - WeaponSlot.transform.position).normalized;
                    }

                    Vector3 forceToAdd = forceDirection * _throwForce
                    + transform.up * _throwUpForce;
                    rb.AddForce(forceToAdd, ForceMode.Impulse);

                    StartCoroutine(grenade.GetComponent<WeaponSystems.Explosive>().Throw());

                    break;

                case WeaponSystems.Weapon.Type.Gun:

                    break;

                default:
                    break;
            }
        }
    }
}