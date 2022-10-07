
using UnityEngine;
using System.Collections.Generic;

namespace Controllers
{
    [RequireComponent(typeof(LineRenderer))]
    public class WeaponController : MonoBehaviour
    {
        public List<WeaponSystems.Weapon> Weapons;
        public int ChosenWeapon;
        public GameObject WeaponSlot;
        [SerializeField] private float _throwForce;
        [SerializeField] private float _throwUpForce;
        [SerializeField] private GameObject _bullet;
        public LineRenderer _lineRenderer;
        private float _projectileMass;

        [SerializeField] private GameObject _sphere;

        [Header("Arch Line Controls")]
        [Range(10, 100)]
        [SerializeField] private int _linePoints;
        [Range(0.01f, 0.25f)]
        [SerializeField] private float _timeBetweenPoints;

        private LayerMask _grenadeCollisionMask;
        private void Awake()
        {
            //Get the grenade collison layer
            int projectileLayer = Weapons[0].Model.layer;
            for (int i = 0; i < 32; i++)
            {
                if (!Physics.GetIgnoreLayerCollision(projectileLayer, i))
                {
                    //Weird black magic
                    _grenadeCollisionMask |= 1 << i;
                }
            }
        }
        private void Start()
        {
            _lineRenderer = gameObject.GetComponent<LineRenderer>();

            _sphere = Instantiate(_sphere, Vector3.zero,
            Quaternion.identity, gameObject.transform);
            _sphere.SetActive(false);

        }

        public void UnDrawArch()
        {
            _lineRenderer.enabled = false;
            _sphere.SetActive(false);
        }

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
                    _projectileMass = Weapons[ChosenWeapon].Model.GetComponent<Rigidbody>().mass;
                    _sphere.transform.localScale =
                    new Vector3(Weapons[ChosenWeapon].Radius,
                    Weapons[ChosenWeapon].Radius,
                    Weapons[ChosenWeapon].Radius);
                    _lineRenderer.enabled = true;
                    break;

                default:
                    UnDrawArch();
                    break;
            }
        }

        //Have the input manager turn on and off the relevant components
        //Limits this to one Update loop instead of one for every worm...
        private void Update()
        {
            if (_lineRenderer.enabled == true)
            {
                DrawArch();
            }
        }

        private void DrawArch()
        {
            _lineRenderer.enabled = true;
            _lineRenderer.positionCount = Mathf.CeilToInt(_linePoints / _timeBetweenPoints) + 2;
            Vector3 startPosition = WeaponSlot.transform.position;
            Vector3 startVelocity = (gameObject.transform.forward * _throwForce + transform.up * _throwUpForce) / _projectileMass;
            int i = 0;
            _lineRenderer.SetPosition(i, startPosition);
            for (float time = 0; time < _linePoints; time += _timeBetweenPoints)
            {
                i++;
                Vector3 point = startPosition + time * startVelocity;
                point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y * 0.5f * time * time);

                _lineRenderer.SetPosition(i, point);

                Vector3 lastPoisition = _lineRenderer.GetPosition(i - 1);

                _sphere.SetActive(true);


                if (Physics.Raycast(lastPoisition,
                (point - lastPoisition).normalized,
                out RaycastHit hit,
                (point - lastPoisition).magnitude,
                _grenadeCollisionMask))
                {
                    _sphere.transform.position = hit.point;
                    _lineRenderer.SetPosition(i, hit.point);
                    _lineRenderer.positionCount = i + 1;
                    break;
                }

                _sphere.transform.position = _lineRenderer.GetPosition(i);
            }

        }

        public void Shoot()
        {
            switch (Weapons[ChosenWeapon]._Type)
            {
                case WeaponSystems.Weapon.Type.Grenade:

                    GameObject grenade = WeaponSlot.transform.GetChild(0).gameObject;

                    SphereCollider sr = grenade.GetComponent<SphereCollider>();
                    sr.enabled = true;

                    Rigidbody rb = grenade.GetComponent<Rigidbody>();

                    rb.useGravity = true;
                    rb.isKinematic = false;

                    Vector3 forceDirection = gameObject.transform.forward;

                    //Decrepit now that I'm doing the line tracker for the grenade.
                    //Will be useful for the gun though, maybe
                    /*RaycastHit hit;

                    if (Physics.Raycast(gameObject.transform.position,
                    gameObject.transform.forward, out hit, 500f))
                    {
                        forceDirection = (hit.point - WeaponSlot.transform.position).normalized;
                    } */

                    Vector3 forceToAdd = forceDirection * _throwForce
                    + transform.up * _throwUpForce;
                    rb.AddForce(forceToAdd, ForceMode.Impulse);

                    UnDrawArch();

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