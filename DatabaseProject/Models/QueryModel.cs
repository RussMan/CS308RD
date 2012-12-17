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

    public class RateModel
    {
        public int rate { get; set; }
    }

    public class PostListModel
    {
        public List<PostModel> posts { get; set; }
        public int total_posts { get; set; }
        public List<int> cid_list { get; set; }
        public RateModel rating { get; set; }
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

        public string ftype { get; set; }

        [Display(Name = "Topic")]
        public string topic { get; set; }   //Optional topic
    }

    public class NewPost_UI_Data
    {
        public NewPostModel new_post { get; set; }
        public List<string> friendships { get; set; }
    }

    public class SearchModel // Used for getting search entry from user to perform post look up by topic
    {
        public string searchTopic { get; set; }
    }

    public class PrivacyModel
    {
        public List<string> Groups { get; set; }
        public string group { get; set; }
        public List<string> Users { get; set; }
        public string user { get; set; }
    }
    //class PostEqualityComparer : IEqualityComparer<PostModel> // For comparing posts to prevent duplicates
    //{
    //    public bool Equals(PostModel x, PostModel y)
    //    {
    //        if (x.cid == y.cid) return true;
    //        else return false;
    //    }

    //    public int GetHashCode(PostModel post)
    //    {
    //        int hashCode = post.cid ^ post.pid;
    //        return hashCode.GetHashCode();
    //    }
    //}

    //class CIDEqualityComparer : IEqualityComparer<int> // For comparing CIDs in the CID List [prevents some enumeration exception]
    //{
    //    public bool Equals(int x, int y)
    //    {
    //        if (x == y) return true;
    //        else return false;
    //    }

    //    public int GetHashCode(int x)
    //    {
    //        return x.GetHashCode();
    //    }
    //}
}