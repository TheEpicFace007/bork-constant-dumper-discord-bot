using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<string> DumpConstantsAsync()
        {
            var client = new RestClient("http://borks.club:2095/dumper");
            var request = new RestRequest(RestSharp.Method.POST);
            request.AddParameter("code",ScriptToConstantDump);
            var response = await client.ExecuteAsync(request);
            Constants = response.Content;
            return response.Content;
        }

        public void SetFileToConstantDump(FileStream file)
        {
            string contentOfFile = "";
            using (var reader = new StreamReader(file))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    contentOfFile.Append<>(line);
                }
            }
        }
    }
}