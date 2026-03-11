using System;
using System.Collections.Generic;
using System.Text;

public class InventoryUI
{
    public void ItemChanged(string name, int oldCount, int newCount)
    {
        Console.WriteLine($"[UI] {name}: {oldCount} -> {newCount}");
    }
}