
using System.ComponentModel.DataAnnotations;

namespace SignalRexamples.Models
{
    public class ChatRoom
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
