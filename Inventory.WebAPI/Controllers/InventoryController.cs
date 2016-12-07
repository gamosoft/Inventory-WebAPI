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
    // TODO: Host it in Azure and have Runscope???
    /// </summary>
    public class InventoryController : ApiController
    {
        #region "Methods"

        /// <summary>
        /// Private static constructor used to initialize in memory inventory with data on first run
        /// </summary>
        static InventoryController()
        {
            InventorySingleton.Instance.Initialize();
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
        public Item Get([FromUri(Name = "id")]string label)
        {
            // TODO: Encode/decode, blank spaces
            // TODO: Cache
            if (String.IsNullOrEmpty(label))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest); // TODO: Check response
            }
            else
            {
                var result = InventorySingleton.Instance.GetByLabel(label);
                if (result == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return result;
            }
        }

        // POST api/<controller>
        public void Post([FromBody]Item value)
        {
            // TODO: Race condition if adding expiration in, say, 1 second and it expires while web request is being processed

            InventorySingleton.Instance.Add(value);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]Item value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        #endregion "Methods"
    }
}