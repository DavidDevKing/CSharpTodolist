# CSharp Todolist

> A simple todolist app built with C#

## About

A simple todolist terminal application. Features todolists inside other todolists.

### Commands

| Command             | Function                                                              |
| :------------------ |:--------------------------------------------------------------------- |
| :h                  | Displays the help menu.                                               |
| :t [name]           | Creates a new task with the specified name.                           |
| :l [name]           | Creates a new list with the specified name.                           |
| :r [name]           | Renames the current list to the specified name.                       |
| :rt [id] [name]     | Renames the task or list with the specified id to the specified name. |
| :ct [id]            | Completes the task or list with the specified id.                     |
| :ut [id]            | Uncompletes the task or list with the selected id.                    |
| :dt [id]            | Deletes the task or list with the specified id.                       |
| :dd                 | Deletes the current list.                                             |
| cd [directory]      | Changes directory of lists based on their id.                         |
| :q                  | Quits the application                                                 |

## Getting Started

1. Clone into the repository

    ``` bash
    git clone https://github.com/DavidDevKing/CSharpTodolist
    ```

2. Run the application:

    ```bash
    dotnet run
    ```

> **Note:** All data lives in memory and is wiped when the application is closed.
