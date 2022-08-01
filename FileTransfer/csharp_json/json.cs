using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_json
{
    class Json
    {
        public char[] buf;
        StreamReader reader;
        int bufferSize = 1024 * 1024 * 4;
        int len = 0;
        public bool openFile(string path)
        {

            if (File.Exists(path))
            {
                // using(Stream s=new (path) )
                //File f =  File.(path);
                FileInfo fileInfo =new FileInfo(path);
                reader = new StreamReader(path);
                buf = new char[bufferSize];
       

                len= reader.Read(buf,0, bufferSize);
                //reader.ReadBlock(buf, 0);
                return true;

            }
            else
                throw new Exception("null file");

        }

        public void readFromBuf(byte[] buf,int offset)
        {
           this.buf= Encoding.Default.GetChars(buf, offset, buf.Length-offset);
        }

        public List<OneToken> tokens;
        public void tokenize()
        {
            tokens = new List<OneToken> { };

            int i = 0;
            for (; ; )
            {
                skipWhite(ref i);
                if (i < len)
                {
                    //if (tokens.Count > 0)
                    //    Console.WriteLine("add   "+tokens.Last().value);
                    if (buf[i] == '[')
                    {
                        i++;
                        skipWhite(ref i);
                        if (buf[i] != '{')
                        {
                            tokens.Add(new OneToken(Token.VALUE_ARRAY, readValueArray(ref i)));
                            i++;
                        }
                        else
                        {
                            tokens.Add(new OneToken(Token.ARRAY_BEGIN, "["));
                            tokens.Add(new OneToken(Token.OBJECT_BEGIN, "{"));
                            i++;
                        }

                        continue;
                    }
                    else if (buf[i] == ']')
                    {
                        i++;
                        tokens.Add(new OneToken(Token.ARRAY_END, "]"));
                        continue;
                    }
                    else if (buf[i] == '{')
                    {
                        //if (tokens.Count() > 0 && tokens.Last().token == Token.VALUE_ARRAY)
                        //{
                        //    tokens.Last().token = Token.ARRAY_BEGIN;
                        // //  tokens.ElementAt(tokens.Count()-1).token=Token.
                        //} 暂时不需要修正
                        tokens.Add(new OneToken(Token.OBJECT_BEGIN, "{"));
                        i++;
                        continue;
                    }
                    else if (buf[i] == '}')
                    {
                        tokens.Add(new OneToken(Token.OBJECT_END, "}"));
                        i++;
                        continue;
                    }
                    else if (buf[i] >= '0' && buf[i] <= '9')
                    {
                        bool intflag;
                        string res = readNumber(ref i, buf[i], out intflag);
                        if (intflag)
                        {

                            tokens.Add(new OneToken(Token.INT, int.Parse(res)));
                        }
                        else
                        {
                            tokens.Add(new OneToken(Token.DOUBLE, double.Parse(res)));
                        }


                        continue;
                    }
                    else if (buf[i] == '"')
                    {
                        tokens.Add(new OneToken(Token.VALUE_STRING, readString(ref i)));
                        i++;
                        continue;
                    }
                    else if (buf[i] == ':')
                    {
                        if (tokens.Last().token == Token.VALUE_STRING)
                            tokens.Last().token = Token.KEY_STRING;

                        tokens.Add(new OneToken(Token.COLON, ":"));
                        i++;
                        continue;
                    }
                    else if (buf[i] == ',')
                    {
                        tokens.Add(new OneToken(Token.COMMA, ","));
                        i++;
                        continue;
                    }

                    else if (buf[i] == 0)
                    {
                        tokens.Add(new OneToken(Token.END, "\0"));
                        return;

                    }
                    else
                    {
                        throw new Exception("unexpect '" + buf[i] + "'");
                    }
                }
                else
                {
                    tokens.Add(new OneToken(Token.END, '0'));
                    return;
                }

            }

        }
        void skipWhite(ref int i)
        {
            while (i < len && (buf[i] == ' '
                || buf[i] == '\t'
                || buf[i] == '\b'
                || buf[i] == '\n'
                || buf[i] == '\r'))
            {

                i++;

            }

        }
        string readNumber(ref int i, char c, out bool intFlag)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(c);
            i++;
            while ((i < len && buf[i] >= '0' && buf[i] <= '9'))
            {
                builder.Append(buf[i]);
                i++;
            }
            if (buf[i] == '.')
            {
                intFlag = false;
                builder.Append(buf[i]);
                i++;
                while ((i < len && buf[i] >= '0' && buf[i] <= '9'))
                {
                    builder.Append(buf[i]);
                    i++;
                }

            }
            else
                intFlag = true;

            return builder.ToString();

        }
        string readString(ref int i)
        {
            i++;
            StringBuilder builder = new StringBuilder(buf[i]);

            while (i < len && buf[i] != '"')
            {
                builder.Append(buf[i]);
                i++;
            }
            if (i >= len)
            {
                throw new Exception("lack '\"' ");
            }
            return builder.ToString();
        }
        object readValueArray(ref int i)
        {
            skipWhite(ref i);

            if (buf[i] >= '0' && buf[i] <= '9')
            {
                List<object> vs = new List<object> { };
                do
                {
                    bool intflag;
                    string res = readNumber(ref i, buf[i], out intflag);
                    
                    if (intflag)
                    {
                        vs.Add(int.Parse(res));
                    }
                    else
                    {
                        vs.Add(double.Parse(res));
                    }

                } while (nextIsOk(',', ref i) && buf[i] >= '0' && buf[i] <= '9');
                if (i >= len)
                {
                    throw new Exception("lack  ']'  ");
                }
                if (buf[i] == ']')
                    return vs.ToArray();

            }
            else if (buf[i] == '"')
            {
                List<string> vs = new List<string> { };
                do
                {
                    vs.Add(readString(ref i));
                    i++;
                } while (nextIsOk(',', ref i) && buf[i] == '"');
                if (buf[i] == ']')
                    return vs.ToArray();
            }
            throw new Exception("unexpect ->'" + buf[i] + "'  ");
        }
        bool nextIsOk(char expext, ref int i)
        {
            skipWhite(ref i);
            if (i < len && buf[i] == expext)
            {
                i++;
                skipWhite(ref i);
                return true;
            }
            return false;
        }


        public object parse()
        {
            tokenize();
            int j = 0;
            switch (tokens.ElementAt(j).token)
            {
                case Token.ARRAY_BEGIN:
                    j++;
                    JsonArray jsonArray = parseJsonArray(ref j);
                    if (tokens.ElementAt(j).token == Token.END)
                        return jsonArray;
                    else break;
                case Token.OBJECT_BEGIN:
                    {
                        j++;
                        JsonObject jsonObject = parseJsonObject(ref j);
                        if (tokens.ElementAt(j).token == Token.END)
                            return jsonObject;
                        else break;
                    }

                default:
                    throw new Exception("unexpect ->'" + tokens.ElementAt(j).value + "' ");

            }
            throw new Exception("unexpect ->'" + tokens.ElementAt(j).value + "' ");
        }
        JsonArray parseJsonArray(ref int j)
        {
            JsonArray jsonObjects = new JsonArray();
            for (; ; )
            {
                if (tokens.ElementAt(j).token == Token.OBJECT_BEGIN)
                {
                    j++;
                    jsonObjects.put(parseJsonObject(ref j));
                }
                if (tokens.ElementAt(j).token != Token.COMMA)
                {
                    break;
                }
                else
                    j++;

            }
            if (tokens.ElementAt(j).token == Token.ARRAY_END)
            {
                j++;
                return jsonObjects;
            }
            throw new Exception("unexpect ->'" + tokens.ElementAt(j).value + "' ");

        }

        JsonObject parseJsonObject(ref int j)
        {
            JsonObject jsonObject = new JsonObject();
            string key = null;

            while (j < tokens.Count)
            {
                OneToken currentToken = tokens.ElementAt(j);
                switch (currentToken.token)
                {
                    case Token.KEY_STRING:

                        nextTokenIsOk(Token.KEY_STRING, tokens.ElementAt(j + 1));
                        key = (string)currentToken.value;
                        j++;
                        break;
                    case Token.INT:
                        nextTokenIsOk(Token.INT, tokens.ElementAt(j + 1));
                        jsonObject.put(key, currentToken.value);
                        j++;
                        break;
                    case Token.DOUBLE:
                        nextTokenIsOk(Token.DOUBLE, tokens.ElementAt(j + 1));
                        jsonObject.put(key, currentToken.value);
                        j++;
                        break;
                    case Token.VALUE_ARRAY:
                        nextTokenIsOk(Token.VALUE_ARRAY, tokens.ElementAt(j + 1));

                        jsonObject.put(key, currentToken.value);
                        j++;
                        break;
                    case Token.COLON:
                        nextTokenIsOk(Token.COLON, tokens.ElementAt(j + 1));
                        j++;
                        break;

                    case Token.VALUE_STRING:
                        if (tokens.ElementAt(j + 1).token == Token.COLON)
                        {
                            tokens.ElementAt(j).token = Token.KEY_STRING;
                            break;
                        }

                        nextTokenIsOk(Token.VALUE_STRING, tokens.ElementAt(j + 1));

                        jsonObject.put(key, currentToken.value);
                        j++;
                        break;

                    case Token.COMMA:
                        nextTokenIsOk(Token.COMMA, tokens.ElementAt(j + 1));
                        j++;
                        break;
                    case Token.OBJECT_BEGIN:
                        j++;
                        jsonObject.put(key, parseJsonObject(ref j));
                        break;
                    case Token.OBJECT_END:
                        j++;
                        return jsonObject;
                    case Token.ARRAY_BEGIN:
                        nextTokenIsOk(Token.ARRAY_BEGIN, tokens.ElementAt(j + 1));
                        j++;
                        jsonObject.put(key, parseJsonArray(ref j));
                        break;
                    default:
                        throw new Exception("unexpect ->'" + tokens.ElementAt(j).value + "' ");

                }

            }
            if (tokens.ElementAt(j).token == Token.END)
                return jsonObject;
            throw new Exception("lack }");
        }

        void nextTokenIsOk(Token currentToken, OneToken nextToken)
        {
            bool flag = false;
            if (currentToken == Token.KEY_STRING)
                flag = nextToken.token == Token.COLON;
            else if (currentToken == Token.COLON)//:
            {
                flag = (nextToken.token == Token.VALUE_ARRAY
                    || nextToken.token == Token.INT
                    || nextToken.token == Token.DOUBLE
                    || nextToken.token == Token.VALUE_STRING
                    || nextToken.token == Token.OBJECT_BEGIN
                    || nextToken.token == Token.ARRAY_BEGIN);

            }
            else if (currentToken == Token.OBJECT_END
                 || currentToken == Token.VALUE_STRING
                 || currentToken == Token.VALUE_ARRAY
                 || currentToken == Token.INT
                 || currentToken == Token.DOUBLE)
                flag = nextToken.token == Token.COMMA || nextToken.token == Token.OBJECT_END || nextToken.token == Token.ARRAY_END;

            else if (currentToken == Token.COMMA)
                flag = nextToken.token == Token.KEY_STRING;
            else if (currentToken == Token.KEY_STRING)
                flag = nextToken.token == Token.COLON;
            else if (currentToken == Token.ARRAY_BEGIN)
                flag = nextToken.token == Token.OBJECT_BEGIN;
            if (!flag)
                throw new Exception("unexpect token->'" + nextToken.value + "'  ");

        }


    }
    enum Token
    {
        ARRAY_BEGIN,
        ARRAY_END,
        OBJECT_BEGIN,
        OBJECT_END,
        COMMA,
        COLON,
        INT,
        DOUBLE,
        KEY_STRING,
        VALUE_STRING,
        END,
        VALUE_ARRAY
    }
    class OneToken
    {
        public Token token;
        public object value;
        public OneToken(Token token, Object value)
        {
            this.token = token;
            this.value = value;
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("" + token + " [");

            if (token == Token.VALUE_ARRAY)
            {
                foreach (var i in (object[])value)
                {
                    if (i is string)
                        builder.Append('"' + i.ToString() + "\",");
                    else
                        builder.Append(i.ToString() + ",");
                }
                builder.Append("\b]");
                return builder.ToString();
            }

            return "" + token + "\t" + value.ToString();
        }
    }

    class JsonArray
    {
        private List<JsonObject> jsons = new List<JsonObject> { };
        public void put(JsonObject json)
        {
            jsons.Add(json);
        }
        public JsonObject get(int index)
        {
            return jsons.ElementAt(index);
        }

    }

    class JsonObject
    {
        Dictionary<string, object> dir = new Dictionary<string, object> { };

        public object getValue(string key)
        {
            dir.TryGetValue(key, out object obj);
            return obj;
        }
        public JsonArray getJsonArray(string key)
        {
            dir.TryGetValue(key, out object obj);
            return (JsonArray)obj;
        }
        public JsonObject getJsonObject(string key)
        {
            dir.TryGetValue(key, out object obj);
            return (JsonObject)obj;

        }
        public int getInt(string key)
        {
            dir.TryGetValue(key, out object obj);
            return (int)obj;
        }
        public double getDouble(string key)
        {
            dir.TryGetValue(key, out object obj);
            return (double)obj;
        }
        public string[] getStringList(string key)
        {
            dir.TryGetValue(key, out object obj);
            return (string[])obj;
        }
        public object[] getNumberList(string key)
        {
            dir.TryGetValue(key, out object obj);
            return (object[])obj;
        }
        public string getString(string key)
        {
            dir.TryGetValue(key, out object obj);
            return (string)obj;
        }
        public void put(string key, object value)
        {
            if (key == "")
                throw new Exception("key cannot be empty");
            dir.Add(key, value);
        }
    }
}
