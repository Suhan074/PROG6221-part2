using System;
using System.Collections.Generic;

namespace CybersecurityChatbotWPF.Services
{
    public class ResponseManager
    {
        private readonly Dictionary<string, string[]> _topicResponses;
        private readonly Random _random;
        private readonly string[] _randomTips;
        private readonly string[] _interestingFacts;

        public ResponseManager()
        {
            _random = new Random();
            
            _topicResponses = new Dictionary<string, string[]>
            {
                { "password", new string[] {
                    "🔐 Use strong, unique passwords with at least 12 characters including uppercase, lowercase, numbers, and symbols.",
                    "🔐 Never reuse passwords across different accounts. If one gets compromised, all your accounts are at risk!",
                    "🔐 Consider using a password manager to generate and store complex passwords securely.",
                    "🔐 Enable Two-Factor Authentication (2FA) whenever possible for an extra layer of security."
                }},
                { "phishing", new string[] {
                    "🎣 Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                    "🎣 Never click links in suspicious emails. Hover over them first to see where they really go.",
                    "🎣 Check the sender's email address carefully - scammers use slight misspellings like 'arnazon.com' instead of 'amazon.com'.",
                    "🎣 If an email creates urgency ('Your account will be closed!'), it's likely a phishing attempt."
                }},
                { "scam", new string[] {
                    "⚠️ If something sounds too good to be true, it probably is! Never share your OTP or PIN with anyone.",
                    "⚠️ Lottery, inheritance, and 'free money' scams are common in South Africa. Legitimate organisations never ask for fees to release prizes.",
                    "⚠️ Be wary of calls claiming you've won a competition you never entered.",
                    "⚠️ Never send money to someone you've only met online, no matter how convincing their story."
                }},
                { "privacy", new string[] {
                    "👤 Review your privacy settings on social media regularly. Share minimal personal information online.",
                    "👤 Be careful what you post publicly - birthdays, addresses, and location check-ins can be used by scammers.",
                    "👤 Use different email addresses for different purposes (shopping, social media, banking).",
                    "👤 Regularly check what information apps have access to on your phone."
                }},
                { "browsing", new string[] {
                    "🌐 Look for 'https://' and the padlock icon in your browser's address bar before entering personal information.",
                    "🌐 Avoid using public Wi-Fi for sensitive transactions like banking. Use a VPN if you must use public networks.",
                    "🌐 Keep your browser and extensions updated to protect against security vulnerabilities.",
                    "🌐 Clear your browsing cache and cookies regularly to remove tracking data."
                }},
                { "safety", new string[] {
                    "🛡️ Keep your software and operating system updated - updates often include important security patches.",
                    "🛡️ Back up your important files regularly to an external drive or cloud service.",
                    "🛡️ Be careful what you download - only use official app stores and trusted websites.",
                    "🛡️ Use antivirus software and keep it updated."
                }},
                { "email", new string[] {
                    "📧 Never open attachments from unknown senders - they may contain malware.",
                    "📧 Set up spam filters to reduce the number of phishing emails you receive.",
                    "📧 If an email looks suspicious, contact the organisation directly using a phone number you know is real.",
                    "📧 Report phishing emails to your email provider and to the company being impersonated."
                }}
            };
            
            _randomTips = new string[]
            {
                "💡 Use a passphrase instead of a password - something like 'BlueElephantJumps$High!'",
                "💡 Check if your email has been in a data breach at haveibeenpwned.com",
                "💡 Never use 'password', '123456', or 'qwerty' as your password - they're the most common!",
                "💡 Log out of accounts when you're done, especially on shared computers.",
                "💡 Be careful what you share on social media - scammers use this information for social engineering."
            };
            
            _interestingFacts = new string[]
            {
                "📊 95% of cybersecurity breaches are caused by human error.",
                "📊 Over 300,000 new pieces of malware are created every day worldwide.",
                "📊 The average person has over 100 passwords to remember!",
                "📊 A strong password can take centuries to crack, while 'password123' takes less than a second.",
                "📊 South Africa ranks in the top 10 countries for cybercrime victims."
            };
        }
        
        public string GetResponseForTopic(string topic, bool isFollowUp)
        {
            if (_topicResponses.ContainsKey(topic))
            {
                var responses = _topicResponses[topic];
                string response = responses[_random.Next(responses.Length)];
                
                if (isFollowUp)
                {
                    response += " Remember, staying informed is your best defence against cyber threats!";
                }
                
                return response;
            }
            
            return GetDefaultResponse();
        }
        
        public string GetRandomTip()
        {
            return _randomTips[_random.Next(_randomTips.Length)];
        }
        
        public string GetSimplifiedTip()
        {
            return "Here's a simple rule: If something feels wrong online, trust your gut. Don't click suspicious links, don't share passwords, and when in doubt, ask someone you trust for help.";
        }
        
        public string GetInterestingFact()
        {
            return _interestingFacts[_random.Next(_interestingFacts.Length)];
        }
        
        public string GetHelpMessage(string userName)
        {
            return $"Here's what I can help you with, {userName}:\n\n" +
                   "🔐 **Password Safety** - Ask me about creating strong passwords\n" +
                   "🎣 **Phishing** - Learn to spot fake emails and messages\n" +
                   "⚠️ **Scams** - Recognise common fraud schemes in South Africa\n" +
                   "👤 **Privacy** - Tips to protect your personal information\n" +
                   "🌐 **Safe Browsing** - How to browse the web securely\n" +
                   "📧 **Email Security** - Best practices for email safety\n\n" +
                   "Just type any of these topics, and I'll share helpful tips!";
        }
        
        public string GetDefaultResponse()
        {
            string[] defaults = {
                "I'm not sure I understand. Could you rephrase that? Try asking about passwords, phishing, safe browsing, privacy, or scams.",
                "I didn't quite catch that. You can ask me for help, or type a topic like 'password safety' or 'phishing tips'.",
                "Hmm, I'm still learning! Why not ask me about cybersecurity topics like online privacy or how to spot scams?"
            };
            return defaults[_random.Next(defaults.Length)];
        }
    }
}