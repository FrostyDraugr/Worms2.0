using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace WeaponSystems{
public class Explosive : Damaging
{
    protected float _radius;

    public void SetUp(float damage, float lifetime, float radius){
        _damage = damage;
        _lifeDuration = lifetime;
        _radius = radius;
    }

    public IEnumerator Throw(){
        if (_hit == false){
            _hit = true;
        yield return new WaitForSeconds(_lifeDuration);
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);
        
         foreach(Collider c in colliders){
            if (c.tag == "DestructibleParent"){
                var script = c.GetComponent<Destructible>();
                script.Hit();
            }
        }
 
        colliders = Physics.OverlapSphere(transform.position, _radius);
        //HashSet<GameObject> damaged = new HashSet<GameObject>();
        foreach(Collider c in colliders){
            if (c.tag == "DestructibleMinor"){
                var script = c.GetComponent<DestructibleMinor>();
                script.Hit(_damage, transform.position);
            }

            if (c.tag == "Player"){ //&& !damaged.Contains(c.gameObject)){
                var script = c.GetComponent<Controllers.CharacterScript>();
                script.Hit(_damage, transform.position);
            }
        }
        
        Destroy(gameObject);
        }
    }
}
}