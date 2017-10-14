using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Net;
using System.IO;

public partial class StoredProcedures

    {

        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void WSPut(SqlString weburl, out SqlString returnval)

        {

            string url = Convert.ToString(weburl);

            string feedData = string.Empty;

            try

            {

                HttpWebRequest request = null;

                HttpWebResponse response = null;

                Stream stream = null;

                StreamReader streamReader = null;



                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "PUT"; // you have to change to

                //PUT/POST/GET/DELETE based on your scenerio…

                request.ContentLength = 0;

                response = (HttpWebResponse)request.GetResponse();

                stream = response.GetResponseStream();

                streamReader = new StreamReader(stream);

                feedData = streamReader.ReadToEnd();



                response.Close();

                stream.Dispose();

                streamReader.Dispose();



            }



            catch (Exception ex)

            {

                SqlContext.Pipe.Send(ex.Message.ToString());

            }

            returnval = feedData;

        }



        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void WSPost(SqlString weburl, SqlString json, out SqlString returnval)
        {
        returnval = string.Empty;
        string url = Convert.ToString(weburl);
            string feedData = string.Empty;
            try
            {
                HttpWebRequest request = null; 

                NetworkCredential nc = new NetworkCredential("r.cruz", "P@$$w0rd");

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Credentials = nc;
                request.Method = "POST";
            request.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
               // string json = "{\n\"comment\":\"this is just a random comment 555...\",\n\"id\":\"3421\",\n\"siemscore\":\"100\",\n\"threatscore\":\"100\",\n\"source\":\"this is a another souce\",\n\"target\":\"richard-wks-002\"\n}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
          
           var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                returnval = streamReader.ReadToEnd();
            }
             
        }
        
        catch (Exception ex)
            {
                SqlContext.Pipe.Send(ex.Message.ToString());
            }
        
        }
    
    };

