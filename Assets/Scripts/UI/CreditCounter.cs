using TMPro;
using UnityEngine;

public class CreditCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _creditCountText;
    [SerializeField] private int _moneyCount;

    public int MoneyCount => _moneyCount;

    public void AddMoney(int money)
    {
        if (money <= 0) throw new System.InvalidOperationException("Начисленны отрицательные деньги");

        _moneyCount += money;

        _creditCountText.text = _moneyCount.ToString();
    }

    public void DecreaseMoney(int money)
    {
        if (money > _moneyCount) throw new System.InvalidOperationException("Недостаточно средств");

        _moneyCount -= money;
        _creditCountText.text = _moneyCount.ToString();
    }
}
