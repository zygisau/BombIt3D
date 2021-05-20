using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;

    private CharacterController controller;

    [SerializeField]
    private Vector3 playerVelocity;

    [SerializeField]
    private Vector3 direction;

    private Vector3 lastPos;
    private PlayerControl playerControl;
    public float horizontal;
    public float horizontalCustom;
    public Vector3 moveCustom;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        playerControl = SettingsManager.Instance.GetPlayerControlByIdx(gameObject.GetComponent<Player>().GetId());
        lastPos = controller.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(getHorizontalAxis(), 0, getVerticalAxis());
        controller.SimpleMove(move * playerSpeed);
        horizontal = Input.GetAxis("Horizontal");
        horizontalCustom = getHorizontalAxis();
        moveCustom = move;
        playerVelocity = controller.velocity;

        if (move != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(move);
            direction = extractUnitVector(move);
        }
    }

    private float getHorizontalAxis()
    {
        float axis = 0;
        if (Input.GetKey(playerControl.Left))
        {
            axis = -1f;
        }
        if (Input.GetKey(playerControl.Right))
        {
            axis = 1f;
        }
        return axis;
    }

    private float getVerticalAxis()
    {
        float axis = 0;
        if (Input.GetKey(playerControl.Down))
        {
            axis = -1f;
        }
        if (Input.GetKey(playerControl.Up))
        {
            axis = 1f;
        }
        return axis;
    }

    private Vector3 extractUnitVector(Vector3 vec)
    {
        int idx = 0;
        float max = vec[idx];
        for (int i = 1; i < 3; i++)
        {
            if (Math.Abs(vec[i]) > Math.Abs(max))
            {
                idx = i;
                max = vec[idx];
            }
        }
        Vector3 newVec = Vector3.zero;
        newVec[idx] = max > 0 ? 1 : -1;
        return newVec;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }
}
