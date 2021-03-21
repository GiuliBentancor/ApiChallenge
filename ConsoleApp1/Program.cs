using System;
using System.Net;
using NUnit.Framework;
using FluentAssertions;
using RestSharp;
using Newtonsoft.Json;
using RestSharp.Authenticators;

namespace ConsoleApp1
{
    public class Program
    {

        static string challenger;
        static RestClient client;
        static int IdExistingPost;
        static string xauth;


   
        public static void Main(string[] args)
        {
            //Getting Started:
            client = new RestClient("http://apichallenges.herokuapp.com/");
            challenger = SetChallenger();
            IdExistingPost = SetIdExistingPost();


            //First Real Challenge:
            GetChallenges200();
            
            //GET Challenges:
            GetTodos200();
            GetTodo404();
            GetTodosId200();
            GetTodosId404();

            //HEAD Challenges:
            GetHeader();

            //Creation Challenges with POST:
            PostTodos201();
            GetTodosFilter200();
            //PostTodos400();

            //Update Challenges with POST:
            PostTodosId200();

            //DELETE Challenges:
            DeleteId200();

            //OPTIONS Challenges:
            Options200();

            //Accept Challenges:
            GetWithHeaderXML200();
            GetWithHeaderJSON200();
            GetWithHeaderANY200();
            GetWithHeaderXMLPref200();
            GetWithNoAcceptHeader200();
            GetWithAcceptHeader404();

            //Content - Type Challenges:
            PostXMLBody201();
            PostUnsupportedContent415();

            //Mix Accept and Content-Type Challenges:
            PostXMLtoJSON201();
            PostJSONtoXML201();

            //Status Code Challenges:
            Delete405();
            Patch500();
            Get204();
            //Trace501();

            //Authentication Challenges:
            PostAuth201();
            PostAuth401();

            //Authorization Challenges:
            Get403();
            Get401();
            GetNote200();
            PostNote200();
            PostNote401();
            PostNote403();
            //GetNoteWithBearer200();
            //PostNoteWithBearer200();

            //Miscellaneous Challenges:

        } 

        public static string SetChallenger()
        {

            var request = new RestRequest("/challenger", DataFormat.Json);

            var response = client.Post(request);

            string idChallenger = response.Headers[2].ToString().Substring(13);

            Console.WriteLine(idChallenger);

            return idChallenger;
        }

        public static int SetIdExistingPost()
        {
            var request = new RestRequest("/todos");

            var response = client.Get(request);

            var obj = JsonConvert.DeserializeObject<dynamic>(response.Content);

            int IdExistingPost = obj.todos[0].id;

            return IdExistingPost;


        }

        public static string SetXauth()
        {
            var request = new RestRequest("/secret/token", Method.POST);
            request.AddHeader("X-Challenger", challenger);
            client.Authenticator = new HttpBasicAuthenticator("admin", "password");


            var response = client.Execute(request);
            xauth = response.Headers[2].ToString().Substring(13);

            Console.WriteLine(xauth);

            return xauth;

        }


