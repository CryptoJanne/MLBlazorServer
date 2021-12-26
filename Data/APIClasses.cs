namespace APIHandler
{
    public class CPU 
    {
        public float core1 { get; set; }
        public float core2 { get; set; }
        public float core3 { get; set; }
        public float core4 { get; set; }
        public float core5 { get; set; }
        public float core6 { get; set; }
        public float core7 { get; set; }
        public float core8 { get; set; }
    }
    public class GPU
    {
        public string name { get; set; }
        public float fanspeed { get; set; }
        public float memoryutilization { get; set; }
        public float gpuutilization { get; set; }
        public string gputemperature { get; set; }
        public float poweruse { get; set; }
    }
    public class Memory 
    {
        public string totalmemory { get; set; }
        public string availablememory { get; set; }
        public float percentused { get; set; }
    }
    public class HardDrive
    {
        public string totalspace { get; set; }
        public string used { get; set; }
        public string free { get; set; }
        public float percent { get; set; }
    }
    public class SystemStats
    {
        public DateTime tid { get; set; }
        public CPU cpu { get; set; }
        public GPU gpu { get; set; }
        public Memory memory { get; set; }
        public HardDrive harddrive1 { get; set; }
        public HardDrive harddrive2 { get; set; }
    }
}