using System;
using System.Collections.Generic;
using System.Text;

public class Inventory
{
    public event Action<string, int, int> ItemChanged;
    private Dictionary<string, int> storage;

    public Inventory()
    {
        storage = new Dictionary<string, int>();
    }
    public void AddItem(string name, int count)
    {
        if (!storage.ContainsKey(name))
        {
            storage.Add(name, 0);
        }
        int oldCount = storage[name];
        storage[name] += count;
        ItemChanged?.Invoke(name, oldCount, storage[name]);
    }

    public void RemoveItem(string name, int count)
    {
        if (storage.ContainsKey(name))
        {
            int oldCount = storage[name];
            storage[name] -= count;
            if (storage[name] < 0)
            {
                storage[name] = 0;
            }
            ItemChanged?.Invoke(name, oldCount, storage[name]);
        }
    }
}