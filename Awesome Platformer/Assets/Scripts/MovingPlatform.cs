using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] List<Transform> positionList;

    int activePosIndex;
    int speed = 3;

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (positionList[activePosIndex].position - transform.position).normalized;
        transform.position = transform.position + dir * speed * Time.deltaTime;

        if (ReachedTarget())
        {
            activePosIndex++;
            activePosIndex = activePosIndex % positionList.Count;
        }
    }

    private bool ReachedTarget()
    {
        return Vector3.Distance(transform.position, positionList[activePosIndex].position) < 0.1f;
    }
}
