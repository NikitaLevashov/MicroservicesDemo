using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProductService.Domain.ValueObjects;


public readonly record struct Price
{
    public decimal Value { get; }

    public Price(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Price must be > 0");

        Value = value;
    }
}





