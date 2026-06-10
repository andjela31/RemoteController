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
### Initial UI (Client + Server)
This screenshot shows the user interface when the application is first launched, before any messages are sent.

- Empty chat window
- Input field ready for typing
- Initial connection state
- Client:
<img width="1080" height="2400" alt="WhatsApp Image 2026-06-10 at 23 22 05" src="https://github.com/user-attachments/assets/e0ceac78-371a-4010-aa65-60283c4adb5b" />
- Server:
<img width="1920" height="1020" alt="{B4A76C7A-BF54-475D-BAFB-D16D5F7D0953}" src="https://github.com/user-attachments/assets/00b3ad7a-2d28-43f8-ad3e-e272d1c02e97" />


### Connection Process
This screenshot shows the application menu that alows client to connect to server.

- Ip address input
- Port input
- Connect button
- Connection status indicator
<img width="1080" height="2400" alt="WhatsApp Image 2026-06-10 at 23 32 25" src="https://github.com/user-attachments/assets/be11249f-f7da-4cf1-8112-3035837b5f7f" />

### Message Exchange
This screenshot shows the messages on both client and server side of the application.

- Client:
<img width="720" height="1600" alt="WhatsApp Image 2026-06-10 at 23 32 29" src="https://github.com/user-attachments/assets/4b1a3e51-a452-4796-9b79-151ca7bdcd09" />
- Server:
<img width="1920" height="1019" alt="{3AF973B8-3D10-46C8-A2C3-216386D8DAF2}" src="https://github.com/user-attachments/assets/134fc92b-7ce7-44e8-a6bb-f54a0512a15a" />

### Log Files
This screenshot shows the log files that are saved on server side.
- last.txt (last sent message):
<img width="674" height="251" alt="{A0FDD2B9-ECEF-4301-B701-758F0BE60C47}" src="https://github.com/user-attachments/assets/0c05063c-d81b-4b0e-ab54-2e4c58a80bf9" />
- log.txt (whole communication):
<img width="675" height="257" alt="{7BA45B1E-30EF-4177-A1BF-9859D99CD430}" src="https://github.com/user-attachments/assets/67645cb6-75ed-4ef7-8a32-9c36a818bf4d" />

## Project Information
- Developed: 2023  
- Improved: 2026  
- Type: Academic Project

## Author
- Andjela Djordjevic
