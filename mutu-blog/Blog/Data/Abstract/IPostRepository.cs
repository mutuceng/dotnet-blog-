using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entity;

namespace Blog.Data.Abstract
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts {get;}
        void CreatePost(Post post);
        void EditPost(Post post);
        void EditPost(Post post, int[] tagIds);
        void DeletePost(Post post);
    }
}