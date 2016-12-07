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
                ID = Guid.NewGuid(),
                Label = "item1",
                Expiration = DateTime.Now.AddMinutes(5),
                Type = ItemType.TypeA
            });
            InventorySingleton.Instance.Add(new Item()
            {
                ID = Guid.NewGuid(),
                Label = "item2",
                Expiration = DateTime.Now.AddMinutes(10),
                Type = ItemType.TypeB
            });
            InventorySingleton.Instance.Add(new Item()
            {
                ID = Guid.NewGuid(),
                Label = "item3",
                Expiration = DateTime.Now.AddMinutes(15),
                Type = ItemType.TypeC
            });
        }

        /// <summary>
        /// Test to try to get all items
        /// </summary>
        [TestMethod]
        public void GetAllItems()
        {
            
            var controller = new InventoryController();
            var result = controller.Get() as List<DAL.Item>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 3);
        }

        [TestMethod]
        public void GetSingleItemOK()
        {
            var controller = new InventoryController();
            var result = controller.Get("item1") as Inventory.DAL.Item;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Label, "item1");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetSingleItemErrorNotFound()
        {
            var controller = new InventoryController();
            var result = controller.Get("doesntexist");
        }

        [TestMethod]
        public void PostSingleItemOK()
        {
            var controller = new InventoryController();
            var sample = new DAL.Item() {
                ID = Guid.NewGuid(),
                Label = "Sample item",
                Expiration = DateTime.Now.AddMinutes(5),
                Type = DAL.ItemType.TypeB
            };
            controller.Post(sample);
            var result = controller.Get() as List<DAL.Item>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void PostSingleItemErrorExpiredItem()
        {
            var controller = new InventoryController();
            var sample = new DAL.Item()
            {
                ID = Guid.NewGuid(),
                Label = "Sample item",
                Expiration = DateTime.Now.AddMinutes(-5),
                Type = DAL.ItemType.TypeB
            };
            controller.Post(sample);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void PostSingleItemErrorDuplicateItem()
        {
            var controller = new InventoryController();
            var sample = new DAL.Item()
            {
                ID = Guid.NewGuid(),
                Label = "item1",
                Expiration = DateTime.Now.AddMinutes(5),
                Type = DAL.ItemType.TypeB
            };
            controller.Post(sample);
        }

        #endregion "Methods"
    }
}