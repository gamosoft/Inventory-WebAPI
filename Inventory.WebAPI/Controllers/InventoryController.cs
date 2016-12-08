using Inventory.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Inventory.WebAPI.Controllers
{
    /// <summary>
    /// Controller for Inventory operations
    /// </summary>
    // Uncomment the following line when authentication is put in place to test with Windows credentials
    // [Authorize]
    public class InventoryController : ApiController
    {
        #region "Methods"

        /// <summary>
        /// Private static constructor used to initialize in memory inventory with dummy data on first run if needed
        /// </summary>
        static InventoryController()
        {
        }

        /// <summary>
        /// Gets all items from the inventory
        /// </summary>
        /// <returns>Serialized information of all items in the inventory</returns>
        public IEnumerable<Item> Get()
        {
            return InventorySingleton.Instance.GetAll();
        }

        /// <summary>
        /// Gets an item given its label
        /// Removes the item from the inventory
        /// If not found returns an exception
        /// </summary>
        /// <param name="label">Label of the item</param>
        /// <returns>Item removed from the inventory</returns>
        public Item Get(string label)
        {
            var result = InventorySingleton.Instance.GetByLabel(label);
            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return result;
        }

        /// <summary>
        /// Adds an item to the repository
        /// </summary>
        /// <param name="value">Item to add</param>
        public void Post([FromBody]Item value)
        {
            var result = InventorySingleton.Instance.Add(value);
            if (!String.IsNullOrEmpty(result))
            {
                HttpResponseMessage resp = new HttpResponseMessage((HttpStatusCode)422)
                {
                    Content = new StringContent(result)
                };
                throw new HttpResponseException(resp);
            }
        }

        #endregion "Methods"
    }
}