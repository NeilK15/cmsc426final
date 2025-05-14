# cmsc426final

### Notes:
- I had issues running the project I just get a blue screen
- The approach to get detections is to run a TCP server in `Blade.cs`
- Then there is a python client script that runs detections and sends results to server
- The server should be started first and in theory when the Blade appears or when script starts running
- Then the client should be run

### Running Blade
- Should run when Blade is spawned in game
- Should be Debug logs that should say what is happening

### Running Detection Code
Create a virtual environment and install the requirements in seperate window:

```bash
cd YOLODetectionClient
python -m venv ./venv
source ./venv/bin/activate # for linux
./venv/Scripts/activate #for windows 
pip install -r requirements.txt
python YOLODetectionClient.py
```

- Eventually I plan to make this an executable with pyinstaller and put it into some assets folder in Unity so it persists when built
- Then in the main game logic using the Process Unity API can run the executable upon startup of the game

To see how it should work run:
```bash
cd YOLODetectionClient
python testserver.py
```
then in seperate terminal run:
```bash
cd YOLODetectionClient
python YOLODetectionClient.py
```