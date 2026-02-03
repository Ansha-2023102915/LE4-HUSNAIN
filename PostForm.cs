using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BlogDataLibrary
{
    public class PostForm
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }
    }
}