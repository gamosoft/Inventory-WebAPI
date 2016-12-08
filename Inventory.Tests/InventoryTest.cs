using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inventory.WebAPI.Controllers;
using System.Collections;
using System.Collections.Generic;
using Inventory.DAL;
using System.Web.Http;

namespace Inventory.Tests
{
    /// <summary>
    /// Unit test class
    /// </summary>
    [TestClass]
    public class InventoryTest
    {
        #region "Methods"

        /// <summary>
        /// Method to populate inventory with sample data for every test to be executed
        /// </summary>
        [TestInitialize]
        public void InitTests()
        {
            InventorySingleton.Instance.Add(new Item()
            {
                Label = "item1",
                Expiration = DateTime.Now.AddSeconds(30),
                Type = ItemType.TypeA
            });
            InventorySingleton.Instance.Add(new Item()
            {
                Label = "item2",
                Expiration = DateTime.Now.AddMinutes(10),
                Type = ItemType.TypeB
            });
            InventorySingleton.Instance.Add(new Item()
            {
                Label = "item3",
                Expiration = DateTime.Now.AddMinutes(15),
                Type = ItemType.TypeC
            });
        }

        /// <summary>
        /// Test to try to get all dummy items
        /// </summary>
        [TestMethod]
        public void GetAllItems()
        {
            var controller = new InventoryController();
            var result = controller.Get() as ICollection<Item>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 3);
        }

        /// <summary>
        /// Test to get one single item
        /// </summary>
        [TestMethod]
        public void GetSingleItemOK()
        {
            var controller = new InventoryController();
            var result = controller.Get("item1") as Item;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Label, "item1");
        }

        /// <summary>
        /// Test to attempt to get a non-existing item
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetSingleItemErrorNotFound()
        {
            var controller = new InventoryController();
            var result = controller.Get("doesntexist");
        }

        /// <summary>
        /// Test to add an item correctly
        /// </summary>
        [TestMethod]
        public void PostSingleItemOK()
        {
            var controller = new InventoryController();
            var sample = new Item() {
                Label = "Sample item",
                Expiration = DateTime.Now.AddMinutes(5),
                Type = ItemType.TypeB
            };
            controller.Post(sample);
            var result = controller.Get();
            result = result as ICollection<Item>;
            Assert.IsNotNull(result);
            // Assert.AreEqual(result.Count, 4);
        }

        /// <summary>
        /// Test to attempt to add an already expired item
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void PostSingleItemErrorExpiredItem()
        {
            var controller = new InventoryController();
            var sample = new Item()
            {
                Label = "Sample item",
                Expiration = DateTime.Now.AddMinutes(-5),
                Type = ItemType.TypeB
            };
            controller.Post(sample);
        }

        /// <summary>
        /// Test to attempt to add an already expired item
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void PostSingleItemErrorLabelEmtpy()
        {
            var controller = new InventoryController();
            var sample = new Item()
            {
                Label = String.Empty,
                Expiration = DateTime.Now.AddMinutes(5),
                Type = ItemType.TypeB
            };
            controller.Post(sample);
        }

        /// <summary>
        /// Test to attempt to add an item with a duplicate label
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void PostSingleItemErrorDuplicateItem()
        {
            var controller = new InventoryController();
            var sample = new Item()
            {
                Label = "item1",
                Expiration = DateTime.Now.AddMinutes(5),
                Type = ItemType.TypeB
            };
            controller.Post(sample);
        }

        #endregion "Methods"
    }
}