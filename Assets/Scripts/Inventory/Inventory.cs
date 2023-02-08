using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Inventory : MonoBehaviour
{
    public event Action OnGunLevelUped;

    [SerializeField] private UIGun _uIGunTemplate;

    [SerializeField] private Button _buyButton;
    [SerializeField] private TMP_Text _buyButtonText;
    [SerializeField] private List<UISlots> _slots;

    private List<UIGun> _gunList = new();

    private DiContainer _diContainer;

    private CreditCounter _creditCounter;
    private int _currentBuyPrice = 50;

    public UIGun CurrentBestUIGun
    {
        get 
        {
            UIGun bestGun = null;

            foreach (var gun in _gunList)
            {
                if (bestGun == null)
                    bestGun = gun;

                if (gun.Level > bestGun.Level)
                    bestGun = gun;
            }

            return bestGun;
        }
    }

    [Inject]
    public void Construct(DiContainer diContainer, CreditCounter creditCounter)
    {
        _diContainer = diContainer;
        _creditCounter = creditCounter;
    }

    private void Start()
    {
        CreatNewGun();
        OnGunLevelUped?.Invoke();
    }

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(BuyNewGun);
        _buyButtonText.text = _currentBuyPrice.ToString();
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveAllListeners();
    }

    public UIGun GetMergedGun(UIGun firstUiGun, UIGun secondJIGun)
    {
        _gunList.Remove(firstUiGun);
        _gunList.Remove(secondJIGun);

        var newUIGun = _diContainer.InstantiatePrefabForComponent<UIGun>(_uIGunTemplate, firstUiGun.CurrentSlot);
        newUIGun.CurrentSlot = firstUiGun.CurrentSlot;
        newUIGun.CopyValues(firstUiGun);
        newUIGun.LevelUp();

        _gunList.Add(newUIGun);

        OnGunLevelUped?.Invoke();

        Destroy(firstUiGun.gameObject);
        Destroy(secondJIGun.gameObject);

        newUIGun.GetComponent<CanvasGroup>().blocksRaycasts = true;

        return newUIGun;
    }

    private void BuyNewGun()
    {
        if (_creditCounter.MoneyCount < _currentBuyPrice | _gunList.Count >= _slots.Count)
            return;

        _creditCounter.DecreaseMoney(_currentBuyPrice);

        _currentBuyPrice *= 2;

        _buyButtonText.text = _currentBuyPrice.ToString();

        CreatNewGun();
    }

    private void CreatNewGun()
    {
        UIGun newUIGun = null;

        foreach (var slot in _slots)
        {
            if (slot.GetComponentInChildren<UIGun>() == null)
            {
                newUIGun = _diContainer.InstantiatePrefabForComponent<UIGun>(_uIGunTemplate, slot.transform);
                newUIGun.CurrentSlot = slot.transform;
                break;
            }
        }

        if (newUIGun == false) return;

        _gunList.Add(newUIGun);
    }
}
