using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Networking.Common;
using Networking.Packets;
using Networking.Threading;
using UnityEngine;

namespace Networking.GameServer
{
    public class GameClient : MonoBehaviour
    {
        public static GameClient Instance;
        public const int DataBufferSize = 4096;

        public TcpConnection TcpConn;
        public UdpConnection UdpConn;

        private bool _isConnected;
        private delegate void PacketHandler(Packet packet);
        private static Dictionary<int, PacketHandler> _packetHandlers;

        public int _myId = 0;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(this);
        }

        private void Start()
        {
            ConnectToServer();
        }

        private void OnApplicationQuit()
        {
            Disconnect(); // Disconnect when the game is closed
        }

        /// <summary>Attempts to connect to the server.</summary>
        public void ConnectToServer()
        {
            TcpConn = new TcpConnection();
            UdpConn = new UdpConnection();
            InitializeClientData();

            _isConnected = true;
            TcpConn.Connect(); // Connect tcp, udp gets connected once tcp is done
        }

        /// <summary>Initializes all necessary client data.</summary>
        private void InitializeClientData()
        {
            _packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)GameServerToClient.Welcome, GameClientHandle.Welcome },
            };
            Debug.Log("Initialized packets.");
        }

        /// <summary>Disconnects from the server and stops all network traffic.</summary>
        private void Disconnect()
        {
            if (!_isConnected) return;
            _isConnected = false;
            TcpConn.TcpSocket.Close();

            Debug.Log("Disconnected from server.");
        }
        
        public class TcpConnection
        {
            public TcpClient TcpSocket;
            private NetworkStream _stream;
            private Packet _receivedData;
            private byte[] _receiveBuffer;

            /// <summary>Attempts to connect to the server via TCP.</summary>
            public void Connect()
            {
                TcpSocket = new TcpClient
                {
                    ReceiveBufferSize = DataBufferSize,
                    SendBufferSize = DataBufferSize
                };

                _receiveBuffer = new byte[DataBufferSize];
                TcpSocket.BeginConnect(Global.GameServerIp, Global.GameServerPort, ConnectCallback, TcpSocket);
            }

            /// <summary>Initializes the newly connected client's TCP-related info.</summary>
            private void ConnectCallback(IAsyncResult result)
            {
                TcpSocket.EndConnect(result);

                if (!TcpSocket.Connected)
                    return;

                _stream = TcpSocket.GetStream();
                _receivedData = new Packet();
                _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if (TcpSocket != null)
                        _stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null); // Send data to server
                }
                catch (Exception ex)
                {
                    Debug.Log($"Error sending data to server via TCP: {ex}");
                }
            }

            /// <summary>Reads incoming data from the stream.</summary>
            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    var byteLength = _stream.EndRead(result);
                    if (byteLength <= 0)
                    {
                        Instance.Disconnect();
                        return;
                    }

                    var data = new byte[byteLength];
                    Array.Copy(_receiveBuffer, data, byteLength);

                    _receivedData.Reset(HandleData(data)); // Reset receivedData if all data was handled
                    _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
                }
                catch
                {
                    Disconnect();
                }
            }

            /// <summary>Prepares received data to be used by the appropriate packet handler methods.</summary>
            /// <param name="data">The received data.</param>
            private bool HandleData(byte[] data)
            {
                var packetLength = 0;

                _receivedData.SetBytes(data);

                if (_receivedData.UnreadLength() >= 4)
                {
                    // If client's received data contains a packet
                    packetLength = _receivedData.ReadInt();
                    if (packetLength <= 0)
                    {
                        // If packet contains no data
                        return true; // Reset receivedData instance to allow it to be reused
                    }
                }

                while (packetLength > 0 && packetLength <= _receivedData.UnreadLength())
                {
                    // While packet contains data AND packet data length doesn't exceed the length of the packet we're reading
                    var packetBytes = _receivedData.ReadBytes(packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using var packet = new Packet(packetBytes);
                        var packetId = packet.ReadInt();
                        _packetHandlers[packetId](packet); // Call appropriate method to handle the packet
                    });

                    packetLength = 0; // Reset packet length
                    if (_receivedData.UnreadLength() < 4) continue;
                    // If client's received data contains another packet
                    packetLength = _receivedData.ReadInt();
                    if (packetLength <= 0)
                    {
                        // If packet contains no data
                        return true; // Reset receivedData instance to allow it to be reused
                    }
                }

                if (packetLength <= 1)
                    return true; // Reset receivedData instance to allow it to be reused

                return false;
            }

            /// <summary>Disconnects from the server and cleans up the TCP connection.</summary>
            private void Disconnect()
            {
                Instance.Disconnect();

                _stream = null;
                _receivedData = null;
                _receiveBuffer = null;
                TcpSocket = null;
            }
        }
        
        public class UdpConnection
        {
            public UdpClient UdpSocket;
            public IPEndPoint EndPoint;

            public UdpConnection()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse(Global.GameServerIp), Global.GameServerPort);
            }

            /// <summary>Attempts to connect to the server via UDP.</summary>
            /// <param name="localPort">The port number to bind the UDP socket to.</param>
            public void Connect(int localPort)
            {
                UdpSocket = new UdpClient(localPort);

                UdpSocket.Connect(EndPoint);
                UdpSocket.BeginReceive(ReceiveCallback, null);

                using var packet = new Packet();
                SendData(packet);
            }

            /// <summary>Sends data to the client via UDP.</summary>
            /// <param name="packet">The packet to send.</param>
            public void SendData(Packet packet)
            {
                try
                {
                    packet.InsertInt(Instance._myId); // Insert the client's ID at the start of the packet
                    if (UdpSocket != null)
                        UdpSocket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                }
                catch (Exception ex)
                {
                    Debug.Log($"Error sending data to server via UDP: {ex}");
                }
            }

            /// <summary>Receives incoming UDP data.</summary>
            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    var data = UdpSocket.EndReceive(result, ref EndPoint);
                    UdpSocket.BeginReceive(ReceiveCallback, null);

                    if (data.Length < 4)
                    {
                        Instance.Disconnect();
                        return;
                    }
                    HandleData(data);
                }
                catch
                {
                    Disconnect();
                }
            }

            /// <summary>Prepares received data to be used by the appropriate packet handler methods.</summary>
            /// <param name="data">The recieved data.</param>
            private void HandleData(byte[] data)
            {
                using (var packet = new Packet(data))
                {
                    var packetLength = packet.ReadInt();
                    data = packet.ReadBytes(packetLength);
                }

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using var packet = new Packet(data);
                    var packetId = packet.ReadInt();
                    _packetHandlers[packetId](packet); // Call appropriate method to handle the packet
                });
            }

            /// <summary>Disconnects from the server and cleans up the UDP connection.</summary>
            private void Disconnect()
            {
                Instance.Disconnect();

                EndPoint = null;
                UdpSocket = null;
            }
        }
    }
}