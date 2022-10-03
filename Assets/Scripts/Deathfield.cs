using UnityEngine;

public class Deathfield : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player"){
            other.transform.GetComponent<Controllers.CharacterScript>().Hit(1000,
            other.transform.position);
        }
    }
}
