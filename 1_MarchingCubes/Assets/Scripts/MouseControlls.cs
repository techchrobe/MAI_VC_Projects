using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControlls : MonoBehaviour
{
    public Vector2 sensitivity;
    public Vector2 acc;
    public float inputLagPeriod;
    public float maxAngle;
    public float moveSpeed;
    public GameObject marchingCubes;

    private Vector2 velocity;
    private Vector2 rotation;
    private Vector2 lastInputEvent;
    private float inputLagTimer;
    public bool wireframeMode = false;

    // Update is called once per frame
    void Update()
    {
        Vector2 newVelocity = GetInput() * sensitivity;

        velocity = new Vector2(Mathf.MoveTowards(velocity.x, newVelocity.x, acc.x * Time.deltaTime),
                                    Mathf.MoveTowards(velocity.y, newVelocity.y, acc.y * Time.deltaTime));

        rotation += velocity * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, -maxAngle, maxAngle);

        transform.eulerAngles = new Vector3(rotation.y, rotation.x, 0);

        transform.position += transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        transform.position += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;

        if (Input.GetKey(KeyCode.Space)) {
            transform.LookAt(marchingCubes.transform);
        }

        if(Input.GetKeyDown(KeyCode.E)) {
            wireframeMode = !wireframeMode;
        }
    }

    private Vector2 GetInput() {
        inputLagTimer += Time.deltaTime;

        Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (!(Mathf.Approximately(0, input.x) && Mathf.Approximately(0, input.y)) || inputLagTimer >= inputLagPeriod) {
            lastInputEvent = input;
            inputLagTimer = 0;
        }
        return lastInputEvent;
    }

    private void OnPreRender() {
        GL.wireframe = wireframeMode;
    }

    void OnPostRender() {
        GL.wireframe = false;
    }
}
