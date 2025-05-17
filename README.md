# cmsc426final

### Notes:
- The approach to get detections is to run a TCP server in `Blade.cs`.
- Then there is a python client script that runs detections and sends results to server.
- I do not understand how to create a Prefab for Blade.cs, and the script has to be added I do not know how to save it.
- Detection works and moves the `Blade`, but has no trail as detection is jittery, might require smoothing.
- Detection is kind of slow if you have GPU test and try with CUDA how fast it is.

### Running Blade
- Blade is spawned in game and the script runs creating a server

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

You should then see the position of the blade change on screen, in the logs for `YOLODetectionClient.py` and `Blade.cs` you should see the JSON that is being sent and received being printed.



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