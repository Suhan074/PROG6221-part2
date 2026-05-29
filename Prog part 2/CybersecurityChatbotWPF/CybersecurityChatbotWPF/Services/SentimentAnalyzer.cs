using System;
using System.Collections.Generic;

namespace CybersecurityChatbotWPF.Services
{
    public class SentimentAnalyzer
    {
        private readonly Dictionary<string, List<string>> _sentimentKeywords;
        private string _lastSentiment = "neutral";

        public SentimentAnalyzer()
        {
            _sentimentKeywords = new Dictionary<string, List<string>>
            {
                { "worried", new List<string> { "worried", "scared", "afraid", "nervous", "anxious", "concerned", "frightened" } },
                { "frustrated", new List<string> { "frustrated", "annoyed", "angry", "upset", "tired", "exhausted", "confused" } },
                { "curious", new List<string> { "curious", "interested", "tell me", "explain", "learn", "teach", "how do" } },
                { "happy", new List<string> { "happy", "great", "awesome", "fantastic", "wonderful", "good", "excellent" } },
                { "sad", new List<string> { "sad", "depressed", "down", "unhappy", "terrible", "awful" } }
            };
        }

        public string DetectSentiment(string userInput)
        {
            string lowerInput = userInput.ToLower();
            
            foreach (var sentiment in _sentimentKeywords)
            {
                foreach (var keyword in sentiment.Value)
                {
                    if (lowerInput.Contains(keyword))
                    {
                        _lastSentiment = sentiment.Key;
                        return sentiment.Key;
                    }
                }
            }
            
            _lastSentiment = "neutral";
            return "neutral";
        }

        public string GetLastSentiment() => _lastSentiment;
        
        public string GetSentimentEmoji(string sentiment)
        {
            return sentiment switch
            {
                "worried" => "😟",
                "frustrated" => "😤",
                "curious" => "🤔",
                "happy" => "😊",
                "sad" => "😢",
                _ => "😐"
            };
        }
    }
}