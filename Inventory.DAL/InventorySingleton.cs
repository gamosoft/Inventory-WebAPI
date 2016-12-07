﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

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
        private static volatile MemoryCache cache;

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
            cache = new MemoryCache("Inventory");
            InitializeDummyData();
        }

        /// <summary>
        /// Adds an item to the inventory
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>Empty if all went well, otherwise error message</returns>
        public string Add(Item item)
        {
            lock (syncRoot)
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
                CacheItemPolicy policy = new CacheItemPolicy()
                {
                    AbsoluteExpiration = new DateTimeOffset(item.Expiration),
                    RemovedCallback = CachedItemRemovedCallback
                };
                cache.Add(item.Label, item, policy);
            }
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

        /// <summary>
        /// Attempts to retrieve an item from the inventory by its label.
        /// If found, also sends a notification.
        /// </summary>
        /// <param name="label">Label of the item to look for</param>
        /// <returns>Item or null if not found</returns>
        public Item GetByLabel(string label)
        {
            Item result;
            lock (syncRoot)
            {
                result = _repository.FirstOrDefault(i => i.Label.ToLower() == label.ToLower());
                if (result != null)
                {
                    _repository.Remove(result);
                    // Remove from cache as well!
                    cache.Remove(result.Label); // TODO: If manually removed also triggers an "expired" notification
                    NotificationManager.SendNotification(String.Format("Item '{0}' removed", result.Label));
                }
            }
            return result;
        }

        /// <summary>
        /// When an item gets removed from the cache (expires), send a notification
        /// </summary>
        /// <param name="arguments">CacheEntryRemovedArguments</param>
        private void CachedItemRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            NotificationManager.SendNotification(String.Format("Item '{0}' expired!!!", arguments.CacheItem.Key));
        }

        /// <summary>
        /// Sample method for testing purposes
        /// </summary>
        public void InitializeDummyData()
        {
            this.Add(new Item()
            {
                Label = "item1",
                Expiration = DateTime.Now.AddSeconds(20),
                Type = ItemType.TypeA
            });
            this.Add(new Item()
            {
                Label = "item2",
                Expiration = DateTime.Now.AddSeconds(30),
                Type = ItemType.TypeB
            });
            this.Add(new Item()
            {
                Label = "item3",
                Expiration = DateTime.Now.AddSeconds(40),
                Type = ItemType.TypeC
            });
        }

        #endregion "Methods"
    }
}