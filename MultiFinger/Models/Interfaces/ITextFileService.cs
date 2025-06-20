using MultiFinger.Models.Figures;

namespace MultiFinger.Models.Interfaces
{
    public interface ITextFileService
    {
        Task<Dictionary<FingerTrace, List<FigureBase>>> ParseFileAsync(string path);
        Task<bool> SaveFileAsync(string path);
    }
}
