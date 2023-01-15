using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    public class ActiveText
    {
        public TMP_Text UIText;
        public float MaxTime;
        public Vector3 UnitPosition;

        public float Timer { get; private set; }

        public ActiveText(float maxTime, TMP_Text uiText, Vector3 unitPosition)
        {
            MaxTime = maxTime;
            Timer = maxTime;
            UIText = uiText;
            UnitPosition = unitPosition + Vector3.up;
        }

        public void MoveText(Camera camera)
        {
            float delta = 1.0f - (Timer / MaxTime);
            Vector3 position = UnitPosition + new Vector3(delta, delta, 0.0f);
            position = camera.WorldToScreenPoint(position);
            position.z = 0.0f;

            UIText.transform.position = position;
        }

        public void DecreaseTimerOnValue(float value)
        {
            Timer -= value;
        }
    }

    [SerializeField] private TMP_Text _textPrefab;
    private const int _poolSize = 64;
    private Queue<TMP_Text> _textPool = new();
    private List<ActiveText> _activeTextList = new();

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        
        for (int i = 0; i < _poolSize; i++)
        {
            TMP_Text template = Instantiate(_textPrefab, transform);
            template.gameObject.SetActive(false);
            _textPool.Enqueue(template);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _activeTextList.Count; i++)
        {
            ActiveText activeText = _activeTextList[i];
            activeText.DecreaseTimerOnValue(Time.deltaTime);

            if (activeText.Timer <= 0.0f)
            {
                activeText.UIText.gameObject.SetActive(false);
                _textPool.Enqueue(activeText.UIText);
                _activeTextList.RemoveAt(i);
                --i;
            }
            else
            {
                var color = activeText.UIText.color;
                color.a = activeText.Timer / activeText.MaxTime;
                activeText.UIText.color = color;

                activeText.MoveText(_camera);
            }
        }
    }

    public void AddText(int amountText, Vector3 unitPosition, Color color)
    {
        var template = _textPool.Dequeue();
        template.color = color;
        template.text = amountText.ToString();
        template.gameObject.SetActive(true);

        ActiveText activeText = new(1.0f, template, unitPosition);
        activeText.MoveText(_camera);
        _activeTextList.Add(activeText);
    }
}
