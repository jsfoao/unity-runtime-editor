## About
This repo contains a **runtime editor** made in **Unity**. The editor is mostly focused in building vehicle frames as seen in other game editors like Dream Car Builder or Main Assembly.

Short Demonstration: https://www.youtube.com/watch?v=-ulo9fZJNpE&feature=emb_imp_woyt
Download Build: https://jwow.itch.io/runtime-graph-editor

### Goals
 - **Easily expanded** for other applications (level editors, custom engine editor, etc).
 - Deepen my knowledge of **graphs**.
 - **Undo and Redo** functionality
 - Editor that feels natural and **easy to work with**.
 
 ![Demo](https://img.itch.zone/aW1hZ2UvMTMxMjc0MC83NjMzNDQ2LnBuZw==/original/O0ia2n.png)

## List of Features
| Feature| Description |
|--|--|
| Graph | Implementation of an **undirectioned and unweighed graph** |
| Undo and Redo | Undo and redo functionality with a **command pattern** |
| Mouse Interaction | Interaction with **mouse in world space** for selecting and moving vertices |
| Mouse State Machine | **Finite State machine** that defines the mouse's multiple states |
| Camera Controller | Control the **camera** around the editor's world space  |

## Keybindings
| Key| Description |
|--|--|
| W| Moves camera forward |
| A | Moves camera left|
| S | Moves camera backward|
| D | Moves camera right |
| Mouse Left | Vertex selection |
| Mouse Right | Camera rotation |
| Mouse Middle| Camera free pan|
| Mouse Scroll | Camera zoom |
| E | Extrude vertex |
| F | Add edge |
| X | Delete edge |
| Delete | Delete selected vertex |
| E | Extrude vertex |
| Left Ctrl | Toggle between multiple or single vertex selection |
| Left Ctrl + A | Select all vertices |
| Q | Undo |
| R | Add vertex on origin |

## Sources & Inspiration
Dream Car Builder: https://store.steampowered.com/app/488550/Dream_Car_Builder/
Main Assembly: https://store.steampowered.com/app/1078920/Main_Assembly/
