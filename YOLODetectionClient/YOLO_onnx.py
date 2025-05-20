from ultralytics import YOLO
import torch

# Smaller faster model
model = YOLO("yolo11s-pose.pt")

# Export to onnx format
model.export(format="onnx", dynamic=True)