using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UIGun : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler 
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Image _gunImage;

    public Transform CurrentSlot { get; set; }

    private StaticData _staticData;

    private int _damage = 3;
    private int _level = 1;

    public int Damage => _damage;
    public int Level => _level;

    private CanvasGroup _canvasGroup;
    private Canvas _mainCanvas;
    private RectTransform _rectTransform;

    [Inject]
    public void Construct(StaticData staticData)
    {
        _staticData = staticData;
    }

    private void Start()
    {
        _levelText.text = _level.ToString();
        _gunImage.sprite = _staticData.GetGunSpriteByLevel(this);

        _rectTransform = GetComponent<RectTransform>();
        _mainCanvas = GetComponentInParent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void LevelUp()
    {
        _levelText.text = _level.ToString();

        _level++;

        if (Damage < 40)
        {
            _damage += 10;
        }
        else
        {
            float newDamage = Damage * 1.2f;
            _damage = (int)newDamage;
        }

        _gunImage.sprite = _staticData.GetGunSpriteByLevel(this);
    }

    public void CopyValues(UIGun uIGun)
    {
        _level = uIGun.Level;
        _damage = uIGun.Damage;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var slotTransform = _rectTransform.parent;
        slotTransform.SetAsLastSibling();
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        _canvasGroup.blocksRaycasts = true;
    }    
}