        public static void GetChallenges200()
        {

            var request = new RestRequest("/challenges", Method.GET);
            request.AddHeader("X-Challenger", challenger);

            var response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        public static void GetTodos200()
        {

            var request = new RestRequest("/todos", Method.GET);
            request.AddHeader("X-Challenger", challenger);

            var response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }


        public static void GetTodo404()
        {
            var request = new RestRequest("/todo", Method.GET);
            request.AddHeader("X-Challenger", challenger);

            var response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        public static void GetTodosId200()
        {
            int id = IdExistingPost;
            var request = new RestRequest($"/todos/{id}", Method.GET);
            request.AddHeader("X-Challenger", challenger);

            var response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }


        public static void GetTodosId404()
        {
            int id = 1;
            var request = new RestRequest($"/todos/{id}", Method.GET);
            request.AddHeader("X-Challenger", challenger);

            var response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        public static void GetHeader()
        {
            var request = new RestRequest("/todos", Method.HEAD);
            request.AddHeader("X-Challenger", challenger);

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public static void PostTodos201()
        {
            var request = new RestRequest("/todos", Method.POST);
            request.AddHeader("X-Challenger", challenger);
            request.AddJsonBody(new Todo()
            {

                title = "10",
                doneStatus = true,
                description = "lala"

            });

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        public static void GetTodosFilter200()
        {
            var request = new RestRequest("/todos", Method.GET);
            request.AddHeader("X-Challenger", challenger);
            request.AddParameter("doneStatus", "true");

            var response = client.Execute(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }


        public static void PostTodos400()
        {
            var request = new RestRequest("/todos", Method.POST);
            request.AddHeader("X-Challenger", challenger);

            request.AddJsonBody(new Todo()
            {

                title = "10",
                description = "lala",

            });

        }

        public static void PostTodosId200()
        {
            var id = IdExistingPost;
            var request = new RestRequest($"/todos/{id}", Method.POST);
            request.AddHeader("X-Challenger", challenger);

            request.AddJsonBody(new Todo()
            {

                title = "updated",

            });

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public static void DeleteId200()
        {
            var id = IdExistingPost;
            var request = new RestRequest($"/todos/{id}", Method.DELETE);
            request.AddHeader("X-Challenger", challenger);

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);


        }

        public static void Options200()
        {

            var request = new RestRequest("/todos", Method.OPTIONS);
            request.AddHeader("X-Challenger", challenger);

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        public static void GetWithHeaderXML200()
        {

            var request = new RestRequest("/todos", Method.GET);
            request.AddHeader("X-Challenger", challenger);
            request.AddHeader("Accept", "application/xml");

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        public static void GetWithHeaderJSON200()
        {

            var request = new RestRequest("/todos", Method.GET);
            request.AddHeader("X-Challenger", challenger);
            request.AddHeader("Accept", "application/json");

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        public static void GetWithHeaderANY200()
        {

            var request = new RestRequest("/todos", Method.GET);
            request.AddHeader("X-Challenger", challenger);
            request.AddHeader("Accept", "*/*");

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        public static void GetWithHeaderXMLPref200()
        {

            var request = new RestRequest("/todos", Method.GET);
            request.AddHeader("X-Challenger", challenger);
            request.AddHeader("Accept", "application/xml, application/json");

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        public static void GetWithNoAcceptHeader200()
        {

            var request = new RestRequest("/todos", Method.GET);
            request.AddHeader("X-Challenger", challenger);

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        public static void GetWithAcceptHeader404()
        {

            var request = new RestRequest("/todos", Method.GET);
            request.AddHeader("X-Challenger", challenger);
            request.AddHeader("Accept", "application/gzip");

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);

        }


        public static void PostXMLBody201()
        {
            var request = new RestRequest("/todos", Method.POST);
            request.AddHeader("X-Challenger", challenger);

            request.RequestFormat = DataFormat.Xml;
            request.AddHeader("Accept", "application/xml");
            request.AddHeader("Content-Type", "application/xml");

            string xmlBody = "<todo> < doneStatus > true </ doneStatus >< description > XML </ description >< title > XMLBODY </ title ></ todo > " ;

            request.AddParameter("application/xml", xmlBody, ParameterType.RequestBody);

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

        }

        public static void PostUnsupportedContent415()
        {
            var request = new RestRequest("/todos", Method.POST);
            request.AddHeader("X-Challenger", challenger);
            request.AddHeader("Content-Type", "NANA");

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);

        }

        public static void PostXMLtoJSON201()
        {
            var request = new RestRequest("/todos", Method.POST);
            request.AddHeader("X-Challenger", challenger);

            request.RequestFormat = DataFormat.Xml;
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/xml");

            string xmlBody = "<todo> < doneStatus > true </ doneStatus >< description > XML </ description >< title > XMLBODY </ title ></ todo > ";

            request.AddParameter("application/xml", xmlBody, ParameterType.RequestBody);

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

        }
       
        public static void PostJSONtoXML201()
        {
            var request = new RestRequest("/todos", Method.POST);
            request.AddHeader("X-Challenger", challenger);

            request.AddHeader("Accept", "application/xml");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new Todo()
            {

                title = "JSON",
                description = "JSON",

            });

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

        }

        public static void Get204()
        {

            var request = new RestRequest("/heartbeat", Method.GET);
            request.AddHeader("X-Challenger", challenger);

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        }

        public static void Delete405()
        {

            var request = new RestRequest("/heartbeat", Method.DELETE);
            request.AddHeader("X-Challenger", challenger);

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);

        }

        public static void Patch500()
        {

            var request = new RestRequest("/heartbeat", Method.PATCH);
            request.AddHeader("X-Challenger", challenger);

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        }

        public static void Trace501()
        {

            var request = new RestRequest("/heartbeat", Method.GET);
            request.AddHeader("X-Challenger", challenger);

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.NotImplemented);

        }


        public static void PostAuth201()
        {
            var request = new RestRequest("/secret/token", Method.POST);
            
            client.Authenticator = new HttpBasicAuthenticator("admin", "password");


            request.AddHeader("X-Challenger", challenger);

           
            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
           

        }

        public static void PostAuth401()
        {
            var request = new RestRequest("/secret/token", Method.POST);
            client.Authenticator = new HttpBasicAuthenticator("giuli", "password");
            request.AddHeader("X-Challenger", challenger);


            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }


        public static void Get403()
        {
            var request = new RestRequest("/secret/note", Method.GET);
            request.AddHeader("X-Challenger", challenger);
            request.AddHeader("X-AUTH-TOKEN", challenger);

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        }

        public static void Get401()
        {
            var request = new RestRequest("/secret/note", Method.GET);
            request.AddHeader("X-Challenger", challenger);
         
            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }


        public static void GetNote200()
        {
            xauth = SetXauth();

            var request = new RestRequest("/secret/note", Method.GET);
            request.AddHeader("X-Challenger", challenger);
            request.AddHeader("X-AUTH-TOKEN", xauth);

            var response = client.Execute(request);

            response.Content.Should().Contain("note");

        }

        public static void PostNote200()
        {

            var request = new RestRequest("/secret/note", Method.POST);
            request.AddHeader("X-Challenger", challenger);
            request.AddHeader("X-AUTH-TOKEN", xauth);

            request.AddJsonBody(new Note()
            {
                note = "my note"
            });

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);


        }

        public static void PostNote401()
        {

            var request = new RestRequest("/secret/note", Method.POST);
            request.AddHeader("X-Challenger", challenger);
         

            request.AddJsonBody(new Note()
            {
                note = "my note"
            });

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);


        }

        public static void PostNote403()
        {

            var request = new RestRequest("/secret/note", Method.POST);
            request.AddHeader("X-Challenger", challenger);
            request.AddHeader("X-AUTH-TOKEN", "XAUTH");

            request.AddJsonBody(new Note()
            {
                note = "my note"
            });

            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);


        }

        public static void GetNoteWithBearer200()
        {
            xauth = SetXauth();
            var bearer = $"Bearer + {xauth}";

            var request = new RestRequest("/secret/note", Method.GET);
     
            request.AddHeader("X-Challenger", challenger);
            request.AddHeader("Authorization", bearer);
     
            var response = client.Execute(request);
           

            Console.WriteLine(response.StatusCode);
            response.StatusCode.Should().Be(HttpStatusCode.OK);



        }

        public static void PostNoteWithBearer200()
        {
            xauth = SetXauth();
            var request = new RestRequest("/secret/note", Method.POST);
            request.AddHeader("X-Challenger", challenger);
            request.AddHeader("Authorization", $"Bearer + {xauth}");

            request.AddJsonBody(new Note()
            {
                note = "my note"
            });

            var response = client.Execute(request);

            Console.WriteLine(response.StatusCode);
            response.StatusCode.Should().Be(HttpStatusCode.OK);


        }




    }
}
