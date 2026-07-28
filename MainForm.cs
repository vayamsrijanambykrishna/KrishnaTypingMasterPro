using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace KrishnaTypingMaster
{
    public class MainForm : Form
    {
        private RichTextBox displayTextBox;
        private TextBox typingTextBox;
        private Label statsLabel;
        private Label timerLabel;
        
        private string sampleText = "Typing accurately is a fundamental skill in the digital age. It improves communication speed and workflow efficiency significantly. Regular practice builds muscle memory, allowing you to type fast without looking at the keyboard. Always aim for high accuracy before trying to increase your raw speed.";
        
        private Stopwatch stopwatch;
        private System.Windows.Forms.Timer uiTimer;
        private bool isTypingStarted = false;
        private int totalKeystrokes = 0;
        private int correctKeystrokes = 0;

        public MainForm()
        {
            // विंडो का डिज़ाइन
            this.Text = "Krishna Typing Master Pro";
            this.Size = new Size(850, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // सैंपल टेक्स्ट बॉक्स
            displayTextBox = new RichTextBox();
            displayTextBox.Location = new Point(50, 50);
            displayTextBox.Size = new Size(730, 200);
            displayTextBox.ReadOnly = true;
            displayTextBox.Font = new Font("Segoe UI", 16);
            displayTextBox.Text = sampleText;
            displayTextBox.BackColor = Color.White;
            this.Controls.Add(displayTextBox);

            // टाइपिंग इनपुट बॉक्स
            typingTextBox = new TextBox();
            typingTextBox.Location = new Point(50, 280);
            typingTextBox.Size = new Size(730, 40);
            typingTextBox.Font = new Font("Segoe UI", 16);
            typingTextBox.TextChanged += new EventHandler(TypingTextBox_TextChanged);
            this.Controls.Add(typingTextBox);

            // आँकड़े (Stats) लेबल
            statsLabel = new Label();
            statsLabel.Location = new Point(50, 350);
            statsLabel.Size = new Size(400, 40);
            statsLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            statsLabel.ForeColor = Color.DarkBlue;
            statsLabel.Text = "Speed: 0 WPM | Accuracy: 100%";
            this.Controls.Add(statsLabel);

            // टाइमर लेबल
            timerLabel = new Label();
            timerLabel.Location = new Point(600, 350);
            timerLabel.Size = new Size(180, 40);
            timerLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            timerLabel.ForeColor = Color.DarkRed;
            timerLabel.Text = "Time: 00:00";
            this.Controls.Add(timerLabel);

            // टाइमर सेटअप
            stopwatch = new Stopwatch();
            uiTimer = new System.Windows.Forms.Timer();
            uiTimer.Interval = 1000; // 1 सेकंड
            uiTimer.Tick += UiTimer_Tick;
        }

        private void UiTimer_Tick(object? sender, EventArgs e)
        {
            TimeSpan ts = stopwatch.Elapsed;
            timerLabel.Text = $"Time: {ts.Minutes:00}:{ts.Seconds:00}";
            CalculateStats();
        }

        private void TypingTextBox_TextChanged(object? sender, EventArgs e)
        {
            string typedText = typingTextBox.Text;

            // अगर टाइपिंग शुरू हुई है, तो टाइमर चालू करें
            if (!isTypingStarted && typedText.Length > 0)
            {
                isTypingStarted = true;
                stopwatch.Start();
                uiTimer.Start();
            }

            // अगर यूज़र ने पूरा टेक्स्ट टाइप कर लिया है, तो रोक दें
            if (typedText.Length >= sampleText.Length)
            {
                stopwatch.Stop();
                uiTimer.Stop();
                typingTextBox.ReadOnly = true;
                CalculateStats();
                MessageBox.Show("टेस्ट पूरा हुआ! आपकी स्पीड: " + statsLabel.Text, "बधाई हो कृष्ण!");
                return;
            }

            UpdateTextColors(typedText);
        }

        private void UpdateTextColors(string typedText)
        {
            totalKeystrokes = typedText.Length;
            correctKeystrokes = 0;

            displayTextBox.SelectAll();
            displayTextBox.SelectionColor = Color.Black;
            displayTextBox.SelectionBackColor = Color.White;

            for (int i = 0; i < typedText.Length; i++)
            {
                displayTextBox.Select(i, 1);
                if (i < sampleText.Length && typedText[i] == sampleText[i])
                {
                    displayTextBox.SelectionColor = Color.Green;
                    displayTextBox.SelectionBackColor = Color.LightGreen;
                    correctKeystrokes++;
                }
                else
                {
                    displayTextBox.SelectionColor = Color.Red;
                    displayTextBox.SelectionBackColor = Color.LightPink;
                }
            }
            displayTextBox.Select(typedText.Length, 0); // कर्सर वापस सेट करें
            CalculateStats();
        }

        private void CalculateStats()
        {
            if (totalKeystrokes == 0) return;

            // सटीकता (Accuracy)
            double accuracy = ((double)correctKeystrokes / totalKeystrokes) * 100;

            // स्पीड (WPM - Words Per Minute) : 5 अक्षर = 1 शब्द माना जाता है
            double minutes = stopwatch.Elapsed.TotalMinutes;
            double wpm = 0;
            if (minutes > 0)
            {
                wpm = (correctKeystrokes / 5.0) / minutes;
            }

            statsLabel.Text = $"Speed: {Math.Round(wpm)} WPM | Accuracy: {Math.Round(accuracy)}%";
        }
    }
}

