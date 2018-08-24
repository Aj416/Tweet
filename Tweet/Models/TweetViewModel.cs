using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tweet.Models
{
    public class TweetViewModel
    {
        // Id property
        public string Id { get; set; }
        // Stamp property
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Stamp { get; set; }
        // Text property
        public string Text { get; set; }
    }
}