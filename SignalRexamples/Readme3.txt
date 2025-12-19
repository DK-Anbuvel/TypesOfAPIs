


SignalR :-

   SignalR is a real-time communication library for ASP.NET and ASP.NET Core that makes it easy to add bi-directional, 
   real-time features to your apps (like chat, live dashboards, notifications, multiplayer games, etc.).


SignalR Transport Type :-

1) Web Sockets
    WebSockets are a protocol that provides a persistent, full-duplex communication channel between a client (browser/app) and a server.
    Unlike HTTP, which is request → response, WebSockets allow server ↔ client to send messages at any time without reconnecting.
    Chat apps
    Live dashboards
    Multiplayer games
    Live notifications
    Stock tickers
    Collaborative apps (Google Docs style)

2) SSE (Server Send Events)
   SSE is a one-way real-time communication protocol where:
   The server continuously pushes events to the client.
   The client cannot send messages back (unlike WebSockets).
   Uses standard HTTP (Content-Type: text/event-stream).
   Supported natively in most browsers.
   Perfect for streaming data like:
   Live notifications
   Stock/crypto price updates
   Log/console streaming
   Progress updates
   AI text streaming

3) Long Polling
  Long Polling is a technique for getting near–real-time updates over regular HTTP when WebSockets or SSE are unavailable.
  How it works:
  Client sends a request to the server.
  The server does not respond immediately.
  The server waits until:
  New data is available, or
  A timeout expires.
  Server returns the response.
  Client immediately sends another request.


How SignalR works :-

Server Side:-  
-------------
 Microsoft.AspNetCore.SignalR
Hub – A central class where you send/receive messages.
Connection – Persistent link (usually WebSocket).

create SignalR hub
Add methods to Hub
register in program.cs
Add client side signalR
connect to signalR hub from client js
call the signalR hub method
signalR hub invokes method in client js to notify clients.
client recives update from signalR hub and performs action.

Client Side:-
-------------

Client – Browser, mobile, desktop app using a SignalR client library.


Azsure SignalR :-

  package;-
  ------
  Microsoft.Azure.SignalR

