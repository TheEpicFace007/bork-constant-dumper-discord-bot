using RestSharp;

namespace auto_server_side_fuck
{
    public class ConstantDumper
    {
        public string ScriptToConstantDump { get; set; }
        public string Constants { get; set; }

        public ConstantDumper(string scriptToConstantDump)
        {
            ScriptToConstantDump = scriptToConstantDump;
        }

        public ConstantDumper()
        {
        }

        public string DumpConstants()
        {
            var client = new RestClient("http://borks.club:2095/dumper");
            var request = new RestRequest(RestSharp.Method.POST);
            request.AddParameter("code",ScriptToConstantDump);
            var response = client.Execute(request);
            Constants = response.Content;
            return response.Content;
        }
    }
}