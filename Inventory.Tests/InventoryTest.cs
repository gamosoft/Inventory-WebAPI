using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inventory.WebAPI.Controllers;
using System.Collections;
using System.Collections.Generic;

namespace Inventory.Tests
{
    [TestClass]
    public class InventoryTest
    {
        [TestMethod]
        public void GetAllItems()
        {
            var controller = new InventoryController();
            var result = controller.Get() as List<DAL.Item>; // TODO: Check cast
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 3);
        }

        [TestMethod]
        public void GetSingleItem()
        {
            var controller = new InventoryController();
            var result = controller.Get("item1") as Inventory.DAL.Item;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Label, "item1");
        }

        [TestMethod]
        public void PostSingleItem()
        {
            var controller = new InventoryController();
            var sample = new DAL.Item() {
                ID = Guid.NewGuid(),
                Label = "Sample item",
                Expiration = DateTime.Now.AddMinutes(5),
                Type = DAL.ItemType.TypeB
            };
            controller.Post(sample);
            //Assert.IsNotNull(result);
            //Assert.AreEqual(result.Label, "item1");
        }

    }
}