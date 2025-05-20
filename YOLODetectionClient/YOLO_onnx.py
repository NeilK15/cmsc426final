from ultralytics import YOLO
import torch

# Smaller faster model
model = YOLO("yolo11n-handpose.pt")

# Export to onnx format
model.export(format="onnx", dynamic=True)