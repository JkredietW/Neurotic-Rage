using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeaponBullet : MonoBehaviour
{
    public List<GameObject> weaponMeshes;
    GameObject chosenWeapon;
    private void Start()
    {
        chosenWeapon = Instantiate(weaponMeshes[Random.Range(0, weaponMeshes.Count)], transform);
        chosenWeapon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }
    private void Update()
    {
        //randomize the rotation
        chosenWeapon.transform.Rotate(new Vector3(Random.Range(-45f, 45f), Random.Range(-45f, 45f), Random.Range(-45f, 45f)));
    }
}
