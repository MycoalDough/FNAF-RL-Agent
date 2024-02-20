import socket
import time
import json
import numpy as np
import threading
from typing import Optional
# Define host and port for the server
host = "127.0.0.1"  # Use your local or external IP address
port = 12345
current_data = []
server_socket: Optional[socket.socket] = None
client_socket: Optional[socket.socket] = None

# Create a socket and bind it to the host and port
def create_host(callback):
    global server_socket, client_socket
    if server_socket:
        server_socket.close()
        
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind(("localhost", 12345))  # Change to your host and port
    server_socket.listen()  # Allow only one connection

    print("Listening for Unity connection")

    while True:
        # Accept a connection from Unity
        client_socket, client_address = server_socket.accept()
        print(f"Accepted connection from {client_address}")

        # Call the callback function to signal the connection is established
        callback(client_socket)



def get_state():
    try:
        client_socket.send("get_state".encode("utf-8"))
        data = client_socket.recv(1024).decode("utf-8")
        data_list = data.split(',')
        current_data = []
        if data:
            for element in data_list:
                try:
                    # Attempt to convert the element to a float directly.
                    float_element = float(element)
                    current_data.append(float_element)
                except ValueError:
                    # Handle non-numeric strings here, if any.
                    if element.lower() == 'true':
                        current_data.append(1.0)  # or True, if you want to keep boolean values
                    elif element.lower() == 'false':
                        current_data.append(0.0)  # or False, as above
                    # Optionally, handle other non-numeric cases if necessary.
        return np.array(current_data, dtype=float)
    except Exception as e:
        print("Error in get_state:", e)
        return None




def play_step(step):
    try:
        to_send = "play_step:" + str(step)
        client_socket.send(to_send.encode("utf-8"))
        data = client_socket.recv(1024).decode("utf-8")
        if data:
            elements = data.split(':')

            result_list = []

            for element in elements:
                if is_float(element):
                    result_list.append(float(element))
                elif element.replace('.', '', 1).isdigit():
                    result_list.append(float(element))
                elif element.lower() == 'true' or element.lower() == 'false':
                    result_list.append(element.lower() == 'true')
                else:
                    result_list.append(element)
            
            #print(result_list[0], result_list[1], result_list[2]);
            return result_list[0], result_list[1], result_list[2], result_list[3]
    except Exception as e:
        print("Error in get_state:", e)
        return None

def is_float(s):
    try:
        float_value = float(s)
        return True
    except ValueError:
        return False

def reset():
    client_socket.send("reset".encode("utf-8"))
    # Now wait for the reset acknowledgment
    while True:
        data = client_socket.recv(1024).decode("utf-8")
        if data == "reset_complete":
            break
