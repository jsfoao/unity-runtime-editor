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

// [CustomEditor(typeof(InputEntity))]
// public class InputEditor : Editor
// {
//     private InputEntity _inputEntity;
//     private void OnEnable()
//     {
//         _inputEntity = (InputEntity)target;
//     }
//
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();
//         GUILayout.BeginHorizontal();
//         if (GUILayout.Button("+"))
//         {
//             Command command = new Command(_inputEntity.CommandHandler);
//             _inputEntity.AddControl(KeyCode.W, command);
//         }
//         if (GUILayout.Button("-"))
//         {
//         }
//         GUILayout.EndHorizontal();
//     }
// }
