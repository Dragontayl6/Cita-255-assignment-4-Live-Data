using System.Collections.ObjectModel;
using System.Text.Json;

namespace Cita_255_assignment_4_Live_Data
{
    public partial class MainPage : ContentPage
    {
        private readonly string path = Path.Combine(FileSystem.AppDataDirectory, "quests.json");
        private string newTitle = string.Empty;
        private string newGiver = string.Empty;
        private string newReward = string.Empty;
        private string message = string.Empty;

        public ObservableCollection<Quest> Quests { get; } = new();
        public Quest? SelectedQuest { get; set; }
        public string NewTitle
        {
            get => newTitle;
            set
            {
                newTitle = value;
                OnPropertyChanged();
            }
        }
        public string NewGiver
        {
            get => newGiver;
            set
            {
                newGiver = value;
                OnPropertyChanged();
            }
        }
        public string NewReward
        {
            get => newReward;
            set
            {
                newReward = value;
                OnPropertyChanged();
            }
        }
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            LoadQuests();
        }

        private void LoadQuests()
        {
            if (!File.Exists(path)) return;

            var json = File.ReadAllText(path);
            var savedQuests = JsonSerializer.Deserialize<List<Quest>>(json);
            if (savedQuests is null) return;

            foreach (var quest in savedQuests)
            {
                Quests.Add(quest);
            }
        }

        private void SaveQuests()
        {
            var json = JsonSerializer.Serialize(Quests);
            File.WriteAllText(path, json);
        }

        private void OnAddQuestClicked(object? sender, EventArgs e)
        {
            Message = string.Empty;

            if (string.IsNullOrWhiteSpace(NewTitle))
            {
                Message = "Please enter a quest title.";
                return;
            }

            if (!int.TryParse(NewReward, out var reward) || reward < 0)
            {
                Message = "Reward must be a non-negative number.";
                return;
            }

            Quests.Add(new Quest
            {
                Title = NewTitle.Trim(),
                Giver = NewGiver.Trim(),
                Reward = reward
            });
            SaveQuests();

            NewTitle = string.Empty;
            NewGiver = string.Empty;
            NewReward = string.Empty;
        }

        private void OnRemoveQuestClicked(object? sender, EventArgs e)
        {
            if (SelectedQuest is null)
            {
                Message = "Select a quest to remove.";
                return;
            }

            Quests.Remove(SelectedQuest);
            SelectedQuest = null;
            SaveQuests();
        }
    }
}
