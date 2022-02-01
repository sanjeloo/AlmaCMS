using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMCartItem
    {
        private VMCartProduct product = new VMCartProduct();

        public VMCartProduct Product
        {
            get { return product; }
            set { product = value; }
        }
        private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }



        public VMCartItem(VMCartProduct vmproduct, int quantity)
        {
            this.product = vmproduct;
            this.quantity = quantity;

        }
    }
}