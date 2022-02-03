using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private float offset;
    private Transform playerPos;
    private bool canFollow;

    private void Start()
    {
        canFollow = false;
    }

    private void FixedUpdate()
    {
        if (!playerPos || !canFollow) return;
        Vector3 temp = transform.position;
        temp.x = playerPos.position.x + offset;
        transform.position = temp;

        float offsetY = transform.position.y - playerPos.position.y;
        if(Mathf.Abs(offsetY) > 8f)
        {
            StartCoroutine(MoveVertical());
        }
    }

    IEnumerator MoveVertical()
    {
        float offsetY = transform.position.y - playerPos.position.y;
        while(Mathf.Abs(offsetY) > 0.1f)
        {
            offsetY -= Time.deltaTime * 0.32f * offsetY;
            transform.Translate(0, -Time.deltaTime * 0.32f * offsetY, 0);
            yield return null;
        }
    }

    public void StartFollowing()
    {
        canFollow = true;
        playerPos = GameObject.Find("Circle").transform;
        offset = transform.position.x - playerPos.position.x;
    }

    public void StopFollowing()
    {
        canFollow = false;
    }
}
