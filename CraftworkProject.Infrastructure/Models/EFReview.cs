using System;
using System.ComponentModel.DataAnnotations;
using CraftworkProject.Domain.Models;

namespace CraftworkProject.Infrastructure
{
    internal class EFReview
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime PublicationDate { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}