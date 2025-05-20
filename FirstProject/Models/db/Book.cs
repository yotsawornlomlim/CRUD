using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace FirstProject.Models.db;

public partial class Book
{
    public string BookId { get; set; } = null!;

    public string BookName { get; set; } = null!;

    public int CategoryId { get; set; }

    public int PublishId { get; set; }

    public string Isbn { get; set; } = null!;

    public double BookCost { get; set; }

    public double? BookPrice { get; set; }
    public virtual Category Category { get; set; } = null!;
    public virtual Publish Publish { get; set; } = null!;
}
