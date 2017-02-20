using System;
using OptionsTradeWell.assistants;

namespace OptionsTradeWell.model
{
    public class TerminalOrder
    {
        //public TerminalOrder(double id, string ticker, TerminalOrderType type, TerminalOrderOperation operation, double price, int quantity)
        //{
        //    Id = id;
        //    Ticker = ticker;
        //    Type = type;
        //    Operation = operation;
        //    Price = price;
        //    Quantity = quantity;
        //}

        public TerminalOrder(double id, string ticker, TerminalOrderType type, TerminalOrderOperation operation, double price, int quantity, TerminalOrderStatus orderStatus)
        {
            Id = id;
            Ticker = ticker;
            Type = type;
            Operation = operation;
            Price = price;
            Quantity = quantity;
            OrderStatus = orderStatus;
        }

        public double Id { get; }
        public string Ticker { get; }

        public TerminalOrderType Type { get; }

        public TerminalOrderOperation Operation { get; }

        public double Price { get; }

        public int Quantity { get; }

        public TerminalOrderStatus OrderStatus { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime LastTime { get; set; }
    }
}