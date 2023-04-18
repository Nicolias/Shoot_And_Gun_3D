using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundMovment : MonoBehaviour
{
    public event Action OnStageChanged;

    [SerializeField] private float _speed;
    [SerializeField] private float _distanceToNextStage;

    private bool _isGroundMoving;

    private const float _revertGroundPosition = 36.75f;

    private float _minGroundPositionByX;
    private float _maxGroundPositionByX;

    private void Start()
    {
        _minGroundPositionByX = transform.position.x;
        _maxGroundPositionByX = transform.position.x + _revertGroundPosition;
    }

    public IEnumerator MoveToNextStage()
    {
        yield return new WaitForSeconds(0.1f);

        if (_isGroundMoving) throw new InvalidOperationException("Движение предыдущей стадии еще не закончено");

        _isGroundMoving = true;

        var nextStagePosition = transform.position.x + _distanceToNextStage;
        while (transform.position.x < nextStagePosition)
        {
            if (transform.position.x >= _maxGroundPositionByX)
            {
                transform.position = new Vector3(_minGroundPositionByX, transform.position.y, transform.position.z);
                nextStagePosition = nextStagePosition - _maxGroundPositionByX + _minGroundPositionByX;
            }

            yield return new WaitForSeconds(0.01f);
            transform.position += Vector3.right * _speed * Time.deltaTime;
        }

        _isGroundMoving = false;
        OnStageChanged?.Invoke();
    }
}
