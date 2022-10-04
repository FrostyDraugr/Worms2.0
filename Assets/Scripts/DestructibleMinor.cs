using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DestructibleMinor : MonoBehaviour
{
    [SerializeField]
    private Managers.GameManager _gm;

    [SerializeField]
    private Material[] _material;
    Rigidbody _rb;

    [SerializeField]
    private float _life;
    private bool _destroyed;

    void Awake()
    {
        _destroyed = false;
        _gm = Managers.GameManager._gameManager;
        _rb = gameObject.GetComponent<Rigidbody>();
    }
    public void Hit(float dmg, Vector3 point)
    {
        _life -= dmg;
        if (_destroyed == false && _life <= 0)
        {
            _destroyed = true;
            if (_gm.Destroyables < 1024 && _gm.DestroyBool)
            {
                //Spawn effects here
                _rb.isKinematic = false;
                _gm.DestroyBool = false;
                _rb.AddForce((point - transform.position) * (dmg * 4));
                _gm.Destroyables++;
                gameObject.layer = LayerMask.NameToLayer("Destroyed");
                Destroy(gameObject, 6);
            }
            else
            {
                _gm.DestroyBool = true;
                _gm.Destroyables++;
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        _gm.Destroyables--;
    }
}