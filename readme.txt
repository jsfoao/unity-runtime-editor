João Freire
In-game/Runtime editor for car frame building. Editor is mostly focused on graphs and connecting vertices together.
Heavily inspired by in-game editor in games like Dream Car Builder or Main Assembly.

Patterns:
- Singleton	
	EditorRenderer.cs, class EditorRenderer as EditorRenderer Instance;
	EditorController.cs, class EditorController as EditorController Instance;
	MouseController.cs, class MouseController as MouseController Instance;
	In all these cases, using a singleton made sense since there shouldn't be any more instances of these classes in the editor.
	In the EditorRenderer for example, it shouldn't be possible to have multiple instances rendering the editor grid, vertices or handle axis. These can only be done once and by a single instance. 
	
- Composite
	GraphRenderer.cs, class GraphRenderer
	VertexRenderer.cs, class VertexRenderer
	EdgeRenderer.cs, class EdgeRenderer
	GraphRenderer just like VertexRenderer and EdgeRenderer inherites from the interface IRenderer.
	Used composite pattern in this case since the GraphRenderer can be treated as a single instance.
	Callind the interface's Render() method on the GraphRenderer will call the same method on both the VertexRenderer and EdgeRenderer.
	In this case, GraphRenderer is the composite and VertexRenderer and EdgeRenderer are leaves.
	
	
- Command
	CommandHandler.cs, class CommandHandler
	Command.cs, class Command
	Used for undo system in the editor to keep track of actions in the editor and their opposite/undo action.
	Some example of actions are:
		- Moving selected vertices (VertexPositionCommand class)
		- Deleting vertices (DeleteVertexCommand class))
		- Creating vertices (CreateVertexCommand class)
	
- State
	MouseController.cs, class MouseController
	Used a finite-state machine to set the three different states that mouse/user can have:
		- Disabled: No vertex is selected, handle axis isn't rendered and their "collisions" with mouse cursor are ignored.
		- Idling: Vertex is selected, handle axis are rendered and "collisions" with mouse cursor are enabled.
		- Grabbing: Will move vertex depending on handle axis selected on previous state (Idling).
	
- Observer 
	InputHandler.cs, class InputHandler
	Command.cs, class Control
	Each Control/Input has an event. The action designated for this Control is subscribed to this event that will be invoked on key down, up or hold (depending on behaviour chosen for Control)
	I used Unity Events for this to make it easy to change and customize each Control on the inspector.
	
- Component
	EditorManager object using components EditorController, Editor Renderer, MouseController, etc
	Vertex.cs, class Vertex using components Selectable and Renderer that inherit from ISelectable and IRenderer, respectively
	Used this pattern to decouple and define more specific behaviour. 
	On the Vertex class for example, ISelectable handles only selection behaviour and IRenderer handles only rendering behaviour like their names imply.