using System;
using OptionsTradeWell.assistants;

namespace OptionsTradeWell.model
{
    public class QuikOrder
    {
        public QuikOrder(double id, string ticker, QuikOrderType type, QuikOrderOperation operation, double price, int quantity)
        {
            Id = id;
            Ticker = ticker;
            Type = type;
            Operation = operation;
            Price = price;
            Quantity = quantity;
        }

        public double Id { get; }
        public string Ticker { get; }

        public QuikOrderType Type { get; }

        public QuikOrderOperation Operation { get; }

        public double Price { get; }

        public int Quantity { get; }

        public QuikOrderStatus OrderStatus { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime LastTime { get; set; }
    }
}