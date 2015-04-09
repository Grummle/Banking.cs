using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfxSharpLib;
using RestSharp;

namespace Banking.cs
{
    public class Banking
    {
        public Ofx.Options Options { get; set; }

        public Banking(Ofx.Options options)
        {
            Options = options;
        }

        public OfxDocument GetStatement()
        {
            var fullUrl = new Uri(Options.Url);
            var baseUrl = string.Format("https://{0}",fullUrl.Host);
            var rightUrl = fullUrl.PathAndQuery;
            var client = new RestClient(baseUrl);
            var request = new RestRequest(rightUrl, Method.POST);
            request.AddParameter("application/x-ofx", Ofx.CreateRequest(Options),ParameterType.RequestBody);
            request.AddHeader("Accept", "application/ofx");
            var response = client.Execute(request);
            var parser = new OfxDocumentParser();
            return parser.Import(response.Content);
        }
    }
}
