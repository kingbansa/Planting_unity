using System;
using System.Collections.Generic; // Q.List를 쓰기위해서 사용한건가?

namespace scMessage
{
    [Serializable] //객체 직렬화 가능
    public class message
    {
        public string messageText; //messageText라는 public 문자열 변수 선언
        private List<scObject> scObjects = new List<scObject>();
        public int messageID;

        public message(string x)
        {
            messageText = x; //message 함수로 들어오면 messageText는 x가 된다.
        }

        public void addSCObject(scObject x)  //void 이기떄문에 리턴값 없음.
        {
            scObjects.Add(x); // List<scObject>.Add(scObject item) 개체를 List<t>끝부분에 추가
        }

        public scObject getSCObject(string x) // Q.scObject가 자료형?
        {
            for (int i = 0; i < scObjects.Count; i++) //scObject.Count는 List<t>에 속해있는 갯수
            {
                if (scObjects[i].name == x) //scObjects[i]에 있는 이름중 x랑 같은 이름이 있으면 그 값을 리턴해준다.
                    return scObjects[i]; // Q.어디에 리턴 하는거지? scObjects[i] == x가 되는것같다.
            }
            return null;
        }

        public scObject getSCObject(int x)
        {
            return scObjects[x]; //getSCObject함수에 자료형이 int인 x가 들어오면 scObjects[x]를 리턴한다.
        }

        public int getSCObjectCount() // Q.매개변수 자리에 아무것도 없다 왜지?
        {
            return scObjects.Count; //getSCObjectCount를 호출하면 자료형이 int인 값(scObjects.Count)을 리턴한다. 
        }
    }

    [Serializable] //http://ikpil.com/1209 << Serializable 한마디로 파일 전송과 저장을 손쉽게 하기 위해서 사용한다.
    public class scObject
    {
        public string name;

        private List<scString> stringL = new List<scString>();
        private List<scBool> boolL = new List<scBool>();
        private List<scInt> intL = new List<scInt>();
        private List<scLong> longL = new List<scLong>();
        private List<scFloat> floatL = new List<scFloat>();
        private List<scDouble> doubleL = new List<scDouble>();
        private List<scObject> objectL = new List<scObject>();

        public void addSCObject(scObject x)
        {
            objectL.Add(x);
        }

        public scObject getSCObject(string x)
        {
            for (int i = 0; i < objectL.Count; i++)
            {
                if (objectL[i].name == x)
                    return objectL[i];
            }
            return null;
        }

        public scObject getSCObject(int x)
        {
            return objectL[x];
        }

        public int getSCObjectCount()
        {
            return objectL.Count;
        }

        public scObject(string x)
        {
            name = x;
        }

        public void addString(string x, string y)
        {
            stringL.Add(new scString(x, y));
        }

        public string getString(string x)
        {
            for (int i = 0; i < stringL.Count; i++)
            {
                if (stringL[i].name == x)
                    return stringL[i].value;
            }
            return null;
        }

        public void addBool(string x, bool y)
        {
            boolL.Add(new scBool(x, y));
        }

        public bool getBool(string x)
        {
            for (int i = 0; i < boolL.Count; i++)
            {
                if (boolL[i].name == x)
                    return boolL[i].value;
            }
            return false;
        }

        public void addInt(string x, int y)
        {
            intL.Add(new scInt(x, y));
        }

        public int getInt(string x)
        {
            for (int i = 0; i < intL.Count; i++)
            {
                if (intL[i].name == x)
                    return intL[i].value;
            }
            return 0;
        }

        public void addLong(string x, long y)
        {
            longL.Add(new scLong(x, y));
        }

        public long getLong(string x)
        {
            for (int i = 0; i < longL.Count; i++)
            {
                if (longL[i].name == x)
                    return longL[i].value;
            }
            return 0;
        }

        public void addFloat(string x, float y)
        {
            floatL.Add(new scFloat(x, y));
        }

        public float getFloat(string x)
        {
            for (int i = 0; i < floatL.Count; i++)
            {
                if (floatL[i].name == x)
                    return floatL[i].value;
            }
            return 0F;
        }

        public void addDouble(string x, double y)
        {
            doubleL.Add(new scDouble(x, y));
        }

        public double getDouble(string x)
        {
            for (int i = 0; i < doubleL.Count; i++)
            {
                if (doubleL[i].name == x)
                    return doubleL[i].value;
            }
            return 0.0;
        }
    }

    [Serializable]
    public class scBool
    {
        public string name;
        public bool value;

        public scBool(string x, bool y)
        {
            name = x;
            value = y;
        }
    }

    [Serializable]
    public class scDouble
    {
        public string name;
        public double value;

        public scDouble(string x, double y)
        {
            name = x;
            value = y;
        }
    }

    [Serializable]
    public class scFloat
    {
        public string name;
        public float value;

        public scFloat(string x, float y)
        {
            name = x;
            value = y;
        }
    }

    [Serializable]
    public class scInt
    {
        public string name;
        public int value;

        public scInt(string x, int y)
        {
            name = x;
            value = y;
        }
    }

    [Serializable]
    public class scLong
    {
        public string name;
        public long value;

        public scLong(string x, long y)
        {
            name = x;
            value = y;
        }
    }

    [Serializable]
    public class scString
    {
        public string name;
        public string value;

        public scString(string x, string y)
        {
            name = x;
            value = y;
        }
    }
}
