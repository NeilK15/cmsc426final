# cmsc426final

### Notes:
- Trained new detection model `yolo11n-handpose.onnx` on hand keypoints.
- This works better than the full body pose detection from before as that is better for when whole body is in frame.
- Also converted models to `.onnx` format as it optimizes computation.
- Run this new client with `hand_detection_client.py`.
- Seperated the server logic to the `DetectionServer.cs` script from `Blade.cs`.
- I attempted to add a webcam with `WebCam.cs` however the camera can not be accessed by
two resources at the same time, so the detection script fails.
- Would have to add a server that feeds video stream to both.

### Running Blade
- Blade is spawned in game and pulls `DetectionData` from `DetectionServer.cs` script.

### Running Detection Code
Create a virtual environment and install the requirements in separate window:
```bash
cd YOLODetectionClient
python -m venv ./venv
source ./venv/bin/activate # for linux
./venv/Scripts/activate #for windows 
pip install -r requirements.txt
```

#### Run New Hand Detection Client
```bash
python hand_detection_client.py
```

#### Run Old Full Body Detection Client
```bash
python YOLODetectionClient.py
```

- Should see detection data being printed in Unity console and python console.
- Can uncomment visualization block in python client to see prediction.