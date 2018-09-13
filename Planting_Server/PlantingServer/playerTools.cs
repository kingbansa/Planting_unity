using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using scMessage;
using MySql;
using System.Globalization;

namespace PlantingServer
{
    class playerTools
    {
        public static string time;
        public static int number, number2, number3, number4, number5, number6, number7;
        public static int wItemNum, fItemNum, sItemNum, nItemNum, sfsItemNum, csItemNum, tsItemNum, Exp = 0;
        // 각각 물, 비료, 태양석, 영양제, 해바라기씨, 선인장씨, 토마토씨, 경험치의 수치 초기화
        public static string wItem, fItem, sItem, nItem, sfsItem, csItem, tsItem;
        // 각각 물, 비료, 태양석, 영양제, 해바라기씨, 선인장씨, 토마토씨
        //public static string name1, name2, name3, name4;
        public static List<string> plantName = new List<string>();
        public static List<int> plantPos = new List<int>();
        public static List<int> plantID = new List<int>();
        public static List<int> Lv = new List<int>();
        public static List<float> waterEXP = new List<float>();
        public static List<float> fertilizerEXP = new List<float>();
        public static List<float> sunEXP = new List<float>();
        //public Dictionary<string, string> tableData = new Dictionary<string, string>();  //key는 컬럼 이름, value는 컬럼 값
        //2차원 배열을 만들어서 넘기면 여러 테이블에 적용가능하다!!! ex) string[][] tableData로 선언해주고 []안에 들어갈 값을 loginScript에서 넘겨주면
        //자기가 원하는 테이블에 맞게 넘겨받을 수 있도록 짜는게 가능.. 물론 그래도 컬럼의 자료형이 int인지 string인지 알고있어야하긴하지만
        //보통은 테이블마다 따로 짠다고하니 다행

        private static playerTools instance;
        public static playerTools Instance
        {
            get
            {
                return instance;
            }
        }

        public static void Time()
        {
            DateTime dateTime = DateTime.Now;
            time = dateTime.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
        }

