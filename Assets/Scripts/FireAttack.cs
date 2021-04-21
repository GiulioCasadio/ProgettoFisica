using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawn;
    public int bulletSpeed;
    public TextMeshProUGUI mirino;
    public AudioSource fireSound;

    private Animator anim;
    private bool aiming = false;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        mirino.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            anim.ResetTrigger("NotActive");
            anim.SetTrigger("Active");
            aiming = true;
            mirino.gameObject.SetActive(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            anim.ResetTrigger("Active");
            anim.SetTrigger("NotActive");
            aiming = false;
            mirino.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) && aiming)
        {            
            anim.SetTrigger("Fire");
            GameObject bullet = Instantiate(bulletPrefab, spawn.position, spawn.rotation) as GameObject;
            bullet.GetComponent<Rigidbody>().AddForce(spawn.transform.forward * bulletSpeed * Time.deltaTime, ForceMode.Impulse);
            fireSound.Play();
        }
    }
}
