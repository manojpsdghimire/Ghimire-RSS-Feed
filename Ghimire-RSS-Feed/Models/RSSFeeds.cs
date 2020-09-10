using System;
using System.ComponentModel.DataAnnotations;

namespace Ghimire_RSS_Feed.Models
{

    //enumtype for Feed Types
    public enum FeedType
    {
        Technology = 1,
        Design = 2,
        Culture = 3,
        Business = 4,
        Politics = 5,
        Opinion = 6,
        Science = 7,
        Health = 8,
        Style= 9,
        Travel = 10
    }

    public class RSSFeeds
    {

        [Key]
        public Guid RSSFeedsId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true)]

        public DateTime PublishedDate { get; set; }
        public Byte[] Image { get; set; }

        [Required]
        public FeedType FeedType { get; set; }

        public string Feedurl { get; set; }
        public string Id { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}