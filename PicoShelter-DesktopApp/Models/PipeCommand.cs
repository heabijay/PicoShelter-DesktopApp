using System;
using System.Collections.Generic;
using System.Text;

namespace PicoShelter_DesktopApp.Models
{
    public class PipeCommand
    {
        public enum CommandType
        {
            Exit,
            OpenSecondInstance
        }

        public CommandType Type { get; set; }
        public string[] CommandLineArgs { get; set; }
    }
}
