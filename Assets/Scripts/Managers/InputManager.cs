using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        GameManager _gm;
        Controllers.CameraController _cm;
        private float _vertical;
        private float _horizontal;
        private void Start()
        {
            _gm = Managers.GameManager._gameManager;
            _cm = Controllers.CameraController._cC;
        }
        void Update()
        {
            _vertical = Input.GetAxisRaw("Vertical");
            _horizontal = Input.GetAxisRaw("Horizontal");
            float zoomDelta = Input.GetAxis("Mouse ScrollWheel");

            if (zoomDelta != 0f)
            {
                _cm.AdjustZoom(-zoomDelta);
            }

            if (_gm.GameLive == true)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    _gm._cs.Attack();
                }

                if (Input.GetButtonDown("Fire2"))
                {
                    _gm._cs.SwitchWeapon();
                }

                if (Input.GetButtonDown("Jump"))
                {
                    _gm._cs.Jump();
                }

            }
        }

        private void FixedUpdate()
        {
            //Change so everything is inside if _gm.GameLive == true
            if (_gm.GameLive == true)
            {
                if (_vertical != 0)
                {
                    _gm._cs.MoveVertical(_vertical);
                }

                if (_horizontal != 0)
                {
                    _gm._cs.RotateOnAxis(_horizontal);
                }
            }
        }

    }
}
