using UnityEngine;

namespace WeaponSystems{
[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public string Name;
    public int Damage;
    public int Ammo;
    public GameObject Model;
    public Type _Type;
    public enum Type{Gun, Grenade}
    public int Lifetime;

    public float Radius;
}
}