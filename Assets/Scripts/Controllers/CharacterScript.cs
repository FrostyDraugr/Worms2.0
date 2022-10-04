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
        private float _speed;

        [SerializeField]
        private float _rSpeed;

        [SerializeField]
        private float _jumpVelocity;

        private Rigidbody _rb;

        [SerializeField]
        private Transform _ws;

        private float _distToGround;

        private WeaponController _wc;

        public int Id;

        public int TeamId;

        private void Awake()
        {
            State = Mode.inactive;
        }

        private void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _distToGround = gameObject.GetComponent<CapsuleCollider>()
            .bounds.extents.y;
            _gm = Managers.GameManager._gameManager;
            _ws = gameObject.transform.GetChild(0).transform;
            _wc = gameObject.GetComponent<WeaponController>();
            _wc.ChosenWeapon = 0;
        }

        public void Hit(float dmg, Vector3 point)
        {
            _rb.AddForce((transform.position - point + Vector3.up) * (dmg * 4));
            Life -= dmg;
            if (Life <= 0 && State != Mode.dead)
            {
                if (State == Mode.inactive)
                {
                    Managers.EventManager._eventManager
                    .DeathTrigger(Id, TeamId, false);
                }
                else
                {
                    Managers.EventManager._eventManager
                    .DeathTrigger(Id, TeamId, true);
                }
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
            }
            else
            {
                EnterCombat();
            }
        }

        public void SwitchWeapon()
        {
            if (State == Mode.combat)
            {
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

            State = Mode.move;
        }

        public void MoveVertical(float dir)
        {
            if (State == Mode.move)
            {
                if (_movePointsLeft > 0)
                {
                    _rb.AddForce(transform.forward * (dir * Time.deltaTime * _speed), ForceMode.Impulse);
                    _movePointsLeft -= 1 * Time.fixedDeltaTime;

                    float y = _rb.velocity.y;

                    _rb.velocity = _rb.velocity.normalized;

                    _rb.velocity = new Vector3(_rb.velocity.x,
                    y, _rb.velocity.z);

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