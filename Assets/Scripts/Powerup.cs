
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class Powerup : MonoBehaviour {

    private Rigidbody powerupRb;
    private Collider powerupCollider;
    public float speed;
    public string powerupType;
    public GameObject activeShieldPrefab;
    public GameObject player;
    GameObject activeShield;
    [SerializeField]
    public static List<GameObject> obtainedPowerups;
    public static Boolean powerupActive;
    public GameObject slowmoAnim;
    public GameObject shieldAnim;

    void Start() {
        powerupActive = false;
        powerupRb = GetComponent<Rigidbody>();
        powerupCollider = GetComponent<Collider>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update() {

        powerupRb.AddForce(new Vector3(0, 0, -speed) * Time.deltaTime);
        if (powerupRb.transform.position.y <= 3) {
            speed = 100;
        }
    }
    public void Activate(GameObject player) {
        powerupActive = true;
        switch (obtainedPowerups[0].GetComponent<Powerup>().powerupType) {
            case "Shield": ActivateShield(player); break;
            case "SlowMotion": Debug.Log("SlowMotion"); ActivateSlowMotion(); break;
            default: Debug.Log("Nothing"); break;

        }
        obtainedPowerups.RemoveAt(0);

    }
    void ActivateShield(GameObject player) {

        activeShield = Instantiate(activeShieldPrefab, player.transform.position + new Vector3(0, 1, 1), Quaternion.Euler(new Vector3(0, 180, 0)));
        GameObject shieldObject = Instantiate(shieldAnim, player.transform.position + new Vector3(0, 1, 1), Quaternion.Euler(new Vector3(0, 180, 0)));
        Coroutine shieldTimer = StartCoroutine(DeactivateShield(shieldObject));



    }
    public IEnumerator DeactivateShield(GameObject shieldObject) {

        yield return new WaitForSeconds(3f);
        Destroy(shieldObject);
        DestroyImmediate(activeShield);

        powerupActive = false;
    }
    public void ObtainPowerup(GameObject type) {
        obtainedPowerups.Add(type);
        Debug.Log("added");
        Debug.Log(string.Join(", ", obtainedPowerups));

    }
    void ActivateSlowMotion() {
        PlayerController playerController = player.GetComponent<PlayerController>();
        Animator playerAnim = player.GetComponent<Animator>();

        //Activate Slowmo
        Time.timeScale = 0.2f;
        playerController.speed *= 4;
        playerAnim.speed = 4;
        GameObject slowMotionObject = Instantiate(slowmoAnim, player.transform.position + new Vector3(0, 1, 1), Quaternion.Euler(new Vector3(0, 180, 0)));

        //Deactivate after delay
        Coroutine slowmoTimer = StartCoroutine(DeactivateSlowmotion(playerController,playerAnim,slowMotionObject));
        

    }
    private IEnumerator DeactivateSlowmotion(PlayerController playerController,Animator playerAnim , GameObject slowMotionObject) {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1f;
        playerController.speed /= 4;
        playerAnim.speed = 1;
        DestroyImmediate(slowMotionObject);
        powerupActive = false;
    }
    

}
