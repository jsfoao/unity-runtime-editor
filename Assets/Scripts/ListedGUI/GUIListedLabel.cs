using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// To use OnGUI()
public class GUIListedLabel
{
    public Vector2 Position;
    public float LineOffset;
    public float IndentOffset;
    public string Header;
    public Dictionary<string, GUILabelItem> Items;

    public GUIListedLabel(Vector2 position, string header = "Header", float lineOffset = 20, float indentOffset = 10)
    {
        Items = new Dictionary<string, GUILabelItem>();
        Position = position;
        LineOffset = lineOffset;
        IndentOffset = indentOffset;
        Header = header;
    }

    public GUILabelItem CreateItem(string text, string value = "")
    {
        GUILabelItem item = new GUILabelItem(text, value);
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
            Vector2 position = new Vector2(Position.x + IndentOffset, Position.y + ((i + 1) * LineOffset));
            GUIItem item = Items.ElementAt(i).Value;
            item.Draw(position);
        }
    }
}

public abstract class GUIItem
{
    public virtual void Draw(Vector2 position) { }
}

public class GUILabelItem : GUIItem
{
    public string Text;
    public string Value;
    
    public GUILabelItem(string text, string value = "")
    {
        Text = text;
        Value = value;
    }

    public override void Draw(Vector2 position)
    {
        string text = $"{Text}: {Value}";
        GUI.Label(new Rect(position, new Vector2(500, 100)), text);
    }
}

public class GUIButtonItem : GUIItem
{
    public string Text;
    public bool Clicked;
    public GUIButtonItem(string text)
    {
        Text = text;
    }

    public override void Draw(Vector2 position)
    {
        Clicked = GUI.Button(new Rect(position, new Vector2(100, 20)), Text);
    }
}