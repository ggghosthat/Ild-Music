using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SynchronizationBlock.Models.SynchArea
{
    public class SerializeService
    {
        private string stuff = "";
        private string pathway = "";
        private string output_raw = "";

        public string Stuff => stuff;

        private static StreamReader reader;
        private static StreamWriter writer;


        public void SetDetails(string pathway, string inputStuff, bool? isSaveOrInit = null) 
        {
            this.pathway = pathway;
            stuff = inputStuff;

            CleanUp();

            if (isSaveOrInit == true)
                Save();
            else if(isSaveOrInit == false)
                Initialize(out this.output_raw);
        }

        private void Initialize(out string? output_stuff) 
        {
            if (File.Exists(this.pathway))
            {
                using (reader = new StreamReader(new FileStream(this.pathway, FileMode.Open), Encoding.UTF8))
                {
                    output_stuff = reader.ReadToEnd();
                    return;
                }
            }
            output_stuff = null;
        }
        
        private void Save() 
        {
            using (writer = new StreamWriter(new FileStream(this.pathway, FileMode.OpenOrCreate), Encoding.UTF8))
            {
                writer.Write(stuff);
            }
        }

        private void CleanUp()
        {
            File.WriteAllText(this.pathway, string.Empty);
        }
    }
}
