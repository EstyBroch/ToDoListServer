using System;
using System.Collections.Generic;

namespace ToDoAPI;

public partial class ItemModel
{

public int Id { get; set; }
    public string Name { get; set; } = null!;

    public bool IsComplete { get; set; }
}
