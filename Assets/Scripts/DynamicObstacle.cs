using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class DynamicObstacle : MonoBehaviour
{
    [SerializeField] private List<Vector3> _path;
    [SerializeField] private float _movementSpeed = 4f;
    int _destinationIndex = 0;

    private void Update()
    {   if(_path.Count != 0)
            FollowThePath();
    }
    void FollowThePath()
    {
        if(transform.position == _path[_destinationIndex])
        {
            _destinationIndex = _destinationIndex == (_path.Count - 1) ? 0 : _destinationIndex + 1;
        }
        transform.position = Vector3.Lerp(transform.position, _path[_destinationIndex], _movementSpeed * Time.deltaTime);
    }
}
