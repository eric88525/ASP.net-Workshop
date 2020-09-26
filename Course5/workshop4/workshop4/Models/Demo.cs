using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace workshop4.Models
{
    public class Demo
    {

        public Demo(string id, string name)
        {
            Id = id;
            Name = name;
        }


        public string Id { get; set; }
        public string Name { get; set; }

    }
}