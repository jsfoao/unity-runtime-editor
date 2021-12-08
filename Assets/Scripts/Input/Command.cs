using System;
using UnityEngine.Events;

[Serializable]
public class Command
{
    public CommandHandler CommandHandler;
    public UnityEvent OnExecute;
    public UnityEvent OnUndo;
    public bool Undoable;

    public Command(CommandHandler commandHandler)
    {
        CommandHandler = commandHandler;
    }

    public virtual void Execute()
    {
        OnExecute.Invoke();
        if (Undoable)
        {
            CommandHandler.CommandStack.Push(this);
        }
    }

    public virtual void Undo()
    {
        OnUndo.Invoke();
    }
}

// Execute
// if (Undoable) { CommandHandler.CommandStack.Push(this); }

// Undo
// if (CommandHandler.EmptyStack()) { return; }
// Command command = CommandHandler.LastCommand();
// command.Undo();
// CommandHandler.RemoveLastCommand();
#if comments
// public class MoveCommand : Command
// {
//     public Vector3 Direction;
//     
//     public MoveCommand(Vector3 direction)
//     {
//         Undoable = true;
//         Direction = direction;
//     }
//     
//     public override void Execute()
//     {
//         Vector3 newPosition =  PlayerProfile.Instance.transform.position + Direction;
//         PlayerProfile.Instance.PlayerController.MoveTo(newPosition);
//     }
//
//     public override void Undo()
//     {
//         MoveCommand lastMoveCommand = (MoveCommand)CommandStack.Peek();
//         Vector3 previousPosition = PlayerProfile.Instance.transform.position - lastMoveCommand.Direction;
//         PlayerProfile.Instance.PlayerController.MoveTo(previousPosition);
//     }
// }

// public class ScaleCommand : Command
// {
//     public Vector3 Scale;
//
//     public ScaleCommand(Vector3 scale)
//     {
//         Undoable = true;
//         Scale = scale;
//     }
//
//     public override void Execute()
//     {
//         Vector3 currentScale = PlayerProfile.Instance.PlayerRenderer.Model.transform.localScale;
//         Vector3 newScale = currentScale + Scale;
//         PlayerProfile.Instance.PlayerRenderer.ChangeScale(newScale);
//     }
//
//     public override void Undo()
//     {
//         ScaleCommand lastScaleCommand = (ScaleCommand)CommandStack.Peek();
//         Vector3 currentScale = PlayerProfile.Instance.PlayerRenderer.Model.transform.localScale;
//         Vector3 previousScale = currentScale - lastScaleCommand.Scale;
//         PlayerProfile.Instance.PlayerRenderer.ChangeScale(previousScale);
//     }
// }

// public class UndoCommand : Command
// {
//     public UndoCommand()
//     {
//         Undoable = false;
//     }
//
//     public override void Execute()
//     {
//         UndoCommand();
//     }
// }
#endif