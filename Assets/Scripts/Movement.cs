using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    public float speed = 9.0f;
    private float gravity = -9.8f;
    private Vector3? dashVector = null;
    CharacterController controller;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!gameObject.GetComponent<PlayerCombatController>().alive) {
            return;
        }
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), gravity, Input.GetAxis("Vertical"));
        movement *= speed * Time.deltaTime;

        if (dashVector != null) {
            movement = (Vector3)dashVector;
        } else if (Input.GetKeyDown(KeyCode.LeftShift) ) {
            StartCoroutine(dash(movement));
        } else {
            movement = Vector3.ClampMagnitude(movement, speed);
        }

        movement = transform.TransformDirection(movement);
        controller.Move(movement);
    }

    IEnumerator dash(Vector3 movement) {
        dashVector = movement * 5;
        yield return new WaitForSeconds(0.1f);
        dashVector = null;
    } 
}
