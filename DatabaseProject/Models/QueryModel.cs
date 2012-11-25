using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class TopicModel
    {
        public List<string> topics { get; set; }
    }

    public class PostModel
    {
        public int cid { get; set; }
        public int ctype { get; set; }
        public string ctext { get; set; }
        public int pid { get; set; }
        public string fname { get; set; }
        public string ptime { get; set; }
        public double avg_rtg { get; set; }
        public List<string> post_Topics { get; set; }
    }

    public class PostListModel
    {
        public List<PostModel> posts { get; set; }
        public int total_posts { get; set; }
    }

    public class NewPostModel
    {
        [Required]
        public string ctext { get; set; }
        
        [Required]
        public int pid { get; set; }    //Needed from the session variable

        [Required]
        [Display(Name = "Make Private?")]   //Option to public/private
        public int ctype;

        public string ptime { get; set; }
        public List<string> topic { get; set; } //Optional topic
    }
}