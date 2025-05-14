import socket
import cv2
from ultralytics import YOLO
import json
import numpy as np

# Intialize local TCP socket 
host= "127.0.0.1"
port = 25001
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Import pretrained model
model = YOLO("yolo11s-pose.pt")

# Intialize webcam capture
cap = cv2.VideoCapture(0)
cap.set(3, 640)
cap.set(4, 480)

try:
    print("Initialized")
    # Connect to the server and send the data
    sock.connect((host, port))
    while True:
        # Capture web cam
        ret, img = cap.read()
        # Get detection results generator
        results = model(img, stream=True)
        for result in results:
            # For each result get the keypoints
            keypts = result.keypoints.numpy().data
            if keypts.shape[0] > 0:
                # If there is a detection

                # Get wrist keypoints
                leftwrist = keypts[0][9].tolist()
                rightwrist = keypts[0][10].tolist()

                # Dictionary with data
                detections = {'left': 
                            {
                                'detected': leftwrist[2]>0.5, 
                                'x':int(leftwrist[0]),
                                'y':int(leftwrist[1])
                            },
                            'right':{
                                'detected': rightwrist[2]>0.5, 
                                'x':int(rightwrist[0]),
                                'y':int(rightwrist[1])
                            }
                            }
                # Create JSON string and send on socket
                data = json.dumps(detections)
                print(data)
                sock.sendall(data.encode("utf-8"))

finally:
    cv2.destroyAllWindows()
    cap.release()
    sock.close()