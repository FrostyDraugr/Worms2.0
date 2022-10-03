using UnityEngine;

namespace Controllers{
public class CameraController : MonoBehaviour
{
    public static Controllers.CameraController _cC;

    [SerializeField]
    private float _smoothness;

    private Managers.GameManager _gm;

    [SerializeField]
    private Camera _cam;

    private float _zoom;

    [SerializeField]
    private float _stickMinZoom, _stickMaxZoom;

    [SerializeField]
    private float _swivelMinZoom, _swivelMaxZoom;

    private Transform _swivel, _stick;
    private void Awake(){
        if (_cC == null){
            DontDestroyOnLoad(gameObject);
            _cC = this;
                }else if (_cC != this){
                    Destroy(gameObject);
    }
    _zoom = 1f;
    _swivel = transform.GetChild(0);
    _stick = _swivel.transform.GetChild(0);
    _stick.localPosition = new Vector3(0f, 0f, -_stickMaxZoom);
    _swivel.localRotation = Quaternion.Euler(_swivelMaxZoom, 0f, 0f);
    }

    public void AdjustZoom(float delta){
        if (_gm.GameLive){
        _zoom = Mathf.Clamp01(_zoom + delta);

        float distance = Mathf.Lerp(_stickMinZoom, _stickMaxZoom, _zoom);
        _stick.localPosition = new Vector3(0f, 0f, -distance);

        float angle = Mathf.Lerp(_swivelMinZoom, _swivelMaxZoom, _zoom);
        _swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }
    }
    private void Start() {
        _gm = Managers.GameManager._gameManager;
    }

    private void FixedUpdate() {
        if (_gm.GameLive){
            UpdateCameraPos();
            //_cam.transform.LookAt(gameObject.transform.position);
        }
    }

    public void UpdateCameraPos()
    {
        gameObject.transform.position =
        Vector3.Lerp(gameObject.transform.position,
        _gm._cs.transform.position, _smoothness * Time.fixedDeltaTime);
        
        gameObject.transform.rotation =
        Quaternion.Lerp(gameObject.transform.rotation,
        _gm._cs.transform.rotation,_smoothness * Time.fixedDeltaTime);
    }
}
}