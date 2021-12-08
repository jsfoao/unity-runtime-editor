using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler
{
    public Stack<Command> CommandStack;

    public CommandHandler()
    {
        CommandStack = new Stack<Command>();
    }

    public void Undo()
    {
        if (EmptyStack()) { return; }
        LastCommand().Undo();
        RemoveLastCommand();
    }
    
    public bool EmptyStack() { return CommandStack.Count == 0; }
    
    public Command LastCommand() { return CommandStack.Peek(); }
    
    public Command RemoveLastCommand() { return CommandStack.Pop(); }
}

[Serializable]
public class Control
{
    public KeyCode KeyCode;
    public Command Command;

    public Control(KeyCode keyCode, Command command)
    {
        KeyCode = keyCode;
        Command = command;
    }
}
