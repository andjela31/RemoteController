# RemoteController2
A client-server system developed in Unity, featuring real-time communication between client and server using socket programming for data exchange.

## Features
- Real-time client-server communication using socket programming
- Android client built in Unity for remote control functionality
- Windows server application built in Unity for handling incoming connections
- Sending and receiving control commands between devices
- Low-latency message exchange over TCP connection
- Support for multiple UI actions triggered remotely
- Stable connection handling with basic connection status feedback

## Technologies Used
- Unity Engine (Client & Server applications)
- C# (scripting and networking logic)
- TCP Sockets (network communication)
- Android Build Support (Unity Client)
- Windows Standalone Build (Unity Server)
- .NET networking libraries

## How It Works
- The system consists of two separate Unity applications: a client (Android) and a server (Windows PC).
- The server application is started first and listens for incoming TCP socket connections on a specific IP address and port.
- The client application runs on an Android device and connects to the server using its IP address and port number.
- Once the connection is established, a persistent socket connection is maintained between client and server.
- The system uses synchronous socket programming, meaning that sending and receiving operations are blocking and executed in a sequential manner.
- The client sends control commands (messages) through the socket connection in real time.
- The server receives the messages, parses them, and executes the corresponding actions inside the Unity application.
- The server can also send responses back to the client to confirm received commands or update the connection status.
- Communication continues bi-directionally until the client disconnects or the server stops listening.

## How To Run The Application
**1. Clone the repository to your local machine:**
```bash
git clone <repository-url>
```

**2. Build or run the RemoteController_Server project in Unity (Windows PC):**
- Launch Unity Hub
- Click Open Project
- Select the RemoteController_Server folder
- Open the scene for the server
- Make sure the server is listening on the correct IP address and port
- You can run it directly in the Unity Editor by pressing Play, or
- Build the project as a Windows Standalone (.exe) via Build Settings and run the executable
  
**3. Build or run the RemoteController_Client project for Android:**
- Open Unity Hub
- Click Open Project
- Select the RemoteController_Client folder
- Switch platform to Android in Build Settings
- You can run it directly in the Unity Editor by pressing Play, or
- Build and install the APK on your Android device
  
**4. Ensure both devices are connected to the same network (Wi-Fi or LAN), or configure port forwarding if running on different networks.**
  
**5. Start the server first, then launch the client application.**

**6. In the client app, enter the server’s IP address and port number and establish the connection.**

**7. Once connected, you can start sending control commands from the Android client to the Windows server in real time.**

## Screenshots
### Initial State of the Application
This screenshot shows the user interface when the application is first launched, before any messages are sent.

- Empty chat window
- Input field ready for typing
- Initial connection state
<img width="1920" height="1019" alt="{24DDD7EF-0B7A-4A06-B3FE-7F2C256C3704}" src="https://github.com/user-attachments/assets/73df636c-3616-48ee-97fb-f22352746099" />

### Active Chat (Messages Sent)
This screenshot shows the application in use with exchanged messages between clients in real time.

- Displayed chat messages
- The sending instance shows the file being transmitted
- The receiving instance displays an alert indicating whether the file integrity is valid based on SHA-1 verification
- Real-time communication between users
- Demonstration of WebSocket functionality
<img width="1920" height="1018" alt="{61F5394A-E430-4FA3-A83E-FCA1929ADEA7}" src="https://github.com/user-attachments/assets/22acfc51-a771-4d49-880c-e70afbd63d48" />


## Project Information
- Developed: 2023  
- Improved: 2026  
- Type: Academic Project

## Author
- Andjela Djordjevic
