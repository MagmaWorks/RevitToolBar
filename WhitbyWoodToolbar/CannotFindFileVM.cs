using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhitbyWoodToolbar
{
    public class CannotFindFileVM
    {
        public bool SkipAll { get; set; } = false;
        public bool SkipOne { get; set; } = false;
        public string FileName { get; set; } = "placeholder";
    }
}
