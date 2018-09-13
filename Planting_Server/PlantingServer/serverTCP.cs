using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using scMessage;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace PlantingServer
{
    class serverTCP
    {
        private static int
            clientPort = 3000, policyFilePort = 2999;

        private static Socket
            policyFileListenSocket, clientListenSocket;

        private static List<clientConnection>
            clients = new List<clientConnection>();

        public static
            List<Socket> clientSockets = new List<Socket>();

        public static 
            ArrayList check = new ArrayList();
         
        public static
            Dictionary<Socket, Socket> clientinfos = new Dictionary<Socket, Socket>();

        public Socket value_Key;
        public Socket value_Value;

        public string plantname;
        public serverTCP()
        {
            try
            {
                // listen for policy requests
                policyFileListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                policyFileListenSocket.Bind(new IPEndPoint(IPAddress.Any, policyFilePort));
                policyFileListenSocket.Listen(int.MaxValue);
                ThreadPool.QueueUserWorkItem(new WaitCallback(listenForPFR));

                // listen for clients
                clientListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientListenSocket.Bind(new IPEndPoint(IPAddress.Any, clientPort));
                clientListenSocket.Listen(int.MaxValue);
                ThreadPool.QueueUserWorkItem(new WaitCallback(listenForClients));

                output.outToScreen("Waiting for client policy file requests on port " + policyFilePort + " and clients on port " + clientPort);
            }
            catch { }
        }

        private void listenForPFR(object x)
        {
            while (serverMain.keepAlive)
            {
                Socket pfRequest = policyFileListenSocket.Accept();
                policyFileConnection newRequest = new policyFileConnection(pfRequest);
            }
        }

        private void listenForClients(object x)
        {
            while (serverMain.keepAlive)
            {
                Socket cSocket = clientListenSocket.Accept();
                clientConnection newCon = new clientConnection(cSocket, this);
            }
        }

        public void handleClientData(Socket cSock, message incObject)
        {
            Stopwatch Sw = new Stopwatch();
            switch(incObject.messageText)
            {
                case "home":
                    message home = new message("homeResponse");
                    cSock.Shutdown(SocketShutdown.Receive);
                    if(true)
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        home.addSCObject(data);
                        sendClientMessage(cSock, home);
                    }
                    break;

                case "close":
                    message close = new message("closeResponse");
                    cSock.Shutdown(SocketShutdown.Receive);
                    if (true)
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        close.addSCObject(data);
                        sendClientMessage(cSock, close);
                    }
                    break;

                case "login":
                    //output.outToScreen("serverTCP - login 정상 실행.");
                    message login = new message("loginResponse");
                    if (playerTools.checkLogin(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getString("password")))
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        login.addSCObject(data);
                        output.outToScreen("loginScript - loginResponse에 True 값 전달.");
                    }

                    else
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        login.addSCObject(data);
                        output.outToScreen("loginScript - loginResponse에 False 값 전달.");
                    }
                    sendClientMessage(cSock, login);
                    break;

                case "register":
                    //output.outToScreen("serverTCP - register 정상 실행.");
                    message register = new message("registerResponse");
                    if (playerTools.createregister(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getString("password")))
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        register.addSCObject(data);
                        output.outToScreen("loginScript - registerResponse에 True 값 전달.");

                    }
                    else
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        register.addSCObject(data);
                        output.outToScreen("loginScript - registerResponse에 False 값 전달.");
                    }
                    sendClientMessage(cSock, register);
                    break;

                case "url":
                    //output.outToScreen("serverTCP - url 정상 실행.");
                    message url = new message("urlResponse");
                    if (playerTools.createurl(incObject.getSCObject(0).getString("url"), incObject.getSCObject(0).getString("account")))
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        url.addSCObject(data);
                        output.outToScreen("loginScript - urlResponse에 True 값 전달.");
                    }
                    else
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        url.addSCObject(data);
                        output.outToScreen("loginScript - urlResponse에 False 값 전달.");
                    }
                    sendClientMessage(cSock, url);
                    break;
                
                case "getItem":
                    //output.outToScreen("serverTCP - getItem 정상 실행.");
                    message getitem = new message("getItemResponse");
                    if(playerTools.getItem(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getString("itemName")))
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        getitem.addSCObject(data);
                        output.outToScreen("loginScript - getItemResponse에 True 값 전달.");
                    }
                    else
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        getitem.addSCObject(data);
                        output.outToScreen("loginScript - getItemResponse에 False 값 전달.");
                    }
                    sendClientMessage(cSock, getitem);
                    break;

                case "UpdatePlantListTable":
                    //output.outToScreen("serverTCP - UpdatePlantListTable 정상 실행.");
                    message updatePlantListTableMes = new message("UpdatePlantListTableResponse");
                    if (playerTools.UpdatePlantListTable(incObject.getSCObject(0).getString("account"),
                        incObject.getSCObject(0).getString("plantName"),
                        incObject.getSCObject(0).getInt("plantID"),
                        incObject.getSCObject(0).getString("itemName"), incObject.getSCObject(0).getInt("posNumber"),
                        incObject.getSCObject(0).getInt("level"), incObject.getSCObject(0).getFloat("expAmount"),
                        incObject.getSCObject(0).getBool("isSeedItem")))
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        updatePlantListTableMes.addSCObject(data);
                        output.outToScreen("loginScript - UpdatePlantListTable에 True 값 전달.");
                    }
                    else
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        updatePlantListTableMes.addSCObject(data);
                        output.outToScreen("loginScript - UpdatePlantListTable에 False 값 전달.");
                    }
                    sendClientMessage(cSock, updatePlantListTableMes);
                    break;

                case "SelectQuery":
                    //output.outToScreen("serverTCP - SelectQuery 정상 실행.");
                    message selectQueryMes = new message("SelectQueryResponse");
                    if (playerTools.SelectQuery(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getString("columnName"),
                        incObject.getSCObject(0).getString("tableName")))
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        data.addInt("plantListTableCount", playerTools.plantPos.Count);
                        for (int i = 0; i <= playerTools.plantPos.Count - 1; i++)
                        {
                            data.addInt("plantPos[" + i + "]", playerTools.plantPos[i]);
                            data.addString("plantName[" + i + "]", playerTools.plantName[i]);
                            data.addInt("plantID[" + i + "]", playerTools.plantID[i]);
                            data.addInt("Lv[" + i + "]", playerTools.Lv[i]);
                            data.addFloat("waterEXP[" + i + "]", playerTools.waterEXP[i]);
                            data.addFloat("sunEXP[" + i + "]", playerTools.sunEXP[i]);
                            data.addFloat("fertilizerEXP[" + i + "]", playerTools.fertilizerEXP[i]);
                        }
                        ////고민중, 걍 노가다임시방편으로 할지 여러 곳에 사용될수잇게 만들지.. 테이블마다 컬럼 개수가 다를 텐데 어떻게 다 가져오지?
                        //위에 써놓음, 다시말하면 2차원배열 만들면됨, 배열 크기는 loginScript에서 넘겨받으면됨,
                        //근데 보통 테이블마다 따로 짠다고하니 별 상관 없을듯함
                        selectQueryMes.addSCObject(data);
                        output.outToScreen("loginScript - SelectQueryResponse에 True 값 전달.");
                    }
                    else
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        selectQueryMes.addSCObject(data);
                        output.outToScreen("loginScript - SelectQueryResponse에 False 값 전달.");
                    }
                    playerTools.plantPos.Clear();
                    playerTools.plantName.Clear();
                    playerTools.plantID.Clear();
                    playerTools.Lv.Clear();
                    playerTools.waterEXP.Clear();
                    playerTools.sunEXP.Clear();
                    playerTools.fertilizerEXP.Clear();
                    sendClientMessage(cSock, selectQueryMes);
                    break;

                case "UpdatePlantExp":
                    //output.outToScreen("serverTCP - SelectQuery 정상 실행.");
                    message UpdatePlantExpMes = new message("UpdatePlantExpResponse");
                    if (playerTools.UpdatePlantExp(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getInt("plantPos"),
                        incObject.getSCObject(0).getInt("level"), incObject.getSCObject(0).getInt("expName"), incObject.getSCObject(0).getFloat("expAmount")))
                    {
                        output.outToScreen("SeverTCP - UpdatePlantExp is complete");
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        UpdatePlantExpMes.addSCObject(data);
                    }
                    else
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        UpdatePlantExpMes.addSCObject(data);
                    }
                    //sendClientMessage(cSock, UpdatePlantExpMes);
                    break;

                case "UpdatePlantID":
                    message UpdatePlantIDMes = new message("UpdatePlantIDResponse");
                    if (playerTools.UpdatePlantID(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getInt("plantPos"),
                        incObject.getSCObject(0).getInt("plantID")))
                    {
                        output.outToScreen("SeverTCP - UpdatePlantID is complete");
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        UpdatePlantIDMes.addSCObject(data);
                    }
                    else
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        UpdatePlantIDMes.addSCObject(data);
                    }
                    //sendClientMessage(cSock, UpdatePlantExpMes);
                    break;

                case "ItemCountCheck":
                    //output.outToScreen("serverTCP - ItemCountCheck 정상 실행.");
                    message itemcountcheck = new message("ItemCountCheckResponse");
                    if (playerTools.ItemCountCheck(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getString("itemName")))
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);

                        if(playerTools.number == playerTools.wItemNum)
                        {
                            data.addInt("wItemNum", playerTools.wItemNum);
                            itemcountcheck.addSCObject(data);
                            //output.outToScreen("loginScript - ItemCountCheckResponse에 True 값과 wItemNum 값 전달.");
                        }
                        if (playerTools.number2 == playerTools.fItemNum)
                        {
                            data.addInt("fItemNum", playerTools.fItemNum);
                            itemcountcheck.addSCObject(data);
                            //output.outToScreen("loginScript - ItemCountCheckResponse에 True 값과 fItemNum 값 전달.");
                        }
                        if (playerTools.number3 == playerTools.sItemNum)
                        {
                            data.addInt("sItemNum", playerTools.sItemNum);
                            itemcountcheck.addSCObject(data);
                            //output.outToScreen("loginScript - ItemCountCheckResponse에 True 값과 sItemNum 값 전달.");
                        }
                        if (playerTools.number4 == playerTools.nItemNum)
                        {
                            data.addInt("nItemNum", playerTools.nItemNum);
                            itemcountcheck.addSCObject(data);
                            //output.outToScreen("loginScript - ItemCountCheckResponse에 True 값과 nItemNum 값 전달.");
                        }
                        if (playerTools.number5 == playerTools.sfsItemNum)
                        {
                            data.addInt("sfsItemNum", playerTools.sfsItemNum);
                            itemcountcheck.addSCObject(data);
                            //output.outToScreen("loginScript - ItemCountCheckResponse에 True 값과 sfsItemNum 값 전달.");
                        }
                        if (playerTools.number6 == playerTools.csItemNum)
                        {
                            data.addInt("csItemNum", playerTools.csItemNum);
                            itemcountcheck.addSCObject(data);
                            //output.outToScreen("loginScript - ItemCountCheckResponse에 True 값과 csItemNum 값 전달.");
                        }
                        if (playerTools.number7 == playerTools.tsItemNum)
                        {
                            data.addInt("tsItemNum", playerTools.tsItemNum);
                            itemcountcheck.addSCObject(data);
                            //output.outToScreen("loginScript - ItemCountCheckResponse에 True 값과 tsItemNum 값 전달.");
                        }
                        output.outToScreen("loginScript - ItemCountCheckResponse에 True 값 전달");
                    }
                    else
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        itemcountcheck.addSCObject(data);
                        output.outToScreen("loginScript - ItemCountCheckResponse에 False 값 전달.");
                    }

                    sendClientMessage(cSock, itemcountcheck);
                    break;

                case "urlcheck":
                    //output.outToScreen("serverTCP - urlcheck 정상 실행.");
                    message urlcheck = new message("urlcheckResponse");
                    if (playerTools.urlcheck(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getString("password")))
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        urlcheck.addSCObject(data);
                        output.outToScreen("loginScript - urlcheckResponse에 True 값 전달.");
                    }
                    else
                    {

                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        urlcheck.addSCObject(data);
                        output.outToScreen("loginScript - urlcheckResponse에 False 값 전달.");
                    }
                    sendClientMessage(cSock, urlcheck);
                    break;

                case "UseItem":
                    //output.outToScreen("serverTCP - UseItem 정상 실행.");
                    message useitem = new message("UseItemResponse");
                    if (playerTools.UseItem(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getString("itemName"), incObject.getSCObject(0).getInt("itemNum")))
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        data.addInt("wItemNum", playerTools.number);
                        data.addInt("fItemNum", playerTools.number2);
                        data.addInt("sItemNum", playerTools.number3);
                        data.addInt("nItemNum", playerTools.number4);
                        data.addInt("sfsItemNum", playerTools.number5);
                        data.addInt("csItemNum", playerTools.number6);
                        data.addInt("tsItemNum", playerTools.number7);
                        useitem.addSCObject(data);
                        output.outToScreen("loginScript - UseItemResponse에 True 값 전달.");
                    }
                    else
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        useitem.addSCObject(data);
                        output.outToScreen("loginScript - UseItemResponse에 False 값 전달.");
                    }
                    sendClientMessage(cSock, useitem);
                    break;

                case "sendtime":
                    message sendtime = new message("sendtimeresponse");
                    if(playerTools.sendtime(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getFloat("time"), incObject.getSCObject(0).getString("scenename")))
                    {
                        output.outToScreen("");
                    }
                    break;

                case "plusExp":
                    //output.outToScreen("serverTCP - plusExp 정상 실행.");
                    message plusExp = new message("plusExpResponse");
                    if (playerTools.plusExp(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getString("password")))
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        plusExp.addSCObject(data);
                        output.outToScreen("loginScript - plusExpResponse에 True 값 전달.");
                    }
                    else
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        plusExp.addSCObject(data);
                        output.outToScreen("loginScript - plusExpResponse에 False 값 전달.");

                    }
                    sendClientMessage(cSock, plusExp);
                    break;

                case "CheckExp":
                    //output.outToScreen("serverTCP - CheckExp 정상 실행.");
                    message CheckExp = new message("CheckExpResponse");
                    if (playerTools.CheckExp(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getString("password")))
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        data.addInt("Exp", playerTools.Exp);
                        CheckExp.addSCObject(data);
                        output.outToScreen("loginScript - CheckExpResponse에 True 값 전달.");
                    }
                    else
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        CheckExp.addSCObject(data);
                        output.outToScreen("loginScript - CheckExpResponse에 False 값 전달.");
                    }
                    sendClientMessage(cSock, CheckExp);
                    break;

                case "getplantname": //이부분도 수정이 필요하다. 첨에 대전 버튼 누르면 되는데 아니면 안된다.
                    message GetPlantNameMessage = new message("GetPlantNameResponse");
                    if (true)
                    {
                        string name = "";
                        char sp = ',';
                        name = playerTools.GetPlantName(incObject.getSCObject(0).getString("account"));
                        string[] spstring = name.Split(sp);
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        data.addString("name1", spstring[0]);
                        data.addString("name2", spstring[1]);
                        data.addString("name3", spstring[2]);
                        data.addString("name4", spstring[3]);
                        GetPlantNameMessage.addSCObject(data);
                        sendClientMessage(cSock, GetPlantNameMessage);
                    }
                    /*else
                    {
                        output.outToScreen("식물 이름 받아오기 실패");
                        scObject data = new scObject("data");
                        data.addBool("response", false);
                        GetPlantNameMessage.addSCObject(data);
                        sendClientMessage(cSock, GetPlantNameMessage);
                    }*/
                    break;

                case "transferip":
                    message newMessage10 = new message("TransferIPResponse");
                    if (clientSockets.Contains(cSock))
                    {
                        output.outToScreen("중복 아이피 또는 Socket 입니다. " + incObject.getSCObject(0).getString("battleip") + cSock);
                    }
                    else
                    {
                        clientSockets.Add(cSock);
                    }

                    if (clientSockets.Count == 2)
                    {
                        clientinfos.Add(clientSockets[0], clientSockets[1]); //KEY VALUE
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        newMessage10.addSCObject(data);
                        sendClientMessage(clientSockets[0], newMessage10);
                        sendClientMessage(clientSockets[1], newMessage10);
                        clientSockets.Clear();
                    }
                    else
                    {
                        scObject data = new scObject("data");
                        output.outToScreen("매칭 인원이 부족합니다.");
                        data.addBool("response", false);
                        sendClientMessage(clientSockets[0], newMessage10);
                    }
                    break;

                case "addrank":
                    message addRank = new message("addRankResponse");
                    if (true)
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        data.addInt("rankpoint", playerTools.addrank(incObject.getSCObject(0).getString("account")));
                        addRank.addSCObject(data);
                    }
                    else
                    {

                    }
                    sendClientMessage(cSock, addRank);
                    break;

                case "subtractionrank":
                    message subtractionrank = new message("subtractionRankResponse");
                    if (true)
                    {
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        data.addInt("rankpoint", playerTools.subtractionrank(incObject.getSCObject(0).getString("account")));
                        subtractionrank.addSCObject(data);
                    }
                    else
                    {

                    }
                    sendClientMessage(cSock, subtractionrank);
                    break;

                case "sendplantname":
                    if (playerTools.sendplantname(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getString("plantname")))
                    {
                        output.outToScreen("sendplantName");//이제 로그인 스크립트에 저장된 이름 다시 쏴줘야해
                    }
                    break;

                case "alldeleteplantname":
                    if (playerTools.AllDeletePlantName(incObject.getSCObject(0).getString("account")))
                    {
                        output.outToScreen("이름 전부 초기화");
                    }
                    break;

                case "deleteplantname":
                    if (playerTools.DeletePlantName(incObject.getSCObject(0).getString("account"), incObject.getSCObject(0).getString("plantname")))
                    {
                        output.outToScreen("이름 1개 완료");
                    }
                    break;

                case "sendcardmessage": //왜 안되는지 모르겠음
                    message SendCreateMyCard = new message("MyCardResponse");
                    message SendCreateAICard = new message("SendCreateAICardResponse");
                    if (clientinfos.ContainsKey(cSock) || clientinfos.ContainsValue(cSock))
                    {
                        plantname = incObject.getSCObject(0).getString("plantname");
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        data.addString("plantname", plantname);
                        SendCreateMyCard.addSCObject(data);
                        SendCreateAICard.addSCObject(data);

                        if (clientinfos.ContainsKey(cSock))
                        {
                            clientinfos.TryGetValue(cSock, out value_Value);
                            sendClientMessage(value_Value, SendCreateAICard);
                            sendClientMessage(cSock, SendCreateMyCard);
                        }
                        else if (clientinfos.ContainsValue(cSock))
                        {
                            value_Key = clientinfos.FirstOrDefault(x => x.Value == cSock).Key;
                            sendClientMessage(value_Key, SendCreateAICard);
                            sendClientMessage(cSock, SendCreateMyCard);
                        }
                    }
                    else
                    {

                    }
                    break;

                case "senddestroyotherobject": //내 오브젝트 파괴 했다는 메세지 받았을때;
                    {
                        output.outToScreen("??");
                        message newMessage13 = new message("SendDestroyMyObjectResponse");
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        newMessage13.addSCObject(data);

                        message newMessage14 = new message("SendDestroyAiObjectResponse");
                        scObject data1 = new scObject("data1");
                        data1.addBool("response", true);
                        newMessage14.addSCObject(data1);

                        if (clientinfos.ContainsKey(cSock))
                        {
                            clientinfos.TryGetValue(cSock, out value_Value);
                            sendClientMessage(cSock, newMessage14);
                            sendClientMessage(value_Value, newMessage13);
                        }

                        else if (clientinfos.ContainsValue(cSock))
                        {
                            value_Key = clientinfos.FirstOrDefault(x => x.Value == cSock).Key;
                            sendClientMessage(cSock, newMessage14);
                            sendClientMessage(value_Key, newMessage13);
                        }
                        output.outToScreen("" + newMessage14);
                    }
                    break;

                case "senddestroycastle":
                    {
                        output.outToScreen("???");
                        message newMessage15 = new message("SendDestroyMyCastleResponse");
                        scObject data = new scObject("data");
                        data.addBool("response", true);
                        newMessage15.addSCObject(data);

                        message newMessage16 = new message("SendDestroyAiCastleResponse");
                        scObject data1 = new scObject("data1");
                        data1.addBool("response", true);
                        newMessage16.addSCObject(data1);    

                        message newMessage17 = new message("SendVictoryResponse");
                        scObject data2 = new scObject("data2");
                        data2.addBool("response", true);
                        newMessage17.addSCObject(data2);

                        message newMessage18 = new message("SendLoseResponse");
                        scObject data3 = new scObject("data3");
                        data3.addBool("response", true);
                        newMessage18.addSCObject(data3);

                        if (clientinfos.ContainsKey(cSock))
                        {
                            clientinfos.TryGetValue(cSock, out value_Value);
                            sendClientMessage(cSock, newMessage16);
                            sendClientMessage(value_Value, newMessage15);
                            sendClientMessage(cSock, newMessage17);
                            sendClientMessage(value_Value, newMessage18);
                        }

                        else if (clientinfos.ContainsValue(cSock))
                        {
                            value_Key = clientinfos.FirstOrDefault(x => x.Value == cSock).Key;
                            sendClientMessage(cSock, newMessage16);
                            sendClientMessage(value_Key, newMessage15);
                            sendClientMessage(cSock, newMessage17);
                            sendClientMessage(value_Key, newMessage18);
                        }
                        clientinfos.Remove(cSock);
                        output.outToScreen("" + newMessage15);
                    }
                    break;


                default:
                    output.outToScreen("The client sent a message: " + incObject.messageText);
                    break;
            }
        }

        public void Disconnect()
        {
            output.outToScreen("모든 클라이언트와 연결 끊기");
            int i = 0;
            message Disconnet = new message("DisconnectResponse");
            scObject data = new scObject("data");
            data.addInt("disconnect", 1);
            Disconnet.addSCObject(data);
            for(i = 0; i < clientSockets.Count(); i++)
            {
                sendClientMessage(clientSockets[i], Disconnet);
            }
        }

        public void send()
        {
            message newMessage = new message("loginResponse");
            scObject data = new scObject("data");
            data.addBool("response", true);
            newMessage.addSCObject(data);
            sendClientMessage(clientSockets[0], newMessage);
            output.outToScreen("send 실행");
            for (int i = 0; clientSockets.Count > i; i++)
            {
                output.outToScreen("" + clientSockets[i]);
            }
        }

        public void initialize_clientsinfo()
        {
            clientinfos = new Dictionary<Socket, Socket>();
            output.outToScreen("clientsinfo 초기화 완료");
        }

        public void show_clientsinfo()
        {
            output.outToScreen("현재 clientsinfo " + clientinfos.Count + "개");
        }

        public void sendClientMessage(Socket cSock, message mes)
        {
            try
            {
                // convert message into a byte array, wrap the message, then send it
                byte[] messageObject = conversionTools.convertObjectToBytes(mes);
                byte[] readyToSend = conversionTools.wrapMessage(messageObject);
                cSock.Send(readyToSend);
            }
            catch { }
        }
    }
}
