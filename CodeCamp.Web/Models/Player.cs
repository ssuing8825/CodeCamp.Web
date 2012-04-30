using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeCamp.Web.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
        public string SkillLevel { get; set; }
    }
}