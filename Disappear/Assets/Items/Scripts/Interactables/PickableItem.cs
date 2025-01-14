using UnityEngine;
using WAG.Interactions;
using WAG.Inventory_Items;

namespace WAG.Items
{
    public class PickableItem : Interactable
    {
        private IInventoryController inventory;
        [field: SerializeField] public ItemController ItemController { get; set; }


        protected override void ActionOnInteract(GameObject sender)
        {
            if (sender.TryGetComponent<IInventoryController>(out inventory))
            {
                if (inventory.AddItemToInventory(ItemController))
                    ItemManager.Instance.StoreItem(ItemController);
            }
            else
            {
                Debug.LogError("Can't find player inventory");
            }
        }
    }
}