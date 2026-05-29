using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CybersecurityChatbotWPF.Services;
using CybersecurityChatbotWPF.Models;

namespace CybersecurityChatbotWPF
{
    public partial class MainWindow : Window
    {
        private readonly ChatbotService _chatbotService;
        private readonly AudioService _audioService;

        public MainWindow()
        {
            InitializeComponent();
            
            _chatbotService = new ChatbotService();
            _audioService = new AudioService();
            
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Play voice greeting
            await _audioService.PlayGreetingAsync();
            
            // Show welcome message
            AddBotMessage("Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online.");
            await Task.Delay(500);
            AddBotMessage("What's your name?");
            
            // Focus on input
            MessageInput.Focus();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
                e.Handled = true;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            MessagesList.Items.Clear();
            AddBotMessage("Conversation cleared. How can I help you today?");
        }

        private async void QuickTopic_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                string topic = button.Content.ToString().Replace("🔐 ", "").Replace("🎣 ", "")
                    .Replace("🌐 ", "").Replace("👤 ", "").Replace("⚠️ ", "").Replace("❓ ", "");
                MessageInput.Text = topic;
                SendMessage();
            }
        }

        private async void SendMessage()
        {
            string userMessage = MessageInput.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                AddBotMessage("Please enter a message.");
                return;
            }

            // Add user message to chat
            AddUserMessage(userMessage);
            MessageInput.Clear();
            
            // Show typing indicator
            ShowTypingIndicator(true);
            
            // Process message and get response
            await Task.Delay(500); // Simulate thinking
            
            var response = _chatbotService.ProcessMessage(userMessage);
            
            ShowTypingIndicator(false);
            
            // Add bot response
            AddBotMessage(response.Response, response.Sentiment);
            
            // Update sidebar info
            UpdateSidebarInfo();
        }

        private void AddUserMessage(string message)
        {
            MessagesList.Items.Add(new ChatMessage
            {
                Sender = "You",
                Message = message,
                IsUser = true,
                Timestamp = DateTime.Now.ToString("HH:mm")
            });
            
            ScrollToBottom();
        }

        private void AddBotMessage(string message, string sentiment = "neutral")
        {
            MessagesList.Items.Add(new ChatMessage
            {
                Sender = "🤖 Cybersecurity Bot",
                Message = message,
                IsUser = false,
                Sentiment = sentiment,
                Timestamp = DateTime.Now.ToString("HH:mm")
            });
            
            ScrollToBottom();
        }

        private void ShowTypingIndicator(bool show)
        {
            if (show)
            {
                StatusText.Text = "✏️ Bot is typing...";
                StatusText.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFD700");
            }
            else
            {
                StatusText.Text = "🟢 Active";
                StatusText.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#27AE60");
            }
        }

        private void ScrollToBottom()
        {
            MessagesScrollViewer.ScrollToBottom();
        }

        private void UpdateSidebarInfo()
        {
            var userProfile = _chatbotService.GetUserProfile();
            var memory = _chatbotService.GetMemoryItems();
            var sentiment = _chatbotService.GetCurrentSentiment();
            
            UserNameText.Text = !string.IsNullOrEmpty(userProfile.Name) ? userProfile.Name : "Not logged in";
            UserInterestText.Text = !string.IsNullOrEmpty(userProfile.Interest) ? $"Interest: {userProfile.Interest}" : "No interests yet";
            
            SentimentText.Text = sentiment;
            
            if (memory.Count > 0)
            {
                MemoryText.Text = string.Join("\n• ", memory);
                MemoryText.Text = "• " + MemoryText.Text;
            }
            else
            {
                MemoryText.Text = "No memory items yet";
            }
        }
    }

    public class ChatMessage
    {
        public string Sender { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsUser { get; set; }
        public string Sentiment { get; set; } = "neutral";
        public string Timestamp { get; set; } = string.Empty;
    }

    // Converter for bool to HorizontalAlignment
    public class BoolToHorizontalAlignmentConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isUser && isUser)
                return HorizontalAlignment.Right;
            return HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter for user to bot background
    public class UserToBotBackgroundConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isUser && isUser)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4A90E2"));
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E3F2FD"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}