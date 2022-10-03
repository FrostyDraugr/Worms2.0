using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class rocket : MonoBehaviour
{
    Rigidbody _rb;
    float _speed = 20f;
    bool collided = false;

    private void Start() {
        _rb = gameObject.GetComponent<Rigidbody>();
        _rb.AddForce(transform.forward * _speed);
        DestroyObjectDelayed(4);
    }

    private void OnCollisionEnter(Collision other) {
        CheckForDestructibles();        
    }

    void DestroyObjectDelayed(float time){
        Destroy(gameObject, time);
    }

    private void OnDestroy() {
        
    }

    private void CheckForDestructibles(){
        if (collided == false){
        collided = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        
        foreach(Collider c in colliders){
            if (c.tag == "DestructibleParent"){
                var script = c.GetComponent<Destructible>();
                script.Hit();
            }
        }

        colliders = Physics.OverlapSphere(transform.position, 2f);
        foreach(Collider c in colliders){
            if (c.tag == "DestructibleMinor"){
                var script = c.GetComponent<DestructibleMinor>();
                script.Hit(100, transform.position);
            }

            if (c.tag == "Player"){
                var script = c.GetComponent<Controllers.CharacterScript>();
                script.Hit(100, transform.position);
            }
        }
        Destroy(gameObject);
    }
    }
}