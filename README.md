# Quick Start

- In order to **create** a new class for the current Day, in VS Code, just select "Terminal" > "Run Task..." > "Add new day".
Now you can implement your code for the day! You can find your new class in `.\Days\` folder.

- Your input text file should be in the `.\input\` folder, named "{current day number}.txt"

- In order to **run** the current Day, in VS Code, just hit F5.

# Start asychronously

- In order to **create** an new class for a previous Day (e.g. day 2, when we are in day 25), in VS Code,
open `.\vscode\tasks.json`, in line 45, provide the day number in the "args" field as a string. (e.g. `"args": ["2"],`).
Then, run the task "Add new day". Now you can implement your code for the day! You can find your new class in `.\Days\` folder.

- Your input text file should be in the `.\input\` folder, named "{day number}.txt" (e.g. `2.txt`)

- In order to **run** a previous day's code, in VS Code,
open `.\vscode\launch.json`, in line 14, provide the day number in the "args" field as a string. (e.g. `"args": ["2"],`).

## Enjoy your Advent of Code!