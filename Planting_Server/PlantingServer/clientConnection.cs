using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using scMessage;

namespace PlantingServer
{
    class clientConnection
    {
        public serverTCP svr;
        public Socket cSock;
        private int MAX_INC_DATA = 512000; // half a megabyte

        public static ArrayList id = new ArrayList();
        public string sendip = "";

        public clientConnection(Socket s, serverTCP sv)
        {
            svr = sv;
            cSock = s;
            ThreadPool.QueueUserWorkItem(new WaitCallback(handleConnection));
            sendipaddress();
        }

        public void handleConnection(object x)
        {
            // broadcast new connection
            output.outToScreen("A client connected from the IP address: " + cSock.RemoteEndPoint.ToString());
            serverTCP.clientSockets.Add(cSock);
            id.Add(cSock.RemoteEndPoint.ToString()); //ArrayList에 IP주소 저장
                                                     //svr.sendClientMessage(cSock, new message("testMessage"));
                                                     //TODO: Remove the test message
            try
            {
                while (cSock.Connected)
                {
                    byte[] sizeInfo = new byte[4];

                    int bytesRead = 0,
                        currentRead = 0;

                    currentRead = bytesRead = cSock.Receive(sizeInfo);
                    //output.outToScreen("" + cSock.Receive(sizeInfo));
                    while (bytesRead < sizeInfo.Length && currentRead > 0)
                    {
                        currentRead =
                            cSock.Receive
                            (
                                sizeInfo, // message frame, size of incoming message
                                bytesRead, // offset into the buffer
                                sizeInfo.Length - bytesRead, // max amount to read
                                SocketFlags.None // no socket flags
                            );

                        bytesRead += currentRead;
                    }

                    // get the message size of incoming message
                    int messageSize = BitConverter.ToInt32(sizeInfo, 0);

                    // create a byte array with the correct message size
                    byte[] incMessage = new byte[messageSize];

                    // begin reading message
                    bytesRead = 0; // reset to ensure proper byte read count

                    currentRead =
                        bytesRead =
                        cSock.Receive
                        (
                            incMessage, // incoming message
                            bytesRead,
                            incMessage.Length - bytesRead,
                            SocketFlags.None
                        );

                    // check to see if we received all data
                    while (bytesRead < messageSize && currentRead > 0)
                    {
                        currentRead =
                            cSock.Receive
                            (
                                incMessage, // incoming message
                                bytesRead,
                                incMessage.Length - bytesRead,
                                SocketFlags.None
                            );
                        bytesRead += currentRead;
                    }

                    // all data received, continue
                    try
                    {
                        message incObject = (message)conversionTools.convertBytesToObject(incMessage);
                        if (incObject != null)
                        {
                            svr.handleClientData(cSock, incObject);
                        }
                    }

                    catch
                    {

                    }
                }
            }

            catch
            {

            }
            output.outToScreen("The client disconnected from IP address: " + cSock.RemoteEndPoint.ToString());
            id.Remove(cSock.RemoteEndPoint.ToString());
            serverTCP.clientSockets.Remove(cSock);
            cSock.Close();

            if (serverTCP.clientinfos.ContainsKey(cSock) || serverTCP.clientinfos.ContainsValue(cSock))
            {
                serverTCP.clientinfos.Remove(cSock);
            }
        }



        public void sendipaddress()
        {
            sendip = cSock.RemoteEndPoint.ToString();
            message newMessage1000 = new message("SendipaddressResponse");
            scObject data = new scObject("data");
            data.addBool("response", true);
            data.addString("useripaddress", sendip);
            newMessage1000.addSCObject(data);
            serverTCP power = new serverTCP();
            power.sendClientMessage(cSock, newMessage1000);
        }

        public static void checkchcek()
        {
            output.outToScreen("현재 접속 아이피 " + id.Count + "개");
            for (int i = 0; i < id.Count; i++)
            {
                Console.WriteLine("{0}", id[i]);
            }
        }
    }
}