using System;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    public CommandHandler CommandHandler;

    [SerializeField] private List<Control> _controls;

    private void Update()
    {
        if (_controls.Count == 0) { return; }
        foreach (Control control in _controls)
        {
            switch (control.Behaviour)
            {
                case BehaviourType.KeyDown:
                {
                    if (Input.GetKeyDown(control.Key))
                    {
                        control.Event.Invoke();
                    }
                    break;
                }
                case BehaviourType.KeyUp:
                {
                    if (Input.GetKeyUp(control.Key))
                    {
                        control.Event.Invoke();
                    }
                    break;
                }
                case BehaviourType.Hold:
                    if (Input.GetKey(control.Key))
                    {
                        control.Event.Invoke();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion
        CommandHandler = new CommandHandler();
    }
}
