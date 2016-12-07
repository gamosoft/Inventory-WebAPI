using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL
{

    /// <summary>
    /// Class to hold item data
    /// </summary>
    public class Item
    {
        #region "Properties"

        /// <summary>
        /// Gets/sets the ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Gets/sets the label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets/sets the expiration of the item
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Gets/sets the type of the item
        /// </summary>
        public ItemType Type { get; set; }

        #endregion "Properties"
    }
}