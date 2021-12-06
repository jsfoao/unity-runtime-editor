using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUIListedButton
{
    public Vector2 Position;
    public float LineOffset;
    public string Header;
    public Dictionary<string, GUIButtonItem> Items;

    public GUIListedButton(Vector2 position, string header = "Header", float lineOffset = 20)
    {
        Items = new Dictionary<string, GUIButtonItem>();
        Position = position;
        LineOffset = lineOffset;
        Header = header;
    }
    
    public GUIButtonItem CreateItem(string text)
    {
        GUIButtonItem item = new GUIButtonItem(text);
        Items.Add(text, item);
        return item;
    }
    
    public void Draw()
    {
        // Draw Header
        GUI.Label(new Rect(Position, new Vector2(500, 100)), Header);
        // Draw Items
        for (int i = 0; i < Items.Count; i++)
        {
            Vector2 position = new Vector2(Position.x, Position.y + ((i + 1) * LineOffset));
            GUIItem item = Items.ElementAt(i).Value;
            item.Draw(position);
        }
    }
}