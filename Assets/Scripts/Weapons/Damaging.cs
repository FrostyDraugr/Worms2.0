
using UnityEngine;
namespace WeaponSystems
{

    public class Damaging : MonoBehaviour
    {
        protected float _damage;
        protected float _lifeDuration;
        protected bool _hit;

        void Awake()
        {
            _hit = false;
        }
    }
}