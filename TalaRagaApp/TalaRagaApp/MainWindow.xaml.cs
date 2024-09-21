using Newtonsoft.Json;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;

using System.Windows;

namespace TalaRagaApp
{
    public partial class MainWindow : Window
    {
        private List<Raga> ragas;
        private List<Tala> talas;

        // Dictionary to map notes (Swaras) to frequencies
        private Dictionary<string, float> noteFrequencies = new Dictionary<string, float>
        {
            { "S", 261.63f },  // Sa (C)
            { "R1", 277.18f }, // Shuddha Rishabham
            { "G3", 329.63f }, // Antara Gandharam
            { "M1", 349.23f }, // Shuddha Madhyamam
            { "P", 392.00f },  // Panchamam
            { "D1", 415.30f }, // Shuddha Dhaivatam
            { "N3", 493.88f }  // Kakali Nishadam
        };

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        // Load Tala and Raga data from JSON
        private void LoadData()
        {
            // Load Ragas
            string ragaJson = File.ReadAllText("Resources/ragas.json");
            ragas = JsonConvert.DeserializeObject<List<Raga>>(ragaJson);
            ragaComboBox.ItemsSource = ragas;
            ragaComboBox.DisplayMemberPath = "Name";

            // Load Talas
            string talaJson = File.ReadAllText("Resources/talas.json");
            talas = JsonConvert.DeserializeObject<List<Tala>>(talaJson);
            talaComboBox.ItemsSource = talas;
            talaComboBox.DisplayMemberPath = "Name";
        }

        // Handle Play Button Click
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRaga = (Raga)ragaComboBox.SelectedItem;
            var selectedTala = (Tala)talaComboBox.SelectedItem;
            string arrangedNotes = notesTextBox.Text;

            if (selectedRaga != null && selectedTala != null && !string.IsNullOrEmpty(arrangedNotes))
            {
                PlayNotes(selectedRaga, selectedTala, arrangedNotes);
            }
            else
            {
                statusTextBlock.Text = "Please select a Raga, Tala and arrange the notes!";
            }
        }

        // Play Notes Based on Tala
        private void PlayNotes(Raga raga, Tala tala, string arrangedNotes)
        {
            var notes = arrangedNotes.Split(' '); // Split arranged notes by space
            int beatDuration = 500; // Set a base duration for each beat

            foreach (var note in notes)
            {
                if (noteFrequencies.ContainsKey(note))
                {
                    PlayFrequency(noteFrequencies[note], beatDuration);
                }
                else
                {
                    statusTextBlock.Text = "Invalid note in arrangement.";
                    break;
                }
            }
        }

        // Generate Sound with NAudio
        private void PlayFrequency(float frequency, int duration)
        {
            var waveOut = new WaveOutEvent();
            var signalGenerator = new NAudio.Wave.SampleProviders.SignalGenerator()
            {
                Gain = 0.2,
                Frequency = frequency,
                Type = NAudio.Wave.SampleProviders.SignalGeneratorType.Sin
            };
            waveOut.Init(signalGenerator.Take(TimeSpan.FromMilliseconds(duration)));
            waveOut.Play();
            System.Threading.Thread.Sleep(duration); // Pause for the duration of the note
        }
    }

    // Raga class
    public class Raga
    {
        public string Name { get; set; }
        public List<string> Notes { get; set; }
    }

    // Tala class
    public class Tala
    {
        public string Name { get; set; }
        public List<int> BeatPattern { get; set; }
    }
}
