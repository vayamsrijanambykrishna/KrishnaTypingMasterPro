using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace KrishnaTypingMaster
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private Stopwatch stopwatch;
        private string sampleText = "Modern technology has transformed how we communicate and work. From smartphones to artificial intelligence, innovation continues to reshape our daily routines.";
        private int totalKeystrokes = 0;
        private int correctKeystrokes = 0;
        private bool isTesting = false;

        public MainWindow()
        {
            InitializeComponent();
            
            // टाइमर सेट अप (Timer Setup)
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            stopwatch = new Stopwatch();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameBox.Text))
            {
                MessageBox.Show("कृपया अपना नाम दर्ज करें!");
                return;
            }

            // लॉगिन स्क्रीन छुपाएं और टाइपिंग व्यू चालू करें (HTML View Switching Logic)
            LoginView.Visibility = Visibility.Hidden;
            LiveTypingView.Visibility = Visibility.Visible;
            SidebarTabs.Visibility = Visibility.Hidden;
            TestControls.Visibility = Visibility.Visible;
            
            StartTest();
        }

        private void StartTest()
        {
            isTesting = true;
            totalKeystrokes = 0;
            correctKeystrokes = 0;
            UserInputBox.Text = "";
            UserInputBox.IsReadOnly = false;
            
            // संदर्भ टेक्स्ट लोड करें
            RefTextParagraph.Inlines.Clear();
            RefTextParagraph.Inlines.Add(new Run(sampleText) { Foreground = Brushes.Black });
            
            stopwatch.Restart();
            timer.Start();
            UserInputBox.Focus();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopwatch.Elapsed;
            TimerDisplay.Text = $"{ts.Minutes:00}:{ts.Seconds:00}";
            UpdateLiveSpeed();
        }

        private void UserInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isTesting) return;

            string typedText = UserInputBox.Text;
            totalKeystrokes = typedText.Length;
            correctKeystrokes = 0;

            RefTextParagraph.Inlines.Clear();

            for (int i = 0; i < sampleText.Length; i++)
            {
                if (i < typedText.Length)
                {
                    if (typedText[i] == sampleText[i])
                    {
                        correctKeystrokes++;
                        RefTextParagraph.Inlines.Add(new Run(sampleText[i].ToString()) { Foreground = Brushes.Gray }); // HTML .completed-text
                    }
                    else
                    {
                        RefTextParagraph.Inlines.Add(new Run(sampleText[i].ToString()) { Background = Brushes.LightPink, Foreground = Brushes.Red }); // HTML .err-perm
                    }
                }
                else if (i == typedText.Length)
                {
                    RefTextParagraph.Inlines.Add(new Run(sampleText[i].ToString()) { Background = new SolidColorBrush(Color.FromRgb(232, 228, 247)), FontWeight = FontWeights.Bold }); // HTML .active-word
                }
                else
                {
                    RefTextParagraph.Inlines.Add(new Run(sampleText[i].ToString()) { Foreground = Brushes.Black });
                }
            }

            if (typedText.Length >= sampleText.Length)
            {
                FinishTest();
            }
        }

        private void UpdateLiveSpeed()
        {
            double minutes = stopwatch.Elapsed.TotalMinutes;
            if (minutes > 0)
            {
                double wpm = (correctKeystrokes / 5.0) / minutes;
                LiveSpeedText.Text = $"Live Speed: {Math.Round(wpm)} WPM";
            }
        }

        private void FinishTest()
        {
            isTesting = false;
            timer.Stop();
            stopwatch.Stop();
            UserInputBox.IsReadOnly = true;
            MessageBox.Show("टेस्ट पूरा हुआ!\n" + LiveSpeedText.Text, "बधाई हो कृष्ण!");
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