        public static bool checkLogin(string account, string password)
        {

            Time();
            //output.outToScreen("playerTools - checkLogin 함수 정상 호출.");
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM accounts WHERE account = '" + account + "' AND password = '" + password + "' LIMIT 1;";
            MySqlDataReader reader;
            bool result = false;

            MySqlConnection con1 = database.getConnection();
            MySqlCommand cmd1 = con1.CreateCommand();
            cmd1.CommandText = "update logdata_" + account + " Set LoginTime = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader1;

            MySqlConnection con2 = database.getConnection();
            MySqlCommand cmd2 = con2.CreateCommand();
            cmd2.CommandText = "Select LoginTime From logdata_" + account + " where account = '" + account + "';";
            MySqlDataReader reader2;

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.GetChar("account") > 0)
                {
                    result = true;
                    con2.Open();
                    reader2 = cmd2.ExecuteReader();
                    if (reader2.GetString(0) == "0")
                    {
                        con1.Open();
                        reader1 = cmd1.ExecuteReader();
                        reader1.Read();
                    }
                }
                con.Close();
                con1.Close();
                con2.Close();
            }
            catch
            {
                //output.outToScreen("playerTools - checkLogin의 false 값을 serverTCP - login에 전달");
                con.Close();
                con1.Close();
                //output.outToScreen("" + result);
            }
            output.outToScreen("ID : " + account + " Password : " + password);
            return result;
        }

        public static bool ItemCountCheck(string account, string itemName) // 데이터베이스에 account에 따른 item의 개수
        {
            //output.outToScreen("playerTools - ItemCountCheck 함수 호출 성공.");
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            MySqlDataReader reader;
            bool result = false;
            try
            {
                if (itemName == "wItem")
                {
                    cmd.CommandText = "SELECT " + itemName + " FROM accounts WHERE account = '" + account + "';";
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    wItemNum = reader.GetInt32(0);
                    number = wItemNum;
                    con.Close();
                    result = true;
                    //output.outToScreen("playerTools - ItemCountCheck의 " + itemName + " true 값을 serverTCP - ItemCountCheck에 전달");
                }
                if (itemName == "fItem")
                {
                    cmd.CommandText = "SELECT " + itemName + " FROM accounts WHERE account = '" + account + "';";
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    fItemNum = reader.GetInt32(0);
                    con.Close();
                    number2 = fItemNum;
                    result = true;
                    //output.outToScreen("playerTools - ItemCountCheck의 " + itemName + " true 값을 serverTCP - ItemCountCheck에 전달");
                }

                if (itemName == "sItem")
                {
                    cmd.CommandText = "SELECT " + itemName + " FROM accounts WHERE account = '" + account + "';";
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    sItemNum = reader.GetInt32(0);
                    con.Close();
                    number3 = sItemNum;
                    result = true;
                    //output.outToScreen("playerTools - ItemCountCheck의 " + itemName + " true 값을 serverTCP - ItemCountCheck에 전달");
                }
                if (itemName == "nItem")
                {
                    cmd.CommandText = "SELECT " + itemName + " FROM accounts WHERE account = '" + account + "';";
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    nItemNum = reader.GetInt32(0);
                    con.Close();
                    number4 = nItemNum;
                    result = true;
                    //output.outToScreen("playerTools - ItemCountCheck의 " + itemName + " true 값을 serverTCP - ItemCountCheck에 전달");
                }

                if (itemName == "sfsItem")
                {
                    cmd.CommandText = "SELECT " + itemName + " FROM accounts WHERE account = '" + account + "';";
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    sfsItemNum = reader.GetInt32(0);
                    con.Close();
                    number5 = sfsItemNum;
                    result = true;
                    //output.outToScreen("playerTools - ItemCountCheck의 " + itemName + " true 값을 serverTCP - ItemCountCheck에 전달");
                }

                if (itemName == "csItem")
                {
                    cmd.CommandText = "SELECT " + itemName + " FROM accounts WHERE account = '" + account + "';";
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    csItemNum = reader.GetInt32(0);
                    con.Close();
                    number6 = csItemNum;
                    result = true;
                    //output.outToScreen("playerTools - ItemCountCheck의 " + itemName + " true 값을 serverTCP - ItemCountCheck에 전달");
                }

                if (itemName == "tsItem")
                {
                    cmd.CommandText = "SELECT " + itemName + " FROM accounts WHERE account = '" + account + "';";
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    tsItemNum = reader.GetInt32(0);
                    con.Close();
                    number7 = tsItemNum;
                    result = true;
                    //output.outToScreen("playerTools - ItemCountCheck의 " + itemName + " true 값을 serverTCP - ItemCountCheck에 전달");
                }
            }

            catch
            { 
                con.Close();
                result = false;
            }
            return result;
        }

        public static bool UseItem(string account, string itemName, int itemNum)
        {
            Time();
            //output.outToScreen("playerTools - UseItem 함수 정상 호출.");
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            MySqlDataReader reader;
            bool result = false;

            MySqlConnection con1 = database.getConnection();
            MySqlCommand cmd1 = con1.CreateCommand();
            cmd1.CommandText = "SELECT Use_Item_Score FROM LogData_" + account + " where account = '" + account + "';";
            MySqlDataReader reader1;

            MySqlConnection con2 = database.getConnection();
            MySqlCommand cmd2 = con2.CreateCommand();
            cmd2.CommandText = "UPDATE LogData_" + account + " SET First_Use_Item_Time = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader2;

            MySqlConnection con3 = database.getConnection();
            MySqlCommand cmd3 = con3.CreateCommand();
            cmd3.CommandText = "UPDATE LogData_" + account + " SET Use_Item_Score = Use_Item_Score + 1 where account = '" + account + "';";
            MySqlDataReader reader3;

            MySqlConnection con4 = database.getConnection();
            MySqlCommand cmd4 = con4.CreateCommand();
            cmd4.CommandText = "UPDATE LogData_" + account + " SET Use_Item_Number_5 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader4;

            MySqlConnection con5 = database.getConnection();
            MySqlCommand cmd5 = con5.CreateCommand();
            cmd5.CommandText = "UPDATE LogData_" + account + " SET Use_Item_Number_10 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader5;

            MySqlConnection con6 = database.getConnection();
            MySqlCommand cmd6 = con6.CreateCommand();
            cmd6.CommandText = "UPDATE LogData_" + account + " SET Use_Item_Number_15 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader6;

            MySqlConnection con7 = database.getConnection();
            MySqlCommand cmd7 = con7.CreateCommand();
            cmd7.CommandText = "UPDATE LogData_" + account + " SET Use_Item_Number_20 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader7;

            try
            {
                cmd.CommandText = "UPDATE accounts SET " + itemName + " = " + itemName + " - " + itemNum + " WHERE account = '" + account + "';";
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                output.outToScreen("" + cmd.CommandText);

                con3.Open();
                reader3 = cmd3.ExecuteReader();
                reader3.Read();
                output.outToScreen("" + cmd3.CommandText);

                con1.Open();
                reader1 = cmd1.ExecuteReader();
                reader1.Read();
                output.outToScreen("" + cmd1.CommandText);

                if (reader1.GetInt32(0) == 1)
                {
                    con2.Open();
                    reader2 = cmd2.ExecuteReader();
                    reader2.Read();
                    output.outToScreen("" + cmd2.CommandText);
                    con2.Close();
                }

                else if (reader1.GetInt32(0) == 5)
                {
                    con4.Open();
                    reader4 = cmd4.ExecuteReader();
                    reader4.Read();
                    output.outToScreen("" + cmd4.CommandText);
                    con4.Close();
                }

                else if (reader1.GetInt32(0) == 10)
                {
                    con5.Open();
                    reader5 = cmd5.ExecuteReader();
                    reader5.Read();
                    output.outToScreen("" + cmd5.CommandText);
                    con5.Close();
                }

                else if (reader1.GetInt32(0) == 15)
                {
                    con6.Open();
                    reader6 = cmd6.ExecuteReader();
                    reader6.Read();
                    output.outToScreen("" + cmd6.CommandText);
                    con6.Close();
                }

                else if (reader1.GetInt32(0) == 20)
                {
                    con7.Open();
                    reader7 = cmd7.ExecuteReader();
                    reader7.Read();
                    output.outToScreen("" + cmd7.CommandText);
                    con7.Close();
                }

                else
                {
                    output.outToScreen("else");
                }

                con.Close();
                con1.Close();
                con3.Close();
                result = true;
            }
            catch
            {
                con.Close();
                result = false;
                //output.outToScreen("playerTools - UseItem의 " + itemName + " False 값을 serverTCP - UseItem에 전달");
            }
            return result;

        }


        public static bool getItem(string account, string itemName)
        {
            //output.outToScreen("playerTools - getItem 함수 정상 호출.");
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE accounts SET " + itemName + " = " + itemName + " +1 WHERE account = '" + account + "';";
            MySqlDataReader reader;
            bool result = false;

            Time();
            MySqlConnection con1 = database.getConnection();
            MySqlCommand cmd1 = con1.CreateCommand();
            cmd1.CommandText = "SELECT Get_Item_Score FROM LogData_" + account + " where account = '" + account + "';";
            MySqlDataReader reader1;

            MySqlConnection con2 = database.getConnection();
            MySqlCommand cmd2 = con2.CreateCommand();
            cmd2.CommandText = "UPDATE LogData_" + account + " SET First_Get_Item_Time = '" + time + "' where account = '" + account + "';"; //첫 아이템 먹은 시간을 측정하기 위한 시간
            MySqlDataReader reader2;

            MySqlConnection con3 = database.getConnection();
            MySqlCommand cmd3 = con3.CreateCommand();
            cmd3.CommandText = "UPDATE LogData_" + account + " SET Get_Item_Score = Get_Item_Score + 1 where account = '" + account + "';"; //아무거나 상관없이 먹으면 올라가게 함
            MySqlDataReader reader3;

            MySqlConnection con4 = database.getConnection();
            MySqlCommand cmd4 = con4.CreateCommand();
            cmd4.CommandText = "UPDATE LogData_" + account + " SET Get_Item_Number_5 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader4;

            MySqlConnection con5 = database.getConnection();
            MySqlCommand cmd5 = con5.CreateCommand();
            cmd5.CommandText = "UPDATE LogData_" + account + " SET Get_Item_Number_10 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader5;

            MySqlConnection con6 = database.getConnection();
            MySqlCommand cmd6 = con6.CreateCommand();
            cmd6.CommandText = "UPDATE LogData_" + account + " SET Get_Item_Number_15 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader6;

            MySqlConnection con7 = database.getConnection();
            MySqlCommand cmd7 = con7.CreateCommand();
            cmd7.CommandText = "UPDATE LogData_" + account + " SET Get_Item_Number_20 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader7;
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                result = true;

                con3.Open();
                reader3 = cmd3.ExecuteReader();
                reader3.Read();

                con1.Open();
                reader1 = cmd1.ExecuteReader();
                reader1.Read();

                if (reader1.GetInt32(0) == 1)
                {
                    con2.Open();
                    reader2 = cmd2.ExecuteReader();
                    reader2.Read();
                    con2.Close();
                }

                else if (reader1.GetInt32(0) == 5)
                {
                    con4.Open();
                    reader4 = cmd4.ExecuteReader();
                    reader4.Read();
                    output.outToScreen("" + cmd4.CommandText);
                    con4.Close();
                }

                else if (reader1.GetInt32(0) == 10)
                {
                    con5.Open();
                    reader5 = cmd5.ExecuteReader();
                    reader5.Read();
                    output.outToScreen("" + cmd5.CommandText);
                    con5.Close();
                }

                else if (reader1.GetInt32(0) == 15)
                {
                    con6.Open();
                    reader6 = cmd6.ExecuteReader();
                    reader6.Read();
                    output.outToScreen("" + cmd6.CommandText);
                    con6.Close();
                }

                else if (reader1.GetInt32(0) == 20)
                {
                    con7.Open();
                    reader7 = cmd7.ExecuteReader();
                    reader7.Read();
                    output.outToScreen("" + cmd7.CommandText);
                    con7.Close();
                }

                else
                {
                    output.outToScreen("else");
                }

                con.Close();
                con1.Close();
                con3.Close();
            }

            catch
            {
                con.Close();
                result = true;
                //output.outToScreen("playerTools - getItem의 " + itemName + " False 값을 serverTCP - getItem에 전달");
            }
            return result;
        }

        public static bool UpdatePlantListTable(string account, string plantName, int plantID, string itemName,
            int posNumber, int level, float expAmount, bool isSeedItem)
        {
            //이거 지금 사실상 거의 씨앗심을때만 사용하고잇음..
            //아이템리스트테이블 필드(PlantPos, PlantName, PlantID,  Lv, WaterEXP, SunEXP, FertilizerEXP, NutrientEXP)
            //output.outToScreen("playerTools - UpdatePlantListTable 함수 정상 호출.");
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            if (isSeedItem)
            {
                cmd.CommandText = "INSERT INTO" + " " + "PlantList_" + account + " " + "(PlantPos, PlantName, PlantID, Lv, " + itemName + ")"
                + " VALUES (" + posNumber + ", " + "'" + plantName + "'" + ", " + plantID + ", " + level + ", " + expAmount + ");";
            }
            else if(itemName == "NutrientEXP")
            {
                cmd.CommandText = "UPDATE" + " " + "PlantList_" + account + " " + "SET WaterEXP = WaterEXP + " + expAmount
                    + ", " + "SunEXP = SunEXP + " + expAmount
                    + ", " + "FertilizerEXP = FertilizerEXP + " + expAmount
                    + ", " + "Lv = " + level + " WHERE PlantName = " + "'" + plantName + "'" + ";";
                //cmd.ExecuteNonQuery();
            }
            else
            {
                //나중에 똑같은 식물 먹을 수 있게 바꾸면 여기도 바꿔야됨
                cmd.CommandText = "UPDATE" + " " + "PlantList_" + account + " " + "SET" + " " + itemName + " = "
                + itemName + " + " + expAmount + ", " + "Lv = " + level + " WHERE PlantPos = " +  posNumber + ";";
                //cmd.ExecuteNonQuery();
            }

            //처음인경우는 인서트, 나중에 식물위치 옮기거나할때는 업데이트로 해줘야함
            //cmd.CommandText = "UPDATE" + "plantList_" + account + "SET " + itemName + " = " + itemName + " +1 WHERE account = '" + account + "';";
            MySqlDataReader reader;
            bool result = false;

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                con.Close();
                result = true;
                //output.outToScreen("playerTools - UpdatePlantListTable의 " + itemName + " True 값을 serverTCP - UpdatePlantListTable에 전달.");
            }

            catch
            {
                con.Close();
                result = false;
                //output.outToScreen("playerTools - UpdatePlantListTable의 " + itemName + " False 값을 serverTCP - UpdatePlantListTable에 전달.");
            }
            return result;
        }

        public static bool SelectQuery(string account, string columnName, string tableName)
        {
            //아이템리스트테이블 필드(PlantPos, PlantName, PlantID, Lv, WaterEXP, SunEXP, FertilizerEXP, NutrientEXP)

            //output.outToScreen("playerTools - SelectQuery 함수 정상 호출.");
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT" + " " + columnName + " " + "FROM" + " " + tableName + account + ";";     //WHERE 넣어서도 할수 잇게 오버라이딩 해야...해...
            MySqlDataReader reader;
            bool result = false;
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //plantName.Add(reader["PlantName"] as string);   //밑에랑 같은 거
                    plantName.Add(reader.GetString(reader.GetOrdinal("PlantName")));
                    plantPos.Add(reader.GetInt32(reader.GetOrdinal("PlantPos")));
                    plantID.Add(reader.GetInt32(reader.GetOrdinal("PlantID")));
                    Lv.Add(reader.GetInt32(reader.GetOrdinal("Lv")));
                    waterEXP.Add(reader.GetFloat(reader.GetOrdinal("WaterEXP")));
                    sunEXP.Add(reader.GetFloat(reader.GetOrdinal("SunEXP")));
                    fertilizerEXP.Add(reader.GetFloat(reader.GetOrdinal("FertilizerEXP")));
                }
                //reader.Read();    //reader.getInt32(0);
                con.Close();
                result = true;
                //output.outToScreen("playerTools - SelectQuery의 True 값을 serverTCP - SelectQuery에 전달.");
            }

            catch
            {
                con.Close();
                result = false;
                output.outToScreen("playerTools - SelectQuery의 False 값을 serverTCP - SelectQuery에 전달.");
            }
            output.outToScreen("DATABASE " + columnName + " from " + tableName + " is selected.");
            return result;
        }

        public static bool UpdatePlantExp(string account, int plantPos, int level, int expName, float expAmount)
        {
            //아이템리스트테이블 필드(PlantPos, PlantName, PlantID, Lv, WaterEXP, SunEXP, FertilizerEXP, NutrientEXP)
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();

            switch (expName)
            {
                // water 0, sun 1, fertilizer 2, nutrient 3으로 설정한다 일단, 나중에 이넘으로 바꿔야...
                case 0:
                    cmd.CommandText = "UPDATE" + " " + "PlantList_" + account + " " + "SET WaterEXP = " + expAmount
                        + ", " + "Lv = " + level + " WHERE PlantPos = " + plantPos + ";";
                    break;

                case 1:
                    cmd.CommandText = "UPDATE" + " " + "PlantList_" + account + " " + "SET SunEXP = " + expAmount
                        + ", " + "Lv = " + level + " WHERE PlantPos = " + plantPos + ";";
                    break;

                case 2:
                    cmd.CommandText = "UPDATE" + " " + "PlantList_" + account + " " + "SET FertilizerEXP = " + expAmount
                        + ", " + "Lv = " + level + " WHERE PlantPos = " + plantPos + ";";
                    break;

                case 3:
                    cmd.CommandText = "UPDATE" + " " + "PlantList_" + account + " "
                        + "SET WaterEXP = " + expAmount + ", "
                        + "SunEXP = " + expAmount + ", "
                        + "FertilizerEXP = " + expAmount + ", "
                        + "Lv = " + level + " WHERE PlantPos = " + plantPos + ";";
                    break;

                default:
                    output.outToScreen("PlayerTools - UpdatePlantExp has Error!");
                    break;
            }
            output.outToScreen("***********PlayerTools - UpdatePlantExp Success************************");
            MySqlDataReader reader;
            #region
            MySqlConnection con1 = database.getConnection();
            MySqlCommand cmd1 = con1.CreateCommand();
            cmd1.CommandText = "Select Lv from plantlist_" + account + " where plantpos = " + plantPos + ";";
            MySqlDataReader reader1;

            MySqlConnection con2 = database.getConnection();
            MySqlCommand cmd2 = con2.CreateCommand();
            cmd2.CommandText = "Update LogData_" + account + " set Lv1 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader2;

            MySqlConnection con3 = database.getConnection();
            MySqlCommand cmd3 = con3.CreateCommand();
            cmd3.CommandText = "Select Lv1 from logdata_" + account + " where account = '" + account + "';";
            MySqlDataReader reader3;

            MySqlConnection con4 = database.getConnection();
            MySqlCommand cmd4 = con4.CreateCommand();
            cmd4.CommandText = "Update LogData_" + account + " set Lv2 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader4;

            MySqlConnection con5 = database.getConnection();
            MySqlCommand cmd5 = con5.CreateCommand();
            cmd5.CommandText = "Select Lv2 from logdata_" + account + " where account = '" + account + "';";
            MySqlDataReader reader5;

            MySqlConnection con6 = database.getConnection();
            MySqlCommand cmd6 = con6.CreateCommand();
            cmd6.CommandText = "Update LogData_" + account + " set Lv3 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader6;

            MySqlConnection con7 = database.getConnection();
            MySqlCommand cmd7 = con7.CreateCommand();
            cmd7.CommandText = "Select Lv3 from logdata_" + account + " where account = '" + account + "';";
            MySqlDataReader reader7;

            MySqlConnection con8 = database.getConnection();
            MySqlCommand cmd8 = con8.CreateCommand();
            cmd8.CommandText = "Update LogData_" + account + " set Lv4 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader8;

            MySqlConnection con9 = database.getConnection();
            MySqlCommand cmd9 = con9.CreateCommand();
            cmd9.CommandText = "Select Lv4 from logdata_" + account + " where account = '" + account + "';";
            MySqlDataReader reader9;

            MySqlConnection con10 = database.getConnection();
            MySqlCommand cmd10 = con10.CreateCommand();
            cmd10.CommandText = "Update LogData_" + account + " set Lv5 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader10;

            MySqlConnection con11 = database.getConnection();
            MySqlCommand cmd11 = con11.CreateCommand();
            cmd11.CommandText = "Select Lv5 from logdata_" + account + " where account = '" + account + "';";
            MySqlDataReader reader11;

            MySqlConnection con12 = database.getConnection();
            MySqlCommand cmd12 = con12.CreateCommand();
            cmd12.CommandText = "Update LogData_" + account + " set Lv6 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader12;

            MySqlConnection con13 = database.getConnection();
            MySqlCommand cmd13 = con13.CreateCommand();
            cmd13.CommandText = "Select Lv6 from logdata_" + account + " where account = '" + account + "';";
            MySqlDataReader reader13;

            MySqlConnection con14 = database.getConnection();
            MySqlCommand cmd14 = con14.CreateCommand();
            cmd14.CommandText = "Update LogData_" + account + " set Lv7 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader14;

            MySqlConnection con15 = database.getConnection();
            MySqlCommand cmd15 = con15.CreateCommand();
            cmd15.CommandText = "Select Lv7 from logdata_" + account + " where account = '" + account + "';";
            MySqlDataReader reader15;

            MySqlConnection con16 = database.getConnection();
            MySqlCommand cmd16 = con16.CreateCommand();
            cmd16.CommandText = "Update LogData_" + account + " set Lv8 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader16;

            MySqlConnection con17 = database.getConnection();
            MySqlCommand cmd17 = con17.CreateCommand();
            cmd17.CommandText = "Select Lv8 from logdata_" + account + " where account = '" + account + "';";
            MySqlDataReader reader17;

            MySqlConnection con18 = database.getConnection();
            MySqlCommand cmd18 = con18.CreateCommand();
            cmd18.CommandText = "Update LogData_" + account + " set Lv9 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader18;

            MySqlConnection con19 = database.getConnection();
            MySqlCommand cmd19 = con19.CreateCommand();
            cmd19.CommandText = "Select Lv9 from logdata_" + account + " where account = '" + account + "';";
            MySqlDataReader reader19;

            MySqlConnection con20 = database.getConnection();
            MySqlCommand cmd20 = con20.CreateCommand();
            cmd20.CommandText = "Update LogData_" + account + " set Lv10 = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader20;

            MySqlConnection con21 = database.getConnection();
            MySqlCommand cmd21 = con21.CreateCommand();
            cmd21.CommandText = "Select Lv10 from logdata_" + account + " where account = '" + account + "';";
            MySqlDataReader reader21;
            #endregion
            bool result = false;
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                con.Close();
                #region
                con1.Open();
                reader1 = cmd1.ExecuteReader();
                reader1.Read();          
                if (reader1.GetInt32(0) == 1)
                {
                    Time();
                    con3.Open();
                    reader3 = cmd3.ExecuteReader();
                    reader3.Read();
                    if (reader3.GetString(0) == "0")
                    {
                        con2.Open();
                        reader2 = cmd2.ExecuteReader();
                        reader2.Read();
                    }
                    con1.Close();
                    con2.Close();
                    con3.Close();
                }

                if (reader1.GetInt32(0) == 2)
                {
                    Time();
                    con5.Open();
                    reader5 = cmd5.ExecuteReader();
                    reader5.Read();
                    if (reader5.GetString(0) == "0")
                    {
                        con4.Open();
                        reader4 = cmd4.ExecuteReader();
                        reader4.Read();
                    }
                    con1.Close();
                    con4.Close();
                    con5.Close();
                }

                if (reader1.GetInt32(0) == 3)
                {
                    Time();
                    con7.Open();
                    reader7 = cmd7.ExecuteReader();
                    reader7.Read();
                    if (reader7.GetString(0) == "0")
                    {
                        con6.Open();
                        reader6 = cmd6.ExecuteReader();
                        reader6.Read();
                    }
                    con1.Close();
                    con6.Close();
                    con7.Close();
                }

                if (reader1.GetInt32(0) == 4)
                {
                    Time();
                    con9.Open();
                    reader9 = cmd9.ExecuteReader();
                    reader9.Read();
                    if (reader9.GetString(0) == "0")
                    {
                        con8.Open();
                        reader8 = cmd8.ExecuteReader();
                        reader8.Read();
                    }
                    con1.Close();
                    con8.Close();
                    con9.Close();
                }

                if (reader1.GetInt32(0) == 5)
                {
                    Time();
                    con11.Open();
                    reader11 = cmd11.ExecuteReader();
                    reader11.Read();
                    if (reader11.GetString(0) == "0")
                    {
                        con10.Open();
                        reader10 = cmd10.ExecuteReader();
                        reader10.Read();
                    }
                    con1.Close();
                    con10.Close();
                    con11.Close();
                }

                if (reader1.GetInt32(0) == 6)
                {
                    Time();
                    con13.Open();
                    reader13 = cmd13.ExecuteReader();
                    reader13.Read();
                    if (reader13.GetString(0) == "0")
                    {
                        con12.Open();
                        reader12 = cmd12.ExecuteReader();
                        reader12.Read();
                    }
                    con1.Close();
                    con12.Close();
                    con13.Close();
                }

                if (reader1.GetInt32(0) == 7)
                {
                    Time();
                    con15.Open();
                    reader15 = cmd15.ExecuteReader();
                    reader15.Read();
                    if (reader15.GetString(0) == "0")
                    {
                        con14.Open();
                        reader14 = cmd14.ExecuteReader();
                        reader14.Read();
                    }
                    con1.Close();
                    con14.Close();
                    con15.Close();
                }

                if (reader1.GetInt32(0) == 8)
                {
                    Time();
                    con17.Open();
                    reader17 = cmd17.ExecuteReader();
                    reader17.Read();
                    if (reader17.GetString(0) == "0")
                    {
                        con16.Open();
                        reader16 = cmd16.ExecuteReader();
                        reader16.Read();
                    }
                    con1.Close();
                    con16.Close();
                    con17.Close();
                }

                if (reader1.GetInt32(0) == 9)
                {
                    Time();
                    con19.Open();
                    reader19 = cmd19.ExecuteReader();
                    reader19.Read();
                    if (reader19.GetString(0) == "0")
                    {
                        con18.Open();
                        reader18 = cmd18.ExecuteReader();
                        reader18.Read();
                    }
                    con1.Close();
                    con18.Close();
                    con19.Close();
                }

                if (reader1.GetInt32(0) == 10)
                {
                    Time();
                    con21.Open();
                    reader21 = cmd21.ExecuteReader();
                    reader21.Read();
                    if (reader21.GetString(0) == "0")
                    {
                        con20.Open();
                        reader20 = cmd20.ExecuteReader();
                        reader20.Read();
                    }
                    con1.Close();
                    con20.Close();
                    con21.Close();
                }
                #endregion
                result = true;
                output.outToScreen("PlayerTools - UpdatePlantExp is success!");
            }
            catch
            {
                con.Close();
                result = false;
                output.outToScreen("PlayerTools - UpdatePlantExp has Error!");
            }
            return result;
        }

        public static bool UpdatePlantID(string account, int plantPos, int plantID)
        {
            //아이템리스트테이블 필드(PlantPos, PlantName, PlantID, Lv, WaterEXP, SunEXP, FertilizerEXP, NutrientEXP)
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE" + " " + "PlantList_" + account + " " + "SET PlantID = " + plantID 
                           + " WHERE PlantPos = " + plantPos + ";";
            
            output.outToScreen("***********PlayerTools - UpdatePlantID Success************************");
            MySqlDataReader reader;
            bool result = false;
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                con.Close();
                result = true;
                output.outToScreen("PlayerTools - UpdatePlantID is success!");
            }
            catch
            {
                con.Close();
                result = false;
                output.outToScreen("PlayerTools - UpdatePlantID has Error!");
            }
            return result;
        }

        public static bool urlcheck(string account, string password)
        {
            //output.outToScreen("playerTools - urlcheck 함수 정상 호출.");
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT url FROM accounts where account = '" + account + "' AND password = '" + password + "';";
            MySqlDataReader reader;
            bool result = false;
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) != "0")
                {
                    result = true;
                    //output.outToScreen("playerTools - urlcheck의 True 값을 serverTCP - urlcheck에 전달.");
                }

                else
                {
                    result = false;
                    //output.outToScreen("playerTools - urlcheck의 False 값을 serverTCP - urlcheck에 전달.");
                }
            }
            con.Close();
            output.outToScreen("URL 중복 검사.");
            return result;
        }

        public static bool idcheck(string account)
        {
            //output.outToScreen("playerTools - idcheck 함수 정상 호출.");
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT account FROM accounts where account = '" + account + "';";
            MySqlDataReader reader;
            bool result;
            con.Open();
            reader = cmd.ExecuteReader();
            if (reader.Read() == true)
            {
                result = true;
                //output.outToScreen("playerTools - urlcheck의 True 값을 playerTools - createregister에 전달.");
            }
            else
            {
                result = false;
                //output.outToScreen("playerTools - urlcheck의 False 값을 playerTools - createregister에 전달.");
            }
            con.Close();
            //output.outToScreen("ID 중복 검사.");
            return result;
        }

        public static bool createregister(string account, string password)
        {
            Time();

            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            MySqlDataReader reader;
            bool result = false;

            MySqlConnection con1 = database.getConnection();
            MySqlCommand cmd1 = con1.CreateCommand();
            cmd1.CommandText = "insert into Logdata_" + account + " (account) values ('" + account + "');";
            MySqlDataReader reader1;

            MySqlConnection con2 = database.getConnection();
            MySqlCommand cmd2 = con2.CreateCommand();
            cmd2.CommandText = "update logdata_" + account + " Set CreateRegister_time = '" + time + "' where account = '" + account + "';";
            MySqlDataReader reader2;

            if (idcheck(account) == false)
            {
                //MySqlDataReader reader2;
                try
                {
                    //아이템리스트테이블 필드(PlantPos, PlantName, PlantID, Lv, WaterEXP, SunEXP, FertilizerEXP, NutrientEXP)
                    con.Open();
                    cmd.CommandText = "CREATE TABLE" + " " + "PlantList_" + account
                                    + " " + "(PlantPos INT(10) NOT NULL PRIMARY KEY default 0"
                                    + ", " + "PlantID INT(10) NOT NULL default 0"
                                    + ", " + "PlantName varchar(50) NOT NULL default 0"
                                    + ", " + "Lv int(10) NOT NULL default 1"
                                    + ", " + "WaterEXP float NOT NULL default 0"
                                    + ", " + "SunEXP float NOT NULL default 0"
                                    + ", " + "FertilizerEXP float NOT NULL default 0);"; //유저의 아이디로 만든 테이블, 그 유저가 가지고있는 식물을 관리
                    cmd.ExecuteNonQuery();

                    //사용자 테이블 구조 바꾸기, 나중에
                    cmd.CommandText = "CREATE TABLE" + " " + "LogData_" + account
                                    + " " + "(account VarChar(100) Not null default 0"
                                    + ", " + "LoginTime VARCHAR(100) NOT NULL default 0"
                                    + ", " + "Use_Item_Score INT(32) NOT NULL default 0"
                                    + ", " + "Get_Item_Score INT(32) NOT NULL default 0"
                                    + ", " + "CreateRegister_Time VARCHAR(100) NOT NULL default 0"
                                    + ", " + "First_Get_Item_Time VARCHAR(100) NOT NULL default 0"
                                    + ", " + "First_Use_Item_Time VARCHAR(100) NOT NULL default 0"
                                    + ", " + "Create_Url_Time VARCHAR(100) NOT NULL default 0"
                                    + ", " + "Lv1 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Lv2 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Lv3 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Lv4 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Lv5 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Lv6 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Lv7 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Lv8 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Lv9 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Lv10 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Use_Item_Number_5 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Use_Item_Number_10 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Use_Item_Number_15 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Use_Item_Number_20 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Get_Item_Number_5 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Get_Item_Number_10 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Get_Item_Number_15 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Get_Item_Number_20 Varchar(200) NOT NULL Default 0"
                                    + ", " + "Stay_SetPlantsBedScene Float(255,0) not null default 0"
                                    + ", " + "Stay_PlantInfoScene Float(255,0) not null default 0"
                                    + ", " + "Stay_GPSScene Float(255,0) not null default 0"
                                    + ", " + "Stay_LobbyScene Float(255,0) not null default 0"
                                    + ", " + "Stay_BattleScene Float(255,0) not null default 0"
                                    + ", " + "Number_Stay_SetPlantsBedScene INT(32) not null default 0"
                                    + ", " + "Number_Stay_PlantInfoScene INT(32) not null default 0"
                                    + ", " + "Number_Stay_GPSScene INT(32) not null default 0"
                                    + ", " + "Number_Stay_LobbyScene INT(32) not null default 0"
                                    + ", " + "Number_Stay_BattleScene INT(32) not null default 0);";
                    cmd.ExecuteNonQuery();

                    /*cmd.CommandText = "UPDATE LogData_" + account + " SET CreateRegister_Time = '" + time + "';";
                    cmd.ExecuteNonQuery();*/

                    cmd.CommandText = "INSERT INTO accounts(account, password, url) VALUES ('" + account + "','" + password + "', 0);";
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    con1.Open();
                    reader1 = cmd1.ExecuteReader();
                    reader1.Read();

                    con2.Open();
                    reader2 = cmd2.ExecuteReader();
                    reader2.Read();

                    result = true;
                    con1.Close();
                    con.Close();
                }
                catch
                {
                    con.Close();
                }
            }
            output.outToScreen("CreateResister result = " + result);
            return result;
        }

        public static bool createurl(string url, string account)
        {
            Time();
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE accounts SET url = '" + url + "' WHERE account = '" + account + "';";
            MySqlDataReader reader;
            bool result = false;

            MySqlConnection con1 = database.getConnection();
            MySqlCommand cmd1 = con1.CreateCommand();
            cmd1.CommandText = "SELECT url FROM accounts WHERE account = '" + account + "';";
            MySqlDataReader reader1;

            MySqlConnection con2 = database.getConnection();
            MySqlCommand cmd2 = con2.CreateCommand();
            cmd2.CommandText = "UPDATE LogData_" + account + " SET Create_Url_Time = '" + time + "' where account = '" + account +"';";
            MySqlDataReader reader2;

            try
            {
                con1.Open();
                reader1 = cmd1.ExecuteReader();
                reader1.Read();

                if (reader1.GetChar("url") != '0')
                {
                    result = false;
                }

                else
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    result = true;
                    con2.Open();
                    reader2 = cmd2.ExecuteReader();
                    reader2.Read();
                    con2.Close();
                }

            }
            catch
            {
                con.Close();
                con1.Close();
            }
            return result;
        }

        public static bool plusExp(string account, string password)
        {
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE accounts SET Exp = Exp + 1 WHERE account = '" + account + "' AND password = '" + password + "';";
            MySqlDataReader reader;
            bool result = false;

            try
            {
                con.Open();

                reader = cmd.ExecuteReader();
                reader.Read();
                con.Close();
                result = true;
                //output.outToScreen("playerTools - plusExp의 True 값을 serverTCP - plusExp에 전달.");
            }

            catch
            {
                con.Close();
                result = true;
                //output.outToScreen("playerTools - plusExp의 False 값을 serverTCP - plusExp에 전달.");
            }
            return result;
        }

        public static bool CheckExp(string account, string password)
        {
            //output.outToScreen("playerTools - CheckExp 함수 정상 호출.");
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "select Exp from accounts where account = '" + account + "' AND password = '" + password + "';";
            MySqlDataReader reader;
            bool result = false;

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                Exp = reader.GetInt32(0);
                con.Close();
                result = true;
                //output.outToScreen("playerTools - CheckExp의 True 값을 serverTCP - CheckExp 전달.");
            }

            catch
            {
                con.Close();
                result = false;
                //output.outToScreen("playerTools - CheckExp의 False 값을 serverTCP - CheckExp 전달.");
            }

            output.outToScreen("DATABASE EXP CHECK : " + Exp);
            return result;
        }
        #region
        public static void Clear() //이름 초기화를 위해서 임시방편으로 만들었다
        {
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "update accounts set name1 = '0', name2 = '0', name3 = '0', name4 = '0'";
            MySqlDataReader reader;
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                con.Close();
            }

            catch
            {
                con.Close();
            }
        }

        public static bool sendtime(string account, float time, string scenename)
        {
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "update logdata_" + account + " Set " + scenename + " = " + scenename + " + " + time + " where account = '" + account + "';";
            output.outToScreen("" + cmd.CommandText);
            MySqlDataReader reader;

            MySqlConnection con1 = database.getConnection();
            MySqlCommand cmd1 = con1.CreateCommand();
            cmd1.CommandText = "update logdata_" + account + " Set Number_" + scenename + " = Number_" + scenename + " + 1 where account = '" + account + "';";
            output.outToScreen("" + cmd1.CommandText);
            MySqlDataReader reader1;

            bool result = false;
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();

                con1.Open();
                reader1 = cmd1.ExecuteReader();
                reader1.Read();

                con.Close();
                con1.Close();
                result = true;
            }

            catch
            {
                con.Close();
            }
            output.outToScreen("Scenetime " + time + "  " + scenename);
            return result;
        }

        public static int addrank(string account)
        {
            int rank = 0;
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "update accounts set rankpoint = rankpoint + 10 where account = '" + account + "';";
            MySqlDataReader reader;

            MySqlConnection con2 = database.getConnection();
            MySqlCommand cmd2 = con2.CreateCommand();
            cmd2.CommandText = "select rankpoint from accounts where account = '" + account + "';";
            MySqlDataReader reader2;

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                con.Close();

                con2.Open();
                reader2 = cmd2.ExecuteReader();
                reader2.Read();
                rank = reader2.GetInt32(0);
                con2.Close();
            }
            catch
            {
                con.Close();
            }
            output.outToScreen(account + " add rankpoint = " + rank);
            return rank;
        }

        public static int subtractionrank(string account)
        {
            int rank = 0;
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "update accounts set rankpoint = rankpoint - 10 where account = '" + account + "';";
            MySqlDataReader reader;

            MySqlConnection con2 = database.getConnection();
            MySqlCommand cmd2 = con2.CreateCommand();
            cmd2.CommandText = "select rankpoint from accounts where account = '" + account + "';";
            MySqlDataReader reader2;

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                con.Close();

                con2.Open();
                reader2 = cmd2.ExecuteReader();
                reader2.Read();
                rank = reader2.GetInt32(0);
                con2.Close();
            }
            catch
            {
                con.Close();
            }
            output.outToScreen(account + " subtract rankpoint = " + rank);
            return rank;
        }

        public static bool sendplantname(string account, string plantname)
        {
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "select name1 from accounts where account = '" + account + "';";
            MySqlDataReader reader;

            MySqlConnection con2 = database.getConnection();
            MySqlCommand cmd2 = con2.CreateCommand();
            cmd2.CommandText = "select name2 from accounts where account = '" + account + "';";
            MySqlDataReader reader2;

            MySqlConnection con3 = database.getConnection();
            MySqlCommand cmd3 = con3.CreateCommand();
            cmd3.CommandText = "select name3 from accounts where account = '" + account + "';";
            MySqlDataReader reader3;

            MySqlConnection con8 = database.getConnection();
            MySqlCommand cmd8 = con8.CreateCommand();
            cmd8.CommandText = "select name4 from accounts where account = '" + account + "';";
            MySqlDataReader reader8;

            MySqlConnection con4 = database.getConnection();
            MySqlCommand cmd4 = con4.CreateCommand();
            cmd4.CommandText = "UPDATE accounts SET name1 = '" + plantname + "' WHERE account = '" + account + "';";
            MySqlDataReader reader4;

            MySqlConnection con5 = database.getConnection();
            MySqlCommand cmd5 = con5.CreateCommand();
            cmd5.CommandText = "UPDATE accounts SET name2 = '" + plantname + "' WHERE account = '" + account + "';";
            MySqlDataReader reader5;

            MySqlConnection con6 = database.getConnection();
            MySqlCommand cmd6 = con6.CreateCommand();
            cmd6.CommandText = "UPDATE accounts SET name3 = '" + plantname + "' WHERE account = '" + account + "';";
            MySqlDataReader reader6;

            MySqlConnection con7 = database.getConnection();
            MySqlCommand cmd7 = con7.CreateCommand();
            cmd7.CommandText = "UPDATE accounts SET name4 = '" + plantname + "' WHERE account = '" + account + "';";
            MySqlDataReader reader7;

            bool result = false;
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();

                con2.Open();
                reader2 = cmd2.ExecuteReader();
                reader2.Read();

                con3.Open();
                reader3 = cmd3.ExecuteReader();
                reader3.Read();

                con8.Open();
                reader8 = cmd8.ExecuteReader();
                reader8.Read();

                if (reader.GetString(0) == "0") //첫번째에 아무것도 없을때.
                {
                    con4.Open();
                    reader4 = cmd4.ExecuteReader();
                    reader4.Read();
                }
                else if (reader.GetString(0) != "0" && reader2.GetString(0) == "0") //첫번째 있고 두번째 없을때.
                {
                    con5.Open();
                    reader5 = cmd5.ExecuteReader();
                    reader5.Read();
                }
                else if (reader2.GetString(0) != "0" && reader3.GetString(0) == "0") //두번째 있고 세번째 없을때.
                {
                    con6.Open();
                    reader6 = cmd6.ExecuteReader();
                    reader6.Read();
                }
                else if (reader3.GetString(0) != "0" && reader8.GetString(0) == "0")
                {
                    con7.Open();
                    reader7 = cmd7.ExecuteReader();
                    reader7.Read();
                }
                else
                {
                    output.outToScreen("뭔지 모르겠다");
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool AllDeletePlantName(string account)
        {
            /*string name1 = "";
            string name2 = "";
            string name3 = "";
            string name4 = "";*/
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "update accounts Set name1 = 0, name2 = 0, name3 = 0, name4 = 0 where account = '" + account + "';";
            MySqlDataReader reader;
            bool result = false;

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                result = true;
                /*name1 = "0";
                name2 = "0";
                name3 = "0";
                name4 = "0";*/
            }

            catch
            {
                con.Close();
            }

            return result;
        }

        public static bool DeletePlantName(string account, string plantname)
        {
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            //cmd.CommandText = "Update accounts Set name1 = 0, name2 = 0, name3 = 0, name4 = 0 where account = '" + account + "';";
            cmd.CommandText = "Select name1 from accounts where account = '" + account + "';";
            MySqlDataReader reader;
            bool result = false;

            MySqlConnection con2 = database.getConnection();
            MySqlCommand cmd2 = con2.CreateCommand();
            cmd2.CommandText = "Select name2 from accounts where account = '" + account + "';";
            MySqlDataReader reader2;

            MySqlConnection con3 = database.getConnection();
            MySqlCommand cmd3 = con3.CreateCommand();
            cmd3.CommandText = "Select name3 from accounts where account = '" + account + "';";
            MySqlDataReader reader3;

            MySqlConnection con4 = database.getConnection();
            MySqlCommand cmd4 = con4.CreateCommand();
            cmd4.CommandText = "Select name4 from accounts where account = '" + account + "';";
            MySqlDataReader reader4;

            MySqlConnection con5 = database.getConnection();
            MySqlCommand cmd5 = con5.CreateCommand();
            cmd5.CommandText = "Update accounts Set name1 = 0 where account = '" + account + "';";
            MySqlDataReader reader5;

            MySqlConnection con6 = database.getConnection();
            MySqlCommand cmd6 = con6.CreateCommand();
            cmd6.CommandText = "Update accounts Set name2 = 0 where account = '" + account + "';";
            MySqlDataReader reader6;

            MySqlConnection con7 = database.getConnection();
            MySqlCommand cmd7 = con7.CreateCommand();
            cmd7.CommandText = "Update accounts Set name3 = 0 where account = '" + account + "';";
            MySqlDataReader reader7;

            MySqlConnection con8 = database.getConnection();
            MySqlCommand cmd8 = con8.CreateCommand();
            cmd8.CommandText = "Update accounts Set name4 = 0 where account = '" + account + "';";
            MySqlDataReader reader8;
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();

                con2.Open();
                reader2 = cmd2.ExecuteReader();
                reader2.Read();

                con3.Open();
                reader3 = cmd3.ExecuteReader();
                reader3.Read();

                con4.Open();
                reader4 = cmd4.ExecuteReader();
                reader4.Read();

                if (reader.GetString(0) == plantname)
                {
                    con5.Open();
                    reader5 = cmd5.ExecuteReader();
                    reader5.Read();
                }
                else if (reader2.GetString(0) == plantname)
                {
                    con6.Open();
                    reader6 = cmd6.ExecuteReader();
                    reader6.Read();
                }
                else if (reader3.GetString(0) == plantname)
                {
                    con7.Open();
                    reader7 = cmd7.ExecuteReader();
                    reader7.Read();
                }
                else if (reader4.GetString(0) == plantname)
                {
                    con8.Open();
                    reader8 = cmd8.ExecuteReader();
                    reader8.Read();
                }
                else
                {
                    output.outToScreen("이런 시발");
                }

                result = true;
            }
            catch
            {
                con.Close();
                result = false;
            }
            return result;
        }

        public static string GetPlantName(string account)
        {
            string name1 = "";
            string name2 = "";
            string name3 = "";
            string name4 = "";
            MySqlConnection con = database.getConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT name1 FROM accounts where account = '" + account + "';";
            MySqlDataReader reader;

            MySqlConnection con2 = database.getConnection();
            MySqlCommand cmd2 = con2.CreateCommand();
            cmd2.CommandText = "SELECT name2 FROM accounts where account = '" + account + "';";
            MySqlDataReader reader2;

            MySqlConnection con3 = database.getConnection();
            MySqlCommand cmd3 = con3.CreateCommand();
            cmd3.CommandText = "SELECT name3 FROM accounts where account = '" + account + "';";
            MySqlDataReader reader3;

            MySqlConnection con4 = database.getConnection();
            MySqlCommand cmd4 = con4.CreateCommand();
            cmd4.CommandText = "SELECT name4 FROM accounts where account = '" + account + "';";
            MySqlDataReader reader4;
            //bool result = false;
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                name1 = reader.GetString(0);
                output.outToScreen("GetPlantName " + name1);
                con2.Open();
                reader2 = cmd2.ExecuteReader();
                reader2.Read();
                name2 = reader2.GetString(0);
                output.outToScreen("GetPlantName " + name2);
                con3.Open();
                reader3 = cmd3.ExecuteReader();
                reader3.Read();
                name3 = reader3.GetString(0);
                output.outToScreen("GetPlantName " + name3);
                con4.Open();
                reader4 = cmd4.ExecuteReader();
                reader4.Read();
                name4 = reader4.GetString(0);
                output.outToScreen("GetPlantName " + name4);
                //result = true;
            }
            catch
            {
                con.Close();
                //result = false;
            }
            return name1+ "," + name2 + "," + name3 + "," +name4;
        }
        #endregion
    }
}