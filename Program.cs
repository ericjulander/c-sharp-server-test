using System;
using System.Net;
using System.Text.RegularExpressions;
using GameManager;
using Newtonsoft.Json;
namespace Http_Testing
{
    class Program
    {

        public static HttpListener generateFromPrefixes(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                throw new Exception("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            return listener;
        }

        static void Main(string[] args)
        {
            Console.WriteLine();
            var listener = generateFromPrefixes(new[] { "http://localhost/", "http://10.5.188.48/" });
            listenAndDoStuff(listener, true);
        }

        public static void listenAndDoStuff(HttpListener listener, bool shouldDoStuff = true)
        {

            var game = new Game();
            listener.Start();
            Console.WriteLine("Listening...");
            var usernameRegex = new Regex(@"\b(?:(.*username\=))(.*)");
            // Note: The GetContext method blocks while waiting for a request. 
            System.Console.WriteLine("Waiting for request!");
            while (shouldDoStuff)
            {
                doThatThing(listener, usernameRegex, game);
            }

            listener.Stop();

        }


        public static string getUserName(HttpListenerRequest request, Regex usernameRegex){
            var match = usernameRegex.Match(request.RawUrl);
            var username = match.Groups[2].Value;
            return username;
        }
         static string setUpUser(HttpListenerRequest request, Regex usernameRegex, Game g)
        {
            var username = getUserName(request, usernameRegex);
            System.Console.WriteLine(username);
            if (!g.Characterz.ContainsKey(username))
            {
                g.AddBlobDude(new Character.BlobDude(username, 0, 0, 50, 50));
            }
            return username;
        }
         static string setUser(HttpListenerRequest request, Regex usernameRegex, Game g, string point)
        {
            var username = setUpUser(request,usernameRegex, g);
            g.updateParameters(JsonConvert.DeserializeObject<Character.Point>(point), username);
            return username;
        }

        public static void doThatThing(HttpListener listener, Regex usernameRegex, Game g)
        {
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            
            if (request.Url.Equals("http://10.5.188.48/"))
            {
                writeResponse(context, System.IO.File.ReadAllText("./test.html"));
                return;
            }
            if (request.Url.ToString().Contains("http://10.5.188.48/getusers"))
            {
                writeResponse(context, JsonConvert.SerializeObject(g.Characterz));
                return;
            }
            if (request.Url.ToString().Contains("http://10.5.188.48/getuser"))
            {
                var usernamez = setUpUser(request, usernameRegex, g);
                writeResponse(context, JsonConvert.SerializeObject(g.Characterz[usernamez]));
                return;
            }
            if (request.Url.ToString().Contains("http://10.5.188.48/setuser"))
            {
                var json = ShowRequestData(request);
                System.Console.WriteLine("JSON!!!! "+json);
                writeResponse(context, JsonConvert.SerializeObject(g.Characterz[setUser(request, usernameRegex, g, json)]));
                return;
            }
            var username = setUpUser(request, usernameRegex, g);
            ShowRequestData(request);
            writeResponse(context, JsonConvert.SerializeObject(g.Characterz[username]));
            //writeResponse(context, "yolo");
            // Obtain a response object.

        }

        public static void writeResponse(HttpListenerContext context, string responseString)
        {
            HttpListenerResponse response = context.Response;
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "POST, GET");
            // Construct a response.
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            response.KeepAlive = true;
            // You must close the output stream.
            output.Close();
        }
        public static string ShowRequestData(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                Console.WriteLine("No client data was sent with the request.");
                return "";
            }
            System.IO.Stream body = request.InputStream;
            System.Text.Encoding encoding = request.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
            if (request.ContentType != null)
            {
                Console.WriteLine("Client data content type {0}", request.ContentType);
            }
            Console.WriteLine("Client data content length {0}", request.ContentLength64);

            Console.WriteLine("Start of client data:");
            // Convert the data to a string and display it on the console.
            string s = reader.ReadToEnd();
            Console.WriteLine(s);
            Console.WriteLine("End of client data:");
            body.Close();
            reader.Close();
            if(s == null) s =""; 
            return s;
            // If you are finished with the request, it should be closed also.
        }

        // This example requires the System and System.Net namespaces.
        public static void SimpleListenerExample(string[] prefixes)
        {



        }
    }
}
