using Assets._Asteroids.Logic.Gameplay;
using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.SaveProviders
{
    public class SaveConflictResolveRequest
    {
        public SaveData LocalSave { get; set; }
        public SaveData CloudSave { get; set; }

        public UniTaskCompletionSource<SaveData> CompletionSource = new();
    }
}