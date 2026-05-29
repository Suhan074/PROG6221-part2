namespace CybersecurityChatbotWPF.Models
{
    public class UserProfile
    {
        public string Name { get; set; } = string.Empty;
        public string Interest { get; set; } = string.Empty;
        public int InteractionCount { get; set; }
        public DateTime FirstInteraction { get; set; } = DateTime.Now;
    }
}