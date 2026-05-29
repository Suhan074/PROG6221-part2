using System;
using System.Collections.Generic;
using System.Linq;
using CybersecurityChatbotWPF.Models;

namespace CybersecurityChatbotWPF.Services
{
    public class ChatbotService
    {
        private readonly ResponseManager _responseManager;
        private readonly SentimentAnalyzer _sentimentAnalyzer;
        private UserProfile _userProfile;
        private ConversationContext _context;
        private string _lastTopic;
        private readonly Random _random;

        public ChatbotService()
        {
            _responseManager = new ResponseManager();
            _sentimentAnalyzer = new SentimentAnalyzer();
            _userProfile = new UserProfile();
            _context = new ConversationContext();
            _random = new Random();
        }

        public ChatResponse ProcessMessage(string userInput)
        {
            string lowerInput = userInput.ToLower().Trim();
            
            // Detect sentiment
            string sentiment = _sentimentAnalyzer.DetectSentiment(lowerInput);
            
            // Handle name capture
            if (string.IsNullOrEmpty(_userProfile.Name) && !lowerInput.Contains("name"))
            {
                _userProfile.Name = userInput;
                _context.Remember("user_name", userInput);
                return new ChatResponse
                {
                    Response = $"Nice to meet you, {_userProfile.Name}! I'm your Cybersecurity Awareness Assistant. You can ask me about passwords, phishing, safe browsing, privacy, or scams. What would you like to learn about?",
                    Sentiment = sentiment
                };
            }
            
            // Check for follow-up requests
            if (lowerInput.Contains("another") || lowerInput.Contains("more") || lowerInput.Contains("again"))
            {
                if (!string.IsNullOrEmpty(_lastTopic))
                {
                    var followUpResponse = _responseManager.GetResponseForTopic(_lastTopic, true);
                    return new ChatResponse
                    {
                        Response = followUpResponse,
                        Sentiment = sentiment
                    };
                }
            }
            
            // Check for topic interest (memory)
            if (lowerInput.Contains("interested in") || lowerInput.Contains("like to learn about"))
            {
                string[] topics = { "password", "phishing", "privacy", "scam", "browsing" };
                foreach (var topic in topics)
                {
                    if (lowerInput.Contains(topic))
                    {
                        _userProfile.Interest = topic;
                        _context.Remember("user_interest", topic);
                        return new ChatResponse
                        {
                            Response = $"Great! I'll remember that you're interested in {topic}. It's a crucial part of staying safe online. " +
                                      _responseManager.GetResponseForTopic(topic, false),
                            Sentiment = sentiment
                        };
                    }
                }
            }
            
            // Check for sentiment-based responses
            if (sentiment == "worried" || sentiment == "scared")
            {
                return new ChatResponse
                {
                    Response = "I understand your concern. Cybersecurity can feel overwhelming, but don't worry! Let me share some simple tips to help you feel more secure. " +
                              _responseManager.GetRandomTip(),
                    Sentiment = sentiment
                };
            }
            
            if (sentiment == "frustrated")
            {
                return new ChatResponse
                {
                    Response = "I hear your frustration. Let me help make this easier for you. " +
                              _responseManager.GetSimplifiedTip(),
                    Sentiment = sentiment
                };
            }
            
            if (sentiment == "curious")
            {
                return new ChatResponse
                {
                    Response = "I'm glad you're curious! That's the best attitude for learning cybersecurity. Let me share something interesting with you. " +
                              _responseManager.GetInterestingFact(),
                    Sentiment = sentiment
                };
            }
            
            // Check for specific topics
            var topics = new Dictionary<string, string>
            {
                { "password", "password" },
                { "phish", "phishing" },
                { "scam", "scam" },
                { "privacy", "privacy" },
                { "brows", "browsing" },
                { "safe", "safety" },
                { "email", "email" }
            };
            
            foreach (var topic in topics)
            {
                if (lowerInput.Contains(topic.Key))
                {
                    _lastTopic = topic.Value;
                    _context.AddToHistory(topic.Value);
                    return new ChatResponse
                    {
                        Response = _responseManager.GetResponseForTopic(topic.Value, false),
                        Sentiment = sentiment
                    };
                }
            }
            
            // Check for greetings
            if (lowerInput.Contains("hello") || lowerInput.Contains("hi") || lowerInput == "hey")
            {
                string[] greetings = {
                    $"Hello {_userProfile.Name}! How can I help you with cybersecurity today?",
                    $"Hi {_userProfile.Name}! Ready to learn about online safety?",
                    $"Hey {_userProfile.Name}! What cybersecurity topic would you like to explore?"
                };
                return new ChatResponse
                {
                    Response = greetings[_random.Next(greetings.Length)],
                    Sentiment = sentiment
                };
            }
            
            // Check for thanks
            if (lowerInput.Contains("thank") || lowerInput.Contains("thanks"))
            {
                string[] thanksResponses = {
                    $"You're welcome, {_userProfile.Name}! Stay safe online! 😊",
                    $"My pleasure! Remember, I'm always here if you have more questions.",
                    $"Glad I could help, {_userProfile.Name}! Cybersecurity is everyone's responsibility."
                };
                return new ChatResponse
                {
                    Response = thanksResponses[_random.Next(thanksResponses.Length)],
                    Sentiment = sentiment
                };
            }
            
            // Check for help
            if (lowerInput == "help" || lowerInput == "what can you do")
            {
                return new ChatResponse
                {
                    Response = _responseManager.GetHelpMessage(_userProfile.Name),
                    Sentiment = sentiment
                };
            }
            
            // Default response with personalized touch
            string defaultResponse = _responseManager.GetDefaultResponse();
            if (!string.IsNullOrEmpty(_userProfile.Interest))
            {
                defaultResponse += $" As someone interested in {_userProfile.Interest}, you might want to review the security settings on your accounts regularly.";
            }
            
            return new ChatResponse
            {
                Response = defaultResponse,
                Sentiment = sentiment
            };
        }

        public UserProfile GetUserProfile() => _userProfile;
        
        public List<string> GetMemoryItems() => _context.GetMemoryItems();
        
        public string GetCurrentSentiment() => _sentimentAnalyzer.GetLastSentiment();
    }

    public class ChatResponse
    {
        public string Response { get; set; } = string.Empty;
        public string Sentiment { get; set; } = string.Empty;
    }
}