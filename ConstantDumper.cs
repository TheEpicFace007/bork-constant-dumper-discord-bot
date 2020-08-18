namespace auto_server_side_fuck
{
    public class ConstantDumper
    {
        public string ScriptToConstantDump { get; set; }
        public string Constants { get; }

        public ConstantDumper(string scriptToConstantDump)
        {
            ScriptToConstantDump = scriptToConstantDump;
        }

        public string DumpConstants()
        {
            
        }
    }
}