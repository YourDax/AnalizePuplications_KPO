using AnalizePuplicationsDataModels;
using AnalyzePublicationsDataModels.Enums;
using System.Security.AccessControl;

namespace AnalyzePublicationsDataModels.Models
{
    /// <summary>
    /// Источник публикации: журнал, конференция, репозиторий
    /// </summary>
    public interface ISourceModel : IId
    {
        /// <summary>
        /// Название источника
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Ссылка на сайт источника
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Тип источника
        /// </summary>
        SourceType SourceType { get; }

        /// <summary>
        /// Дата добавления источника
        /// </summary>
        DateTime DateCreated { get; }
    }
}
