using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 9.0f;
    private float gravity = -9.8f;
    CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.GetComponent<PlayerCombatController>().alive) {
            return;
        }
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), gravity, Input.GetAxis("Vertical"));
        movement *= (Input.GetKey(KeyCode.LeftShift) ? 2 * speed : speed) * Time.deltaTime;
        movement = transform.TransformDirection(movement);
        movement = Vector3.ClampMagnitude(movement, speed);
        controller.Move(movement);
    }
}
