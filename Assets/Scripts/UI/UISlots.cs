using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISlots : MonoBehaviour, IDropHandler
{
    [SerializeField] private Inventory _inventory;

    public void OnDrop(PointerEventData eventData)
    {
        var otherItemTransform = eventData.pointerDrag.transform;

        if (otherItemTransform.TryGetComponent(out UIGun gun))
        {
            var gunInSlot = GetComponentInChildren<UIGun>();

            if (gunInSlot != null && gun.Equals(gunInSlot) != true)
            {
                if (gun.Level == gunInSlot.Level)
                    _inventory.GetMergedGun(gunInSlot, gun);
                else
                    return;
            }
            else
            {
                SetPerent(otherItemTransform);
            }
        }
            
    }

    private void SetPerent(Transform otherItemTransform)
    {
        otherItemTransform.SetParent(transform);
        otherItemTransform.localPosition = Vector3.zero;

        otherItemTransform.GetComponent<UIGun>().CurrentSlot = transform;
    }
}
