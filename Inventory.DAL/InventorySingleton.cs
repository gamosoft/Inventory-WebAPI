using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.DAL
{
    /// <summary>
    /// Inventory defined as a singleton class to avoid threading issues
    /// </summary>
    public sealed class InventorySingleton
    {
        #region "Variables"

        private static volatile InventorySingleton instance;
        private static object syncRoot = new Object();
        private List<Item> _repository;

        #endregion "Variables"

        #region "Properties"

        /// <summary>
        /// Gets instance of the inventory
        /// </summary>
        public static InventorySingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new InventorySingleton();
                    }
                }

                return instance;
            }
        }

        #endregion "Properties"

        #region "Methods"

        /// <summary>
        /// Default constructor to initialize internal repository
        /// </summary>
        private InventorySingleton()
        {
            _repository = new List<Item>();
        }

        /// <summary>
        /// Adds an item to the inventory
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>Empty if all went well, otherwise error message</returns>
        public string Add(Item item)
        {
            if (item.Expiration <= DateTime.Now)
            {
                return "Item already expired";
            }
            // Simple lowercase check to see that label doesn't exist
            if (_repository.Any(i => i.Label.ToLower() == item.Label.ToLower()))
            {
                return "Label already exists";
            }
            _repository.Add(item);
            return String.Empty;
        }

        /// <summary>
        /// Gets all the items in the inventory
        /// </summary>
        /// <returns>Enumerable collection of items in the inventory</returns>
        public IEnumerable<Item> GetAll()
        {
            return _repository;
        }

        public Item GetByLabel(string label)
        {
            // TODO: Mays/mins.
            var result = _repository.FirstOrDefault(i => i.Label.ToLower() == label.ToLower());
            if (result != null)
            {
                // TODO: Make it atomic
                _repository.Remove(result);
                // TODO: If notification in log => do all operations in a TRANSACTION
                NotificationManager.SendNotification(String.Format("Item '{0}' removed", result.Label));
            }


            return result;
        }

        /// <summary>
        /// Sample method for testing purposes
        /// </summary>
        public void InitializeDummyData()
        {
            _repository.Add(new Item() { ID = Guid.NewGuid(), Label = "item1", Expiration = DateTime.Now.AddMinutes(5), Type = ItemType.TypeA });
            _repository.Add(new Item() { ID = Guid.NewGuid(), Label = "item2", Expiration = DateTime.Now.AddMinutes(10), Type = ItemType.TypeB });
            _repository.Add(new Item() { ID = Guid.NewGuid(), Label = "item3", Expiration = DateTime.Now.AddMinutes(15), Type = ItemType.TypeC });
        }

        #endregion "Methods"
    }
}