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
        /// Private static constructor used to initialize in memory inventory with dummy data on first run
        /// </summary>
        static InventoryController()
        {
            // TODO: Remove this for deployment
            InventorySingleton.Instance.InitializeDummyData();
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
            if (String.IsNullOrEmpty(label))
            {
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, "Label was null or empty"));
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

            var result = InventorySingleton.Instance.Add(value);
            if (!String.IsNullOrEmpty(result))
            {
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, result));
            }

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