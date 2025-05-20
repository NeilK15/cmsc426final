import socket
import cv2
from ultralytics import YOLO
import json
import numpy as np
import time
import torch
# Intialize local TCP socket 
host= "127.0.0.1"
port = 25001
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Import pretrained model

# Smaller faster model
model = YOLO("yolo11s-pose.onnx")

window = "detection"

try:
    print("Initialized")
    # Try connecting to the server intiated by Blade.cs
    try: 
        sock.connect((host, port))  
    except OSError as msg:
        sock = None
        print(msg)
    while True:
        try:
            # If no connection try reconnecting and skip detection
            if sock is None:
                try:
                    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                    sock.connect((host, port))
                except OSError as msg:
                    sock=None
                    print(msg)
                    time.sleep(10)
                    continue
            
            # 0 is input for webcan slightly faster than cv2 cap
            if torch.cuda.is_available():
                results = model(0, device="cuda:0", stream=True)
            else:
                results = model(0, stream=True)
            for result in results:
                

                # For each result get the keypoints
                norm_keypts = result.keypoints.numpy().xyn
                
                keypts = result.keypoints.numpy().data
                if keypts.shape[0] > 0 and keypts.shape[1] > 0:
                    # If there is a detection

                    # Get wrist normalized_keypoints
                    leftwrist = norm_keypts[0][9].tolist()
                    rightwrist = norm_keypts[0][10].tolist()

                    # Get visibility of the keypoints
                    leftwrist_visibility = float(keypts[0][9][2])
                    rightwrist_visibility = float(keypts[0][10][2])

                    # Dictionary with data
                    detections = {'left': 
                                {
                                    'detected': leftwrist_visibility > 0.5, 
                                    'x':leftwrist[0],
                                    'y':leftwrist[1]
                                },
                                'right':{
                                    'detected': rightwrist_visibility > 0.5, 
                                    'x':rightwrist[0],
                                    'y':rightwrist[1]
                                }
                                }
                    # Create JSON string and send on socket
                    data = json.dumps(detections)
                    print(data)
                    sock.sendall(data.encode("utf-8"))

                # Visualization:
                plot_img = result.plot()
                cv2.imshow(window, plot_img)
                cv2.waitKey(1)
           
        except (ConnectionRefusedError, BrokenPipeError):
            # Connection error
            print("Reconnecting...")
            try:
                sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                sock.connect((host, port))
            except OSError as msg:
                sock = None
                print(msg)
                time.sleep(10)

finally:
    cv2.destroyAllWindows()
    sock.close()