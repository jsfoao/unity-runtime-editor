using System.Collections.Generic;

public class CommandHandler
{
    public Stack<Command> CommandStack;

    public CommandHandler()
    {
        CommandStack = new Stack<Command>();
    }

    public void ExecuteCommand(Command command)
    {
        command.CommandHandler = this;
        command.Execute();
        CommandStack.Push(command);
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