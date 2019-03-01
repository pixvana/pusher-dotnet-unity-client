# Pusher Unity Client library

This is a C# library that can be used within Unity for interacting with the Pusher API.

It is largely based on the official pusher-dot-net-client library which can be found here <https://github.com/pusher/pusher-dotnet-client>

More general documentation can be found at <http://pusher.com/docs/>.

## Differences from pusher-dotnet-client

- Uses WebSocketSharp <https://github.com/sta/websocket-sharp> library
- Uses MiniJSON for JSON parsing <https://gist.github.com/darktable/1411710>
- Removed nuget support, and C# solution project files

## Installation

Import PusherClient.unitypackage file from top level of this repo.

As referenced above, the project requires MiniJSON and websocket-sharp. If you do not have
these in your project already then copy contents of UnityClient/Assets/Plugins into your
project's Plugins folder (should include MiniJSON directory, websocket-sharp.dll)

Example usage below, for a more complete example see [UnityClient/Assets/Scripts/App.cs](https://github.com/MiketoString/pusher-dotnet-unity-client/blob/master/UnityClient/Assets/Scripts/App.cs)

```
// app key from pusher settings
PusherSettings.AppKey = "";

// url to authorization server callback, if connecting to private channels
PusherSettings.HttpAuthUrl = "";

PusherClient.Pusher pusherClient = new PusherClient.Pusher( PusherSettings.AppKey, opts );
pusherClient.Connected += HandleConnected;
pusherClient.Connect();
```

## License

This code is free to use under the terms of the MIT license.
