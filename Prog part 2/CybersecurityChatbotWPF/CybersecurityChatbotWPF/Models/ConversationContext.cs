using System.Collections.Generic;

namespace CybersecurityChatbotWPF.Models
{
    public class ConversationContext
    {
        private readonly Dictionary<string, string> _memory;
        private readonly List<string> _topicHistory;
        
        public ConversationContext()
        {
            _memory = new Dictionary<string, string>();
            _topicHistory = new List<string>();
        }
        
        public void Remember(string key, string value)
        {
            if (_memory.ContainsKey(key))
                _memory[key] = value;
            else
                _memory.Add(key, value);
        }
        
        public string Recall(string key)
        {
            return _memory.ContainsKey(key) ? _memory[key] : string.Empty;
        }
        
        public void AddToHistory(string topic)
        {
            _topicHistory.Add(topic);
            if (_topicHistory.Count > 10)
                _topicHistory.RemoveAt(0);
        }
        
        public List<string> GetTopicHistory() => new List<string>(_topicHistory);
        
        public List<string> GetMemoryItems()
        {
            var items = new List<string>();
            foreach (var item in _memory)
            {
                items.Add($"{item.Key}: {item.Value}");
            }
            return items;
        }
    }
}