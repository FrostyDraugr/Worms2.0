using UnityEngine;

namespace Controllers
{

    public class CharacterScript : MonoBehaviour
    {
        [SerializeField]
        private float _movePoints;

        private float _movePointsLeft;

        public float Life;
        private Managers.GameManager _gm;

        //Just playing around, could be used for 
        public enum Mode { inactive, combat, move, dead };

        public Mode State;

        [SerializeField]
        private float _force;

        [SerializeField]
        private float _maxSpeed;

        [SerializeField]
        private float _rSpeed;

        [SerializeField]
        private float _jumpVelocity;

        private Rigidbody _rb;

        [SerializeField]
        private Transform _ws;

        private float _distToGround;

        private WeaponController _wc;

        public Managers.TeamManager AssignedTeam;

        private void Awake()
        {
            State = Mode.inactive;
        }

        //Create an OnDisable and an OnEnable that turns on/off the WeaponController
        //Instead of having a bool

        private void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _distToGround = gameObject.GetComponent<CapsuleCollider>()
            .bounds.extents.y;
            _gm = Managers.GameManager.GameMang;
            _ws = gameObject.transform.GetChild(0).transform;
            _wc = gameObject.GetComponent<WeaponController>();
            _wc.ChosenWeapon = 0;
        }

        public void Hit(float dmg, Vector3 point)
        {
            _rb.AddForce((transform.position - point) * (dmg * 2), ForceMode.Impulse);
            Life -= dmg;
            if (Life <= 0 && State != Mode.dead)
            {
                bool assigned;
                if (State == Mode.inactive)
                {
                    assigned = false;
                }
                else
                {
                    assigned = true;
                }
                Managers.EventManager._eventManager
                .DeathTrigger(AssignedTeam, gameObject, assigned);
                State = Mode.dead;
            }
        }

        public void WormActive()
        {
            _movePointsLeft = _movePoints;
            State = Mode.move;
        }

        public void Attack()
        {
            if (State == Mode.combat)
            {
                _wc.Shoot();
                StartCoroutine(_gm.EndTurn());
                State = Mode.inactive;
            }
            else
            {
                EnterCombat();
            }
        }

        //Why is switch weapon in the characterScript, idiot
        public void SwitchWeapon()
        {
            if (State == Mode.combat)
            {
                _wc.UnDrawArch();
                if (_wc.ChosenWeapon == _wc.Weapons.Count - 1)
                {
                    _wc.ChosenWeapon = 0;
                }
                else
                {
                    _wc.ChosenWeapon++;
                }
                foreach (Transform child in _wc.WeaponSlot.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                _wc.InstWeapon();
            }
            else
            {
                EnterCombat();
            }
        }
        public void CancelAttack()
        {
            if (State == Mode.combat && _movePointsLeft > 0)
            {
                _wc.UnDrawArch();
                EnterMoving();
            }
        }



        private void EnterCombat()
        {
            _wc.ChosenWeapon = 0;

            foreach (Transform child in _wc.WeaponSlot.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            _wc.InstWeapon();
            State = Mode.combat;
        }

        private void EnterMoving()
        {
            foreach (Transform child in _wc.WeaponSlot.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            _wc.UnDrawArch();
            State = Mode.move;
        }

        public void MoveVertical(float dir)
        {
            if (State == Mode.move)
            {
                if (_movePointsLeft > 0)
                {
                    /*_rb.AddForce(transform.forward * (dir * Time.deltaTime * _force),
                    ForceMode.Force);
                     _movePointsLeft -= 1 * Time.fixedDeltaTime; */
                    float y = _rb.velocity.y;

                    //_rb.velocity = _rb.velocity.normalized * _maxSpeed;

                    _rb.velocity = new Vector3(
                    Mathf.Clamp(Time.deltaTime * _force * transform.forward.x * dir
                    , -_maxSpeed, _maxSpeed),
                    y,
                    Mathf.Clamp(Time.deltaTime * _force * transform.forward.z * dir
                    , -_maxSpeed, _maxSpeed));

                }
                else
                {
                    EnterCombat();
                }
            }
            else
            {
                if (_movePointsLeft > 0)
                {
                    EnterMoving();
                }
            }
        }

        public void RotateOnAxis(float dir)
        {
            transform.Rotate(0, dir * _rSpeed, 0, Space.Self);
        }

        public void Jump()
        {
            if (State == Mode.move)
            {
                if (_movePointsLeft > 0 && IsGrounded())
                {
                    _rb.AddForce(Vector3.up * _jumpVelocity, ForceMode.Impulse);
                    _movePointsLeft--;
                }
                else
                {
                    EnterCombat();
                }
            }
            else
            {
                if (_movePointsLeft > 0)
                {
                    EnterMoving();
                }
            }
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, _distToGround + 0.1f);
        }
    }
}