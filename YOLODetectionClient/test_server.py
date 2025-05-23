# Echo server program
import socket

host, port = "127.0.0.1", 25001
with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((host, port))
    s.listen(1)
    conn, addr = s.accept()
    with conn:
        print('Connected by', addr)
        while True:
            
            data = conn.recv(1024)
            if data:
                print(data.decode("utf-8"))