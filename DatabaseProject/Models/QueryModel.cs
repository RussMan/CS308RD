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
        [Display(Name = "Body")]
        public string ctext { get; set; }

        [Required]
        [Display(Name = "Members Only?")]   //Option to public/private
        public bool ctype { get; set; }

        [Required]
        public int pid { get; set; }

        [Display(Name = "Topic")]
        public string topic { get; set; }   //Optional topic
    }
}