using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetroOperaciones.Models
{
    public class ListItem
    {
        public int ListItemID { get; set; }
        public String ListItemNm { get; set; }

        public ListItem(int listItemID, String listItemNm)
        {
            this.ListItemID = listItemID;
            this.ListItemNm = listItemNm;
        }
    }
}