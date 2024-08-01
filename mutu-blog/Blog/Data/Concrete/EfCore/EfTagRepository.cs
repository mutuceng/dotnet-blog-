using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Abstract;
using Blog.Entity;

namespace Blog.Data.Concrete.EfCore
{
    public class EfTagRepository:ITagRepository
    {
        private BlogContext _context;
        public EfTagRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Tag> Tags => _context.Tags;

        public void CreateTag(Tag Tag)
        {
            _context.Tags.Add(Tag);
            _context.SaveChanges();
        }

        public void DeleteTag(Tag tag)
        {
                
        }
    }
}