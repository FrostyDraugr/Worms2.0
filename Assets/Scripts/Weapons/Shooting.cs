using UnityEngine;
using System.Collections;

namespace WeaponSystems
{
    public class Shooting : Damaging
    {
        public void SetUp(float damage, float lifetime)
        {
            _damage = damage;
            _lifeDuration = lifetime;
        }

        public IEnumerator Fire(int ammo, Transform firePoint)
        {
            float timeBetweenShots = 6 / ammo;
            for (int i = 0; i < ammo; i++)
            {
                yield return new WaitForSeconds(timeBetweenShots);
                if (Physics.Raycast(firePoint.position, firePoint.forward,
                out RaycastHit hit, 500f))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        hit.collider.gameObject.
                        GetComponent<Controllers.CharacterScript>()
                        .Hit(_damage, hit.point);
                    }
                }
            }
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    }
}