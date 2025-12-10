using AnalizePuplicationsDataModels;

namespace AnalyzePublicationsDataModels.Models
{
    /// <summary>
    /// Автор / исследователь
    /// </summary>
    public interface IAuthorModel : IId
    {
        /// <summary>
        /// Полное имя автора
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Организация или кафедра
        /// </summary>
        string Affiliation { get; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Дата добавления автора
        /// </summary>
        DateTime DateCreated { get; }
    }
}
