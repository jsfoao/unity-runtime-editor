using System;
using System.Collections.Generic;
using UnityEngine;

public class InputEntity : MonoBehaviour
{
    public CommandHandler CommandHandler;
    [NonSerialized]public List<Control> Controls;
    [SerializeField] private List<Control> TempControls;

    public void Undo()
    {
        CommandHandler.Undo();
    }
    
    public Control AddControl(KeyCode keyCode, Command command)
    {
        Control control = new Control(keyCode, command);
        control.Command.CommandHandler = CommandHandler;
        Controls.Add(control);
        return control;
    }
    
    private void Update()
    {
        foreach (Control control in Controls)
        {
            if (Input.GetKeyDown(control.KeyCode))
            {
                control.Command.Execute();
            }
        }
    }

    private void Awake()
    {
        Controls = new List<Control>();
        CommandHandler = new CommandHandler();
        foreach (Control control in TempControls)
        {
            AddControl(control.KeyCode, control.Command);
        }
    }
}
